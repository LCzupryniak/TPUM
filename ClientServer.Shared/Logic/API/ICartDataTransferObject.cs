namespace ClientServer.Shared.Logic.API
{
    public interface ICartDataTransferObject
    {
        public abstract Guid Id { get; }
        public abstract int Capacity { get; }

        public abstract IEnumerable<IProductDataTransferObject> Items { get; }
    }
}
