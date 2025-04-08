using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API;

namespace DataTests
{
    internal class DummyProduct : IProduct
    {
        public string Name { get; private set; }
        public double Price { get; private set; }

        public DummyProduct(string name, double price)
        {
            Name = name;
            Price = price;
        }
    }
}
