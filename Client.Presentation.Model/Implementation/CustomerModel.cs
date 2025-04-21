using Client.Presentation.Model.API;
using ClientServer.Shared.Logic.API;

namespace Client.Presentation.Model.Implementation
{
    internal class CustomerModel : ICustomerModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public float Money { get; }
        public ICartModel Inventory { get; }

        // DTO
        public CustomerModel(ICustomerDataTransferObject dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Money = dto.Money;
            Inventory = new CartModel(dto.Inventory);
        }

        // direct creation
        public CustomerModel(Guid id, string name, float money, ICartModel inventory)
        {
            Id = id;
            Name = name;
            Money = money;
            Inventory = inventory;
        }
    }
}