using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationDate { get; set; }

        public Product()
        {
            CreationDate = DateTime.Now;
        }

        public bool IsAvailable()
        {
            return Quantity > 0;
        }
    }
}
