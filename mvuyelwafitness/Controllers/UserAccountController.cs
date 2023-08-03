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
    public class UserAccountController : Controller
    {
        private db_a98f3b_mvuyelwafitnessdbEntities vuvuDB = new db_a98f3b_mvuyelwafitnessdbEntities();
        // GET: UserAccount
        public ActionResult ViewAccount(string email)
        {

            if (email == null )
            {
                return RedirectToAction("Login","UserAccount");
            }
            else
            {
                List<Customer> cust = vuvuDB.Customers.Where(c => c.Email == email.ToLower()).ToList();

                var ordervm = new OrderVM();
                ordervm.myCustomer = vuvuDB.Customers.Where(c => c.Email == email.ToLower()).ToList();
                ordervm.myOrders = vuvuDB.Orders.ToList();
                ordervm.myOrdItem = vuvuDB.Order_Item.ToList();
                ordervm.MyAddresses = vuvuDB.Addresses.ToList();
                return View(ordervm);
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                //Convert the hashed bytes to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public ActionResult AddAccount()
        {
            return View();
        }

        public string CreateAccount(string name, string surname, string email, string password, string phone)
        {
            string passHash = HashPassword(password);

            //date 
            DateTime date = DateTime.Now;
            var fdate = date.ToString("dddd, dd MMMM yyyy HH:mm:ss");

            //form reset key
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);

            List<Customer> emp = vuvuDB.Customers.Where(e => e.Email == email).ToList();

            if (email != null)
            {
                vuvuDB.Customers.Add(new Customer
                {
                    Name = name,
                    Email = email,
                    Surname = surname,
                    Password = passHash,
                    Phone = phone,
                    Date_Created = Convert.ToDateTime(fdate),
                    Reset_key = random.ToString()
                });

                try
                {
                    vuvuDB.SaveChanges();
                    return JsonConvert.SerializeObject(new { message = "User account created." });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { message = "Failed, Cannot create account. Server error! Please contact Support"});
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "User already exists" });
            }
        }

        public string AddAddress(string email, string strAddress, string town, string province, string zip)
        {

            var mycust = vuvuDB.Customers.Where(c => c.Email == email).FirstOrDefault();

            if (mycust != null)
            {
                vuvuDB.Addresses.Add(new Address
                {
                   CUSTOMER_ID = mycust.CUSTOMER_ID,
                   Street_add = strAddress,
                   Town_City = town,
                   Province = province,
                   Zip = zip
                });

                try
                {
                    vuvuDB.SaveChanges();
                    return JsonConvert.SerializeObject(new { message = "Address added." });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { message = "Failed, Cannot add address. Server error! Please contact Support" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Customer not avialable exists" });
            }
        }

        //Edit Account method 
        public string EditAccount(string name, string surname, string email, string phone, string loginEmail)
        {
            var custToEdit = vuvuDB.Customers.Where(c => c.Email == loginEmail.ToLower()).FirstOrDefault();
            if (custToEdit != null)
            {
                custToEdit.Email = email;
                custToEdit.Name = name;
                custToEdit.Surname = surname;
                custToEdit.Phone = phone;
                try
                {
                    vuvuDB.SaveChanges();
                    return JsonConvert.SerializeObject(new { message = "Account changes saved successfully." });
                }
                catch (Exception)
                {
                    return JsonConvert.SerializeObject(new { message = "Changes cannot be made to account, please contact support https://mvuyelwafitness.co.za/Contact." });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "User not found, Please check email entered!" });
            }
        }

        public string EditAddress(int addId, string strAddress, string town, string province, string zip)
        {
            //var mycust = vuvuDB.Customers.Where(c => c.Email == email).FirstOrDefault();
            var addressToEdit = vuvuDB.Addresses.Where(c => c.ADDRESS_ID == addId).FirstOrDefault();

            if (addressToEdit != null)
            {
                addressToEdit.Street_add = strAddress;
                addressToEdit.Town_City = town;
                addressToEdit.Province = province;
                addressToEdit.Zip = zip;

                try
                {
                    vuvuDB.SaveChanges();
                    return JsonConvert.SerializeObject(new { message = "Address changes saved successfully." });
                }
                catch (Exception)
                {
                    return JsonConvert.SerializeObject(new { message = "Changes cannot be made to account, please contact support https://mvuyelwafitness.co.za/Contact." });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Address not found, Please check details or contact support!" });
            }
        }

        public string RemoveAddress(int addressId)
        {
            var itemToDel = vuvuDB.Addresses.Where(a => a.ADDRESS_ID == addressId).FirstOrDefault();

            if (itemToDel != null)
            {
                vuvuDB.Addresses.Remove(itemToDel);

                try
                {
                    vuvuDB.SaveChanges();
                    return JsonConvert.SerializeObject(new { message = "success" });
                }
                catch (Exception)
                {
                    return JsonConvert.SerializeObject(new { message = "Failed, Please contact support." });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Failed, Please contact support." });
            }
        }
        public string UserLogin(string email, string password)
        {
            List<Customer> cust = vuvuDB.Customers.Where(e => e.Email.Contains(email.ToLower())).ToList();

            string passHash = HashPassword(password);

            if (cust.Count != 0)
            {
                if (cust[0].Password == passHash)
                {
                    return JsonConvert.SerializeObject(new { email = cust[0].Email });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { message = "Incorrect Password / Email!" });

                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Email does not exist!" });

            }
        }

        public string ResetcustPass(string email, string resetkey, string password)
        {
            string passHash = HashPassword(password);

            List<Customer> cust = vuvuDB.Customers.Where(e => e.Email.Contains(email)).ToList();

            var custToupdate = vuvuDB.Customers.Where(x => x.Email == email).FirstOrDefault();

            if (custToupdate != null)
            {
                if (resetkey == custToupdate.Reset_key)
                {
                    custToupdate.Password = passHash;

                    var myessage = "";
                    try
                    {
                        vuvuDB.SaveChanges();
                        myessage = "Password for " + custToupdate.Name.Replace(" ", "") + " has been successfully reset!";
                    }
                    catch (Exception)
                    {
                        myessage = custToupdate.Name.Replace(" ", "") + "'s password cannot be reset. Reset key is invalid!!";
                    }

                    return JsonConvert.SerializeObject(new { message = myessage });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { message = "Reset Key is invalid!" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Email doesn't match any records!" });
            }
        }

        public string ResetKey(string email)
        {
            //generate random number and save to db as reset key
            var custTOgenResetKey = vuvuDB.Customers.Where(e => e.Email.Contains(email.ToLower())).FirstOrDefault();
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);

            if (custTOgenResetKey != null)
            {
                custTOgenResetKey.Reset_key = randomNumber.ToString();
                try
                {
                    vuvuDB.SaveChanges();
                }
                catch (Exception)
                {
                    return JsonConvert.SerializeObject(new { message = "Reset key not been created for: " + email });
                }
            }

            //send the reset They to the customer
            List<Customer> cust = vuvuDB.Customers.Where(e => e.Email.Contains(email.ToLower())).ToList();

            if (cust != null)
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    //create the mail message 
                    MailMessage mail = new MailMessage();

                    //set the addresses 
                    mail.From = new MailAddress("noreply@mvuyelwafitness.co.za"); //IMPORTANT: This must be same as your smtp authentication address.
                    mail.To.Add(email);

                    //set the content 
                    mail.Subject = "Mvuyelwafitness.co.za Account password reset";
                    mail.Body = "Hello " + cust[0].Name + "\nThis is you reset key: " + cust[0].Reset_key;
                    //send the message 
                    SmtpClient smtp = new SmtpClient("mail.mvuyelwafitness.co.za");

                    //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                    NetworkCredential Credentials = new NetworkCredential("noreply@mvuyelwafitness.co.za", "vuvu@2003");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = Credentials;
                    smtp.Port = 25;    //alternative port number is 8889
                    smtp.EnableSsl = false;
                    smtp.Send(mail);

                    return JsonConvert.SerializeObject(new { message = "Reset key has been sent to you email: " + email });
                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject(new { message = "Failed, Reset key not sent! ", err = ex });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Failed, useremail does not exists!" });
            }
        }
       

        public string ContactUs(string name, string email, string subject, string message)
        {
            //date 
            DateTime date = DateTime.Now;
            var fdate = date.ToString("dddd, dd MMMM yyyy HH:mm:ss");

            if (name != null || email != null)
            {
                if (message != null)
                {
                    vuvuDB.ContactForms.Add(new ContactForm
                    {
                        Name = name,
                        Email = email,
                        Subject = subject,
                        Message = message,
                        Date = Convert.ToDateTime(fdate)
                    });
                    try
                    {
                        vuvuDB.SaveChanges();
                        return JsonConvert.SerializeObject(new { message = "Message sent" });
                    }
                    catch (Exception)
                    {
                        return JsonConvert.SerializeObject(new { message = "Failed to send message, please contact support https://mvuyelwafitness.co.za/Contact" });
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(new { message = "Please fill in all required fields" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Please fill in all required fields" });
            }

        }
    }
}