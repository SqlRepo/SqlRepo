using System;

namespace SqlRepo.Abstractions
{
    public interface IEntityActivatorFactory
    {
        EntityActivator<T> Create<T>() where T: class, new();
    }
}