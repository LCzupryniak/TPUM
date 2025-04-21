namespace Client.Presentation.Model.API
{
    public interface ICustomerModel
    {
        public abstract Guid Id { get; }
        public abstract string Name { get; }
        public abstract float Money { get; }
        public abstract ICartModel Inventory { get; }
    }
}