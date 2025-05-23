﻿namespace Server.ObjectModels.Logic.API
{
    public interface ICustomerDataTransferObject
    {
        public abstract Guid Id { get; }

        public abstract string Name { get; set; }

        public abstract float Money { get; set; }

        public abstract ICartDataTransferObject Cart { get; set; }
    }
}
