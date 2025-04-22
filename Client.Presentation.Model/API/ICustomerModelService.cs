namespace Client.Presentation.Model.API
{
    public interface ICustomerModelService
    {
        public abstract IEnumerable<ICustomerModel> GetAllCustomers();
        public abstract ICustomerModel? GetCustomer(Guid id);
        public abstract void AddCustomer(Guid id, string name, float money, Guid cartId);
        public abstract bool RemoveCustomer(Guid id);
        public abstract bool UpdateCustomer(Guid id, string name, float money, Guid cartId);
        public abstract void TriggerPeriodicItemMaintenanceDeduction();
    }
}