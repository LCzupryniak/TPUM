namespace Client.Presentation.Model.API
{
    public interface ICartModelService
    {
        public abstract IEnumerable<ICartModel> GetAllCarts();
        public abstract ICartModel? GetCart(Guid id);

        public abstract void Add(Guid id, int capacity);
    }
}