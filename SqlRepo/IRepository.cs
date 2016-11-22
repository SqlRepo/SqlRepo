using System;
using System.Collections.Generic;

namespace SqlRepo
{
    public interface IRepository<TEntity>
        where TEntity: class, new()
    {
        IDeleteCommand<TEntity> Delete();
        int Delete(TEntity entity);
        IInsertCommand<TEntity> Insert();
        TEntity Insert(TEntity entity);
        ISelectCommand<TEntity> Query();
        IEnumerable<TEntity> ResultsFrom(ISelectCommand<TEntity> query);
        IUpdateCommand<TEntity> Update();
        int Update(TEntity entity);
    }
}