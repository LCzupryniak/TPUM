using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IInventoryManagement
    {
        void AdjustProductQuantity(int productId, int quantityChange);
        bool CheckProductAvailability(int productId, int requiredQuantity);
    }
}
