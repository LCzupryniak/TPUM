namespace Server.ObjectModels.Logic.API
{
    public interface ICustomerLogic
    {
        public abstract IEnumerable<ICustomerDataTransferObject> GetAll();

        public abstract ICustomerDataTransferObject? Get(Guid id);

        public abstract void Add(ICustomerDataTransferObject item);

        public abstract bool RemoveById(Guid id);

        public abstract bool Remove(ICustomerDataTransferObject item);

        public abstract bool Update(Guid id, ICustomerDataTransferObject item);

        public abstract void PeriodicItemMaintenanceDeduction();

        public abstract void DeduceMaintenanceCost(ICustomerDataTransferObject customer);
    }
}
