using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Implementation;

namespace Data.API
{
    public interface IDataFactory
    {
        IDataRepository CreateRepository();
    }

    public static class DataFactory
    {
        public static IDataRepository CreateRepository()
        {
            return new DataRepository(); // dostęp do klasy internal
        }
    }
}
