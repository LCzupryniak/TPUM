using Client.ObjectModels.Data.API;

namespace Client.ObjectModels.Logic.API
{
    public interface ILogicFactory
    {
        public abstract static ICustomerLogic CreateCustomerLogic(IDataRepository? dataRepository = default);

        public abstract static ICartLogic CreateCartLogic(IDataRepository? dataRepository = default);

        public abstract static IProductLogic CreateItemLogic(IDataRepository? dataRepository = default);

        public abstract static IOrderLogic CreateOrderLogic(IDataRepository? dataRepository = default);
    }
}
