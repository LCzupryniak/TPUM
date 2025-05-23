﻿namespace Client.Presentation.Model.API
{
    public interface IProductModel
    {
        public abstract Guid Id { get; }
        public abstract string Name { get; }
        public abstract int Price { get; }
        public abstract int MaintenanceCost { get; }
    }
}