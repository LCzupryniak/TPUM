using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.API
{
    public interface IOrder
    {
        int Id { get; }
        ICustomer Customer { get; }
    }
}
