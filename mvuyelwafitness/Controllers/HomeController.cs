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
    public class HomeController : Controller
    {
        private db_a98f3b_mvuyelwafitnessdbEntities vuvuDB = new db_a98f3b_mvuyelwafitnessdbEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public string RequestCart(string email)
        {
            if(email != null)
            {
                var cust = vuvuDB.Customers.Where(c => c.Email == email).FirstOrDefault();
                List<Cart_Item> crtItems = vuvuDB.Cart_Item.Where(i => i.CUSTOMER_ID == cust.CUSTOMER_ID).ToList();

                int? qty = 0;
                foreach(var itm in crtItems)
                {
                    qty += itm.Quantity;
                }

                return JsonConvert.SerializeObject(new { cartQty = qty });
            }
            else
            {
                return JsonConvert.SerializeObject(new { cartQty = 0 });
            }
            
        }
        public string ContactUs(string email, string name, string subject, string message)
        {
            //date 
            DateTime date = DateTime.Now;
            var fdate = date.ToString("dddd, dd MMMM yyyy HH:mm:ss");

            if (email != null || name != null)
            {
                vuvuDB.ContactForms.Add(new ContactForm
                {
                    Date = Convert.ToDateTime(fdate),
                    Email = email,
                    Name = name,
                    Subject = subject,
                    Message = message,
                });

                try
                {
                    vuvuDB.SaveChanges();

                    SendEmail(email, name, subject, message);

                    return JsonConvert.SerializeObject(new { message = "Message sent" });
                }
                catch (Exception)
                {
                    return JsonConvert.SerializeObject(new { message = "Failed to send. Please check your network connection or email support@mvuyelwafitness.co.za for assistance." });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Please ensure all fields are filled in." });

            }
        }

        public string SendEmail(string email, string name, string subject, string message)
        {
           
            if (email != null)
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    //create the mail message 
                    MailMessage mail = new MailMessage();

                    //set the addresses 
                    mail.From = new MailAddress("noreply@mvuyelwafitness.co.za"); //IMPORTANT: This must be same as your smtp authentication address.
                    mail.CC.Add(email);
                    mail.To.Add("support@mvuyelwafitness.co.za");

                    //set the content 
                    mail.Subject = "Support message sent: " + subject;
                    mail.Body = "Hello \n" + name + " sent a support message." + "\n\nTheir Email: " + email+ "\nSubject: "+ subject +"\n\nTheir message: \n" + message;
                    //send the message 
                    SmtpClient smtp = new SmtpClient("mail.mvuyelwafitness.co.za");

                    //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                    NetworkCredential Credentials = new NetworkCredential("noreply@mvuyelwafitness.co.za", "vuvu@2003");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = Credentials;
                    smtp.Port = 25;    //alternative port number is 8889
                    smtp.EnableSsl = false;
                    smtp.Send(mail);

                    return JsonConvert.SerializeObject(new { message = "Message sent"});
                }
                catch (Exception)
                {
                    return JsonConvert.SerializeObject(new { message = "Failed!" });
                }
            }
            else
            {
                return JsonConvert.SerializeObject(new { message = "Failed!" });
            }
        }

        public ActionResult _Layout(string cusId)
        {
            if(cusId == null)
            {
                return View();
            }
            else
            {
                List<Cart_Item> cart = vuvuDB.Cart_Item.Where(c => c.CUSTOMER_ID == Convert.ToInt32(cusId)).ToList();
                return View(cart);
            }
            
        }
    }
}