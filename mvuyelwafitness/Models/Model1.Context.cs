﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class db_a98f3b_mvuyelwafitnessdbEntities : DbContext
    {
        public db_a98f3b_mvuyelwafitnessdbEntities()
            : base("name=db_a98f3b_mvuyelwafitnessdbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Order_Item> Order_Item { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Administrator> Administrators { get; set; }
        public virtual DbSet<Cart_Item> Cart_Item { get; set; }
        public virtual DbSet<ContactForm> ContactForms { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Cust_Wishlist> Cust_Wishlist { get; set; }
        public virtual DbSet<Payment_Statuses> Payment_Statuses { get; set; }
        public virtual DbSet<Delivery_Status> Delivery_Status { get; set; }
    }
}
