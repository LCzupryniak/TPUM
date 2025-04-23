namespace Client.ObjectModels.Logic.API
{
    public interface ICartLogic
    {
        public abstract IEnumerable<ICartDataTransferObject> GetAll();

        public abstract ICartDataTransferObject? Get(Guid id);

        public abstract void Add(ICartDataTransferObject item);

        public abstract bool RemoveById(Guid id);

        public abstract bool Remove(ICartDataTransferObject item);

        public abstract bool Update(Guid id, ICartDataTransferObject item);
    }
}
