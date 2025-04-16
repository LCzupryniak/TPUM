using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.API;

namespace Data.Implementation
{
    public abstract class DataFactory
    {
        public static IDataRepository CreateRepository(IDataRepository? dataRepository = default(IDataRepository))
        {
            return dataRepository ?? new DataRepository();
        }
    }
}
