using System;

namespace SqlRepo
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> Create<TEntity>() where TEntity: class, new();
    }
}