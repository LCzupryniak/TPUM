using Client.Data.API;
using Client.Logic.Implementation;
using Client.ObjectModels.Data.API;
using Client.ObjectModels.Logic.API;

namespace Client.Logic.API
{
    public abstract class LogicFactory : ILogicFactory
    {
        private static IDataRepository _repository = DataRepositoryFactory.CreateDataRepository();

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

        public static IConnectionService CreateConnectionService(IConnectionService? service = default(IConnectionService), Action? onDataChanged = null)
        {
            IConnectionService connectionService = service ?? new ClientConnectionService();

            if (onDataChanged != null)
                _repository.OnDataChanged += onDataChanged;

            return connectionService;
        }
    }
}
