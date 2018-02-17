using System;

namespace SqlRepo
{
    public interface IStatementFactory
    {
        IDeleteStatement<TEntity> CreateDelete<TEntity>() where TEntity: class, new();
        IInsertStatement<TEntity> CreateInsert<TEntity>() where TEntity: class, new();
        ISelectStatement<TEntity> CreateSelect<TEntity>() where TEntity: class, new();
        IUpdateStatement<TEntity> CreateUpdate<TEntity>() where TEntity: class, new();
    }
}