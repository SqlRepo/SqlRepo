using System;

namespace SqlRepo.Abstractions
{
    public interface IStatementFactory
    {
        IDeleteStatement<TEntity> CreateDelete<TEntity>()
            where TEntity: class, new();

        IExecuteStatement<TEntity> CreateExecute<TEntity>()
            where TEntity: class, new();

        IExecuteStatement<int> CreateExecuteNonQuery();

        IExecuteSqlStatement<TEntity> CreateExecuteSql<TEntity>()
            where TEntity: class, new();

        IExecuteSqlStatement<int> CreateExecuteSqlNonQuery();

        IInsertStatement<TEntity> CreateInsert<TEntity>()
            where TEntity: class, new();

        ISelectStatement<TEntity> CreateSelect<TEntity>()
            where TEntity: class, new();

        IUpdateStatement<TEntity> CreateUpdate<TEntity>()
            where TEntity: class, new();

        IStatementFactory UseConnectionProvider(IConnectionProvider connectionProvider);
    }
}