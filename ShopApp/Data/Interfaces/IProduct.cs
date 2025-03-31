using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IProduct
    {
        int Id { get; }
        string Name { get; }
        string Description { get; }
        decimal Price { get; }
        int StockQuantity { get; }
        ProductCategory Category { get; }
    }
}
