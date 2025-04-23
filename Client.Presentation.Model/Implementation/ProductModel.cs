using Client.ObjectModels.Logic.API;
using Client.Presentation.Model.API;

namespace Client.Presentation.Model.Implementation
{
    internal class ProductModel : IProductModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public int Price { get; }
        public int MaintenanceCost { get; }

        // DTO
        public ProductModel(IProductDataTransferObject dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Price = dto.Price;
            MaintenanceCost = dto.MaintenanceCost;
        }

        // direct creation
        public ProductModel(Guid id, string name, int price, int maintenanceCost)
        {
            Id = id;
            Name = name;
            Price = price;
            MaintenanceCost = maintenanceCost;
        }
    }
}
