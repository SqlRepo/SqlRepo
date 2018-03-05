using System;
using System.Collections.Generic;

namespace SqlRepo.Abstractions
{
    public interface IRepository<TEntity>
        where TEntity: class, new()
    {
        IDeleteStatement<TEntity> Delete();
        int Delete(TEntity entity);
        IExecuteStatement<int> ExecuteNonQueryProcedure();
        IExecuteSqlStatement<int> ExecuteNonQuerySql();
        IExecuteStatement<TEntity> ExecuteProcedure();
        IExecuteSqlStatement<TEntity> ExecuteSql();
        IInsertStatement<TEntity> Insert();
        TEntity Insert(TEntity entity);
        ISelectStatement<TEntity> Query();
        IEnumerable<TEntity> ResultsFrom(ISelectStatement<TEntity> query);
        IUpdateStatement<TEntity> Update();
        int Update(TEntity entity);
        IRepository<TEntity> UseConnectionProvider(IConnectionProvider connectionProvider);
    }
}