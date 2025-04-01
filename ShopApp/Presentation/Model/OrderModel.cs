using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public List<OrderItemModel> Items { get; set; }

        public OrderModel()
        {
            Items = new List<OrderItemModel>();
        }
    }
}
