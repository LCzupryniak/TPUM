using Client.Presentation.Model.API;
using ClientServer.Shared.Logic.API;

namespace Client.Presentation.Model.Implementation
{
    internal class CustomerModel : ICustomerModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public float Money { get; }
        public ICartModel Cart { get; }

        // DTO
        public CustomerModel(ICustomerDataTransferObject dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Money = dto.Money;
            Cart = new CartModel(dto.Cart);
        }

        // direct creation
        public CustomerModel(Guid id, string name, float money, ICartModel cart)
        {
            Id = id;
            Name = name;
            Money = money;
            Cart = cart;
        }
    }
}