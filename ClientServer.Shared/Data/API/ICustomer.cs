namespace ClientServer.Shared.Data.API
{
    public interface ICustomer : IIdentifiable
    {
        public abstract string Name { get; }

        public abstract float Money { get; }

        public abstract ICart Cart { get; }
    }
}
