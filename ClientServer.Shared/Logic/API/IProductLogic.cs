namespace ClientServer.Shared.Logic.API
{
    public interface IProductLogic
    {
        public abstract IEnumerable<IProductDataTransferObject> GetAll();

        public abstract IProductDataTransferObject? Get(Guid id);

        public abstract void Add(IProductDataTransferObject item);

        public abstract bool RemoveById(Guid id);

        public abstract bool Remove(IProductDataTransferObject item);

        public abstract bool Update(Guid id, IProductDataTransferObject item);
    }
}
