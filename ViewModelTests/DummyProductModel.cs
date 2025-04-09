using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace ViewModelTests
{
    internal class DummyProductModel : IProductModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
