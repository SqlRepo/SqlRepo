using System;

namespace SqlRepo
{
    public interface ICommandFactory
    {
        IDeleteCommand<TEntity> CreateDelete<TEntity>() where TEntity: class, new();
        IInsertCommand<TEntity> CreateInsert<TEntity>() where TEntity: class, new();
        ISelectCommand<TEntity> CreateSelect<TEntity>() where TEntity: class, new();
        IUpdateCommand<TEntity> CreateUpdate<TEntity>() where TEntity: class, new();
    }
}