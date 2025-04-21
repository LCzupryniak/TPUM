namespace Client.Presentation.Model.API
{
    public interface IProductModelService
    {
        public abstract IEnumerable<IProductModel> GetAllItems();
        public abstract IProductModel? GetItem(Guid id);
        public abstract void AddItem(Guid id, string name, int price, int maintenanceCost);
        public abstract bool RemoveItem(Guid id);
        public abstract bool UpdateItem(Guid id, string name, int price, int maintenanceCost);
    }
}