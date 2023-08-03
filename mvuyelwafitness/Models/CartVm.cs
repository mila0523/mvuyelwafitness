using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvuyelwafitness.Models
{
    public class CartVm
    {
        public List<Customer> myCustomer { get; set; }
        public List<Address> Deliveryaddress { get; set; }
        public List<Cart_Item> Cart { get; set; }
        public List<Product> Prod { get; set; }
    }
}