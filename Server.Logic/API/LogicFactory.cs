using Server.Data.API;
using Server.Logic.Implementation;
using Server.ObjectModels.Logic.API;
using Server.ObjectModels.Data.API;


namespace Server.Logic.API
{
    public abstract class LogicFactory : ILogicFactory
    {
        public static ICustomerLogic CreateCustomerLogic(IDataRepository? dataRepository = default(IDataRepository))
        {
            return new CustomerLogic(dataRepository ?? _repository);
        }

        public static ICartLogic CreateCartLogic(IDataRepository? dataRepository = default(IDataRepository))
        {
            return new CartLogic(dataRepository ?? _repository);
        }

        public static IProductLogic CreateItemLogic(IDataRepository? dataRepository = default(IDataRepository))
        {
            return new ProductLogic(dataRepository ?? _repository);
        }

        public static IOrderLogic CreateOrderLogic(IDataRepository? dataRepository = default(IDataRepository))
        {
            return new OrderLogic(dataRepository ?? _repository);
        }

        private static IDataRepository _repository = DataRepositoryFactory.CreateDataRepository();
    }
}
