using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvuyelwafitness.Models
{
    public class Products_VM
    {
        public List<Product> Categories { get; set; }
        public List<Product> ProductByFilter { get; set; }
        public List<Cust_Wishlist> CustWishlist { get; set; }
    }
}