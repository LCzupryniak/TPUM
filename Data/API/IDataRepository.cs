using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.API
{
    public interface IDataRepository
    {
        List<IProduct> GetAllProducts();
        List<ICustomer> GetAllCustomers();
        List<IOrder> GetAllOrders();
    }
}
