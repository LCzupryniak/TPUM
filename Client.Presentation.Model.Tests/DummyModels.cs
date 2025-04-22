using Client.Presentation.Model.API;

namespace Client.Presentation.Model.Tests
{
    // Dummy Item Model
    internal class DummyItemModel : IProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public int MaintenanceCost { get; set; }
    }

    // Dummy Cart Model
    internal class DummyCartModel : ICartModel
    {
        public Guid Id { get; set; }
        public int Capacity { get; set; }
        public IEnumerable<IProductModel> Items { get; set; } = Enumerable.Empty<IProductModel>();
    }

    // Dummy Customer Model
    internal class DummyCustomerModel : ICustomerModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Money { get; set; }
        public ICartModel Cart { get; set; } = default!;
    }

    // Dummy Order Model
    internal class DummyOrderModel : IOrderModel
    {
        public Guid Id { get; set; }
        public ICustomerModel Buyer { get; set; } = default!;
        public IEnumerable<IProductModel> ItemsToBuy { get; set; } = Enumerable.Empty<IProductModel>();
    }
}