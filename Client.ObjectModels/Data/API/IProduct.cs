﻿namespace Client.ObjectModels.Data.API
{
    public interface IProduct : IIdentifiable
    {
        public abstract string Name { get; }

        public abstract int Price { get; }

        public abstract int MaintenanceCost { get; }
    }
}
