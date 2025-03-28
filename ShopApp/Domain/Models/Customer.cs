using System;
using System.Collections.Generic;

namespace ShopApp.Domain.Models
{

    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactInfo { get; set; }
    }
}