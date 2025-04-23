using Server.ObjectModels.Logic.API;

namespace Server.Logic.API
{
    public interface IMaintenanceReporter : IObserver<ICustomerDataTransferObject>
    {
        void Subscribe(IObservable<ICustomerDataTransferObject> provider, Action onComplete, Action<Exception> onError, Action<ICustomerDataTransferObject> onNext);
        void Unsubscribe();
    }
}