namespace Server.ObjectModels.Data.API
{
    public interface ICart : IIdentifiable
    {
        public abstract int Capacity { get; }

        public abstract List<IProduct> Items { get; }
    }
}
