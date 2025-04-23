using Server.Data.Implementation;
using Server.ObjectModels.Data.API;

namespace Server.Data.API
{
    public abstract class DataContextFactory : IDataContextFactory
    {
        public static IDataContext CreateDataContext(IDataContext? dataContext = default(IDataContext))
        {
            return dataContext ?? new DataContext();
        }
    }
}
