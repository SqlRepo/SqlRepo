using System;

namespace SqlRepo.Abstractions
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> Create<TEntity>() where TEntity: class, new();
    }
}