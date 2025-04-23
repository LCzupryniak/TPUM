using Server.ObjectModels.Logic.API;
using Server.Logic.API;

namespace Server.Logic.Implementation
{
    internal class MaintenanceTracker : IMaintenanceTracker
    {
        private readonly List<IObserver<ICustomerDataTransferObject>> _observers;

        public MaintenanceTracker()
        {
            _observers = new List<IObserver<ICustomerDataTransferObject>>();
        }

        public IDisposable Subscribe(IObserver<ICustomerDataTransferObject> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
            return new Unsubscriber(_observers, observer);
        }

        public void Track(ICustomerDataTransferObject customer)
        {
            foreach (IObserver<ICustomerDataTransferObject> o in _observers)
            {
                if (customer == null)
                {
                    o.OnError(new Exception("TrackCustomer is null"));
                }
                else
                {
                    o.OnNext(customer);
                }
            }
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<ICustomerDataTransferObject>> _observers;
            private readonly IObserver<ICustomerDataTransferObject> _observer;

            public Unsubscriber(List<IObserver<ICustomerDataTransferObject>> observers, IObserver<ICustomerDataTransferObject> observer)
            {
                _observer = observer;
                _observers = observers;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
