﻿namespace Server.ObjectModels.Data.API
{
    public interface IDataContextFactory
    {
        public abstract static IDataContext CreateDataContext(IDataContext? dataContext = default);
    }
}
