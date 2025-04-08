using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API;

namespace Data.Implementation
{
    public static class DataFactory
    {
        public static IDataRepository CreateRepository()
        {
            return new DataRepository();
        }
    }
}
