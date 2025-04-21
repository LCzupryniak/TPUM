namespace Client.Presentation.Model.API
{
    public interface ICartModelService
    {
        public abstract IEnumerable<ICartModel> GetAllInventories();
        public abstract ICartModel? GetInventory(Guid id);

        public abstract void Add(Guid id, int capacity);
    }
}