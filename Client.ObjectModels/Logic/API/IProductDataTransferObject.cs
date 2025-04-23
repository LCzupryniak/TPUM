namespace Client.ObjectModels.Logic.API
{
    public interface IProductDataTransferObject
    {
        public abstract Guid Id { get; }

        public abstract string Name { get; }

        public abstract int Price { get; }

        public abstract int MaintenanceCost { get; }
    }
}
