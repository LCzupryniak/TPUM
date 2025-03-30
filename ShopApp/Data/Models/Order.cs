using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public Customer CustomerOwner { get; set; }
        public List<Product> OrderedProducts { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus CurrentStatus { get; set; }

        public Order()
        {
            OrderedProducts = new List<Product>();
            OrderDate = DateTime.Now;
            CurrentStatus = OrderStatus.Created;
        }
    }
    public enum OrderStatus
    {
        Created,
        Processing,
        Completed,
        Cancelled
    }
}
