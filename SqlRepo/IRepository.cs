using System;
using System.Collections.Generic;

namespace SqlRepo
{
    public interface IRepository<TEntity>
        where TEntity: class, new()
    {
        IDeleteStatement<TEntity> Delete();
        int Delete(TEntity entity);
        IInsertStatement<TEntity> Insert();
        TEntity Insert(TEntity entity);
        ISelectStatement<TEntity> Query();
        IEnumerable<TEntity> ResultsFrom(ISelectStatement<TEntity> query);
        IUpdateStatement<TEntity> Update();
        int Update(TEntity entity);
    }
}