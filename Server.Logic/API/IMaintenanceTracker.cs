﻿using Server.ObjectModels.Logic.API;

namespace Server.Logic.API
{
    public interface IMaintenanceTracker : IObservable<ICustomerDataTransferObject>
    {
        public void Track(ICustomerDataTransferObject device);
    }
}
