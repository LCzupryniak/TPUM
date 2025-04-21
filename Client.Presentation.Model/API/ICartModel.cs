namespace Client.Presentation.Model.API
{
    public interface ICartModel
    {
        public abstract Guid Id { get; }
        public abstract int Capacity { get; }
        public abstract IEnumerable<IProductModel> Items { get; }
    }
}
