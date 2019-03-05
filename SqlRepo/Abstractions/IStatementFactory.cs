using System;

namespace SqlRepo.Abstractions
{
    public interface IStatementFactory
    {
        IDeleteStatement<TEntity> CreateDelete<TEntity>()
            where TEntity: class, new();

        IExecuteQueryProcedureStatement<TEntity> CreateExecuteQueryProcedure<TEntity>()
            where TEntity: class, new();

        IExecuteNonQueryProcedureStatement CreateExecuteNonQueryProcedure();

        IExecuteQuerySqlStatement<TEntity> CreateExecuteQuerySql<TEntity>()
            where TEntity: class, new();

        IExecuteNonQuerySqlStatement CreateExecuteNonQuerySql();

        IInsertStatement<TEntity> CreateInsert<TEntity>()
            where TEntity: class, new();

        ISelectStatement<TEntity> CreateSelect<TEntity>()
            where TEntity: class, new();

        IUpdateStatement<TEntity> CreateUpdate<TEntity>()
            where TEntity: class, new();

        IStatementFactory UseConnectionProvider(IConnectionProvider connectionProvider);
    }
}