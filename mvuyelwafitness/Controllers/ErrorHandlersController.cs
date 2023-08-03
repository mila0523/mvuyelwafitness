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
    public class ErrorHandlersController : Controller
    {
        private db_a98f3b_mvuyelwafitnessdbEntities vuvuDB = new db_a98f3b_mvuyelwafitnessdbEntities();

        // GET: ErrorHandlers
        public ActionResult Canceled()
        {
            return View();
        }

        public ActionResult SuccessfullOrder()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }
    }
}