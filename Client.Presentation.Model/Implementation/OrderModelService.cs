using Client.Presentation.Model.API;
using ClientServer.Shared.Logic.API;

namespace Client.Presentation.Model.Implementation
{
    internal class OrderModelService : IOrderModelService
    {
        private readonly IOrderLogic _orderLogic;
        private readonly ICustomerLogic _customerLogic;
        private readonly IProductLogic _itemLogic;

        public OrderModelService(IOrderLogic orderLogic, ICustomerLogic customerLogic, IProductLogic itemLogic)
        {
            _orderLogic = orderLogic ?? throw new ArgumentNullException(nameof(orderLogic));
            _customerLogic = customerLogic ?? throw new ArgumentNullException(nameof(customerLogic));
            _itemLogic = itemLogic ?? throw new ArgumentNullException(nameof(itemLogic));
        }

        public IEnumerable<IOrderModel> GetAllOrders()
        {
            return _orderLogic.GetAll()
                             .Select(dto => new OrderModel(dto)); // Map DTO to Model
        }

        public IOrderModel? GetOrder(Guid id)
        {
            IOrderDataTransferObject? dto = _orderLogic.Get(id);
            return dto == null ? null : new OrderModel(dto); // Map DTO to Model
        }

        public void AddOrder(Guid id, Guid buyerId, IEnumerable<Guid> itemIds)
        {
            ICustomerDataTransferObject buyerDto = _customerLogic.Get(buyerId)!;
            List<IProductDataTransferObject> itemDtos = new List<IProductDataTransferObject>();
            foreach (Guid itemId in itemIds)
            {
                IProductDataTransferObject? itemDto = _itemLogic.Get(itemId);
                if (itemDto == null)
                {
                    throw new InvalidOperationException($"Item with ID {itemId} not found.");
                }
                itemDtos.Add(itemDto);
            }

            TransientOrderDTO transientDto = new TransientOrderDTO(id, buyerDto, itemDtos);
            _orderLogic.Add(transientDto);
        }

        public bool RemoveOrder(Guid id)
        {
            return _orderLogic.RemoveById(id);
        }

        public void TriggerPeriodicOrderProcessing()
        {
            _orderLogic.PeriodicOrderProcessing();
        }
    }
}