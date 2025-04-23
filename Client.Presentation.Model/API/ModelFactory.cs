using Client.Logic.API;
using Client.ObjectModels.Logic.API;
using Client.Presentation.Model.Implementation;

namespace Client.Presentation.Model.API
{
    public abstract class ModelFactory
    {
        // Helper stays the same or can be inlined
        private static TLogic ResolveLogic<TLogic>(TLogic? injectedLogic, Func<TLogic> defaultStaticCreator) where TLogic : class
        {
            return injectedLogic ?? defaultStaticCreator();
        }

        // Static creation methods directly
        public static ICustomerModelService CreateCustomerModelService(ICustomerLogic? customerLogic = null, ICartLogic? cartLogic = null)
        {
            ICustomerLogic resolvedCustomerLogic = ResolveLogic(customerLogic, () => LogicFactory.CreateCustomerLogic());
            ICartLogic resolvedCartLogic = ResolveLogic(cartLogic, () => LogicFactory.CreateCartLogic());
            return new CustomerModelService(resolvedCustomerLogic, resolvedCartLogic);
        }

        public static ICartModelService CreateCartModelService(ICartLogic? cartLogic = null)
        {
            ICartLogic resolvedLogic = ResolveLogic(cartLogic, () => LogicFactory.CreateCartLogic());
            return new CartModelService(resolvedLogic);
        }

        public static IProductModelService CreateItemModelService(IProductLogic? itemLogic = null)
        {
            IProductLogic resolvedLogic = ResolveLogic(itemLogic, () => LogicFactory.CreateItemLogic());
            return new ProductModelService(resolvedLogic);
        }

        public static IOrderModelService CreateOrderModelService(IOrderLogic? orderLogic = null, ICustomerLogic? customerLogic = null, IProductLogic? itemLogic = null)
        {
            IOrderLogic resolvedOrderLogic = ResolveLogic(orderLogic, () => LogicFactory.CreateOrderLogic());
            ICustomerLogic resolvedCustomerLogic = ResolveLogic(customerLogic, () => LogicFactory.CreateCustomerLogic());
            IProductLogic resolvedItemLogic = ResolveLogic(itemLogic, () => LogicFactory.CreateItemLogic());
            return new OrderModelService(resolvedOrderLogic, resolvedCustomerLogic, resolvedItemLogic);
        }
    }
}