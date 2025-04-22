using ClientServer.Shared.Data.API;

namespace ClientServer.Shared.Logic.API
{
    public interface ILogicFactory
    {
        public abstract static ICustomerLogic CreateCustomerLogic(IDataRepository? dataRepository = default(IDataRepository));

        public abstract static ICartLogic CreateCartLogic(IDataRepository? dataRepository = default(IDataRepository));

        public abstract static IProductLogic CreateItemLogic(IDataRepository? dataRepository = default(IDataRepository));

        public abstract static IOrderLogic CreateOrderLogic(IDataRepository? dataRepository = default(IDataRepository));
    }
}
