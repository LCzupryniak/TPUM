﻿using Client.Data.Implementation;
using Client.ObjectModels.Data.API;

namespace Client.Data.API
{
    public abstract class DataContextFactory : IDataContextFactory
    {
        public static IDataContext CreateDataContext(IDataContext? dataContext = default(IDataContext))
        {
            return dataContext ?? new DataContext();
        }
    }
}
