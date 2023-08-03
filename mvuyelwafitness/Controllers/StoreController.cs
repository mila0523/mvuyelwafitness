using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using mvuyelwafitness.Models;


namespace mvuyelwafitness.Controllers
{
    public class StoreController : Controller
    {
        private db_a98f3b_mvuyelwafitnessdbEntities vuvuDB = new db_a98f3b_mvuyelwafitnessdbEntities();
        // GET: Store
        public ActionResult Store(string filterString)
        {
            
            if (filterString == null)
            {
                var myProducts = new Products_VM();
                myProducts.Categories = vuvuDB.Products.ToList();
                myProducts.ProductByFilter = vuvuDB.Products.ToList();

                return View(myProducts);
            }
            else
            {
                var myProducts = new Products_VM();
                myProducts.Categories = vuvuDB.Products.ToList();
                myProducts.ProductByFilter = vuvuDB.Products.Where(p => p.Name.Contains(filterString) || p.Category.Contains(filterString) || p.Description.Contains(filterString)).ToList();

                return View(myProducts);
            }                  
        }

        public ActionResult ProductDetails(string prodId)
        {
            List<Product> product = vuvuDB.Products.Where(p => p.PROD_ID == prodId).ToList();
            return View(product);
        }

        public string AddTOCart(string email, string productID)
        {
            var myCust = vuvuDB.Customers.Where(c => c.Email == email).FirstOrDefault();

            if(email != null && productID != null)
            {
                vuvuDB.Cart_Item.Add(new Cart_Item
                {
                    CUSTOMER_ID = myCust.CUSTOMER_ID,
                    PROD_ID = productID,
                    Quantity = 1,
                });

                try
                {
                    vuvuDB.SaveChanges();
                    return JsonConvert.SerializeObject(new { message = "Added to cart." });
                }
                catch (Exception)
                {
                    return JsonConvert.SerializeObject(new { message = "Failed to add." });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Failed to add to cart. You are not logged in! Please log in." });
            }         
        }

        public string AddToWishList(string email, string productID)
        {
            var myCust = vuvuDB.Customers.Where(c => c.Email == email).FirstOrDefault();

            if (email != null && productID != null)
            {
                vuvuDB.Cust_Wishlist.Add(new Cust_Wishlist
                {
                    CUSTOMER_ID = myCust.CUSTOMER_ID,
                    PROD_ID = productID,
                });

                try
                {
                    vuvuDB.SaveChanges();
                    return JsonConvert.SerializeObject(new { message = "Added to Wishlist." });
                }
                catch (Exception)
                {
                    return JsonConvert.SerializeObject(new { message = "Failed to add." });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Failed to add to Wishlist. You are not logged in! Please log in." });
            }
        }

        public string modQuantity(string email, string productID, int newQty)
        {
            var myCust = vuvuDB.Customers.Where(c => c.Email == email).FirstOrDefault();
            var qtyToAdjust = vuvuDB.Cart_Item.Where(i => i.CUSTOMER_ID == myCust.CUSTOMER_ID && i.PROD_ID == productID).FirstOrDefault(); 

            if(email != null && productID != null)
            {
                if(myCust != null && qtyToAdjust != null)
                {
                    qtyToAdjust.Quantity = newQty;
                    try
                    {
                        vuvuDB.SaveChanges();
                        return JsonConvert.SerializeObject(new { message = "Qty Changed." });
                    }
                    catch (Exception)
                    {
                        return JsonConvert.SerializeObject(new { message = "Qty Change failed!" });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new { message = "Qty Change failed!" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Qty Change failed!" });
            }
        }

        public ActionResult Cart(string email)
        {
            var cartVM = new CartVm();

            if (email != null)
            {
                var myCust = vuvuDB.Customers.Where(c => c.Email == email).FirstOrDefault();

                cartVM.Cart = vuvuDB.Cart_Item.Where(cr => cr.CUSTOMER_ID == myCust.CUSTOMER_ID).ToList();
                cartVM.myCustomer = vuvuDB.Customers.Where(c => c.Email == email).ToList();
                cartVM.Prod = vuvuDB.Products.ToList();

                return View(cartVM);
            }
            else
            {
                return View();
            }        
        }

        public string RemoveFromCart(string email, string productID)
        {
            var myCust = vuvuDB.Customers.Where(c => c.Email == email).FirstOrDefault();
            var itemToDel = vuvuDB.Cart_Item.Where(i => i.CUSTOMER_ID == myCust.CUSTOMER_ID && i.PROD_ID == productID).FirstOrDefault();

            if (itemToDel != null)
            {
                vuvuDB.Cart_Item.Remove(itemToDel);

                try
                {
                    vuvuDB.SaveChanges();
                    return JsonConvert.SerializeObject(new { message = "success"});
                }
                catch (Exception)
                {
                    return JsonConvert.SerializeObject(new { message = "Failed" });
                }              
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Failed" });
            }
           
        }

        public string RemoveFromWishList(int ListID)
        {
            var itemToDel = vuvuDB.Cust_Wishlist.Where(i => i.LIST_ID == ListID).FirstOrDefault();

            if (itemToDel != null)
            {
                vuvuDB.Cust_Wishlist.Remove(itemToDel);

                try
                {
                    vuvuDB.SaveChanges();
                    return JsonConvert.SerializeObject(new { message = "success" });
                }
                catch (Exception)
                {
                    return JsonConvert.SerializeObject(new { message = "Failed" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Failed" });
            }
        }

        
        public string CreatenewOrder(string orderTotal, int? custId, string session, string maddress)
        {
            var customID = 0;
            if (custId != null)
            {
                customID = Convert.ToInt32(custId);
            }
            else
            {
                customID = -1;
            }

            var customer = vuvuDB.Customers.Where(c => c.CUSTOMER_ID == customID).FirstOrDefault();

            List<Product> prod = vuvuDB.Products.ToList();
            //date 
            DateTime date = DateTime.Now;
            var fdate = date.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            var newdate = Convert.ToDateTime(fdate);
            var nsession = Convert.ToInt32(session);

            var exiOrder = vuvuDB.Orders.Where(o => o.CUSTOMER_ID == custId && o.SESSION_ID == nsession).FirstOrDefault();

            if (exiOrder == null)
            {
                if (customer != null)
                {
                    //first create order
                    vuvuDB.Orders.Add(new Order
                    {
                        CUSTOMER_ID = customID,
                        Date = Convert.ToDateTime(fdate),
                        Total = Convert.ToDecimal(orderTotal),
                        DEL_STATUS_ID = 3,
                        STATUS_ID = 2,
                        SESSION_ID = nsession
                    });

                    try
                    {
                        vuvuDB.SaveChanges();
                        //addItemsToOrder(customID, fdate);
                        //SendConfirmEmail(customID, fdate, maddress);
                        return JsonConvert.SerializeObject(new { message = "success" });

                    }
                    catch (Exception)
                    {
                        return JsonConvert.SerializeObject(new { message = "Failed to save!" });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new { message = "Failed, No customer found!" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Exists" });
            }
        }

        public string addItemsToOrder(int custId, string date)
        {
            //var customID = 0;
            //if (custId != null)
            //{
            //    customID = Convert.ToInt32(custId);
            //}
            //else
            //{
            //    customID = -1;
            //}
            List<Cart_Item> itemstodelincart = vuvuDB.Cart_Item.Where(cr => cr.CUSTOMER_ID == custId).ToList();
            var order = vuvuDB.Orders.Where(o => o.CUSTOMER_ID == custId && o.Date == Convert.ToDateTime(date)).FirstOrDefault();

            if (itemstodelincart != null && order != null)
            {
                //add cart items to order_item table
                foreach (var crtItem in itemstodelincart)
                {
                    vuvuDB.Order_Item.Add(new Order_Item
                    {
                        ORDER_NUM = order.ORDER_NUM,
                        PROD_ID = crtItem.PROD_ID,
                        Quantity = crtItem.Quantity
                    });

                    try
                    {
                        vuvuDB.SaveChanges();

                        //remove item from cart
                        vuvuDB.Cart_Item.Remove(crtItem);
                        vuvuDB.SaveChanges();
                        return JsonConvert.SerializeObject(new { message = "success" });
                    }
                    catch (Exception)
                    {
                        return JsonConvert.SerializeObject(new { message = "Failed" });
                    }
                }
                return JsonConvert.SerializeObject(new { message = "Failed" });
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Failed" });
            }
        }

        public string SendConfirmEmail(int custId, string mydate, string address)
        {

            var customer = vuvuDB.Customers.Where(c => c.CUSTOMER_ID == custId).FirstOrDefault();

            var order = vuvuDB.Orders.Where(o => o.CUSTOMER_ID == custId && o.Date == Convert.ToDateTime(mydate)).FirstOrDefault();

            List<Order_Item> ord_Itm = vuvuDB.Order_Item.Where(ot => ot.ORDER_NUM == order.ORDER_NUM).ToList();
            //string products = "";
            List<Product> prods = new List<Product>();
            foreach (var oitem in ord_Itm)
            {
                prods = vuvuDB.Products.Where(p => p.PROD_ID == oitem.PROD_ID).ToList();
                //products = prods.Join(", ", prods);
            }


            if (customer != null)
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    //create the mail message 
                    MailMessage mail = new MailMessage();

                    //set the addresses 
                    mail.From = new MailAddress("noreply@mvuyelwafitness.co.za"); //IMPORTANT: This must be same as your smtp authentication address.
                    mail.CC.Add("admin@mvuyelwafitness.co.za");
                    mail.To.Add("support@mvuyelwafitness.co.za");

                    //set the content 
                    mail.Subject = "New Order";
                    mail.Body = "Hello \nThis is " + customer.Name + "'s new order." + "\n\nTheir Email: " + customer.Email +
                        "\nDelivery Address: " + address +
                        "\nPayment Status: " + order.Payment_Statuses +
                        "\nCustomer's order items are as follows:\n\n" + prods;
                    //send the message 
                    SmtpClient smtp = new SmtpClient("mail.mvuyelwafitness.co.za");

                    //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                    NetworkCredential Credentials = new NetworkCredential("noreply@mvuyelwafitness.co.za", "vuvu@2003");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = Credentials;
                    smtp.Port = 25;    //alternative port number is 8889
                    smtp.EnableSsl = false;
                    smtp.Send(mail);

                    return JsonConvert.SerializeObject(new { message = "Sent" });
                }
                catch (Exception)
                {
                    return JsonConvert.SerializeObject(new { message = "Failed" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Failed" });
            }

        }
        public ActionResult Wishlist(int custId)
        {
            var products = new Products_VM();
            products.CustWishlist = vuvuDB.Cust_Wishlist.Where(p => p.CUSTOMER_ID == custId).ToList();
            products.ProductByFilter = vuvuDB.Products.ToList();

            return View(products);
        }

        public ActionResult Checkout(int custId)
        {
            var checkoutItems = new CartVm();
            checkoutItems.Cart = vuvuDB.Cart_Item.Where(c => c.CUSTOMER_ID == custId).ToList();
            checkoutItems.Deliveryaddress = vuvuDB.Addresses.Where(a => a.CUSTOMER_ID == custId).ToList();
            checkoutItems.Prod = vuvuDB.Products.ToList();
            checkoutItems.myCustomer = vuvuDB.Customers.Where(c => c.CUSTOMER_ID == custId).ToList();

            return View(checkoutItems);
        }
    }
}