//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mvuyelwafitness.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Payment
    {
        public int PAYMENT_ID { get; set; }
        public int CUSTOMER_ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Amount { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
