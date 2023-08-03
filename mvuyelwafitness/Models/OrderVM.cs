using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvuyelwafitness.Models
{
    public class OrderVM
    {
        public List<Customer> myCustomer { get; set; }
        public List<Order> myOrders { get; set; }
        public List<Order_Item> myOrdItem { get; set; }
        public List<Address> MyAddresses { get; set; }
    }
}