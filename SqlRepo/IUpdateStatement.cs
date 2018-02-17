using System;
using System.Linq.Expressions;

namespace SqlRepo
{
    public interface IUpdateStatement<TEntity> : ISqlStatement<int>
        where TEntity: class, new()
    {
        IUpdateStatement<TEntity> And(Expression<Func<TEntity, bool>> expression);
        IUpdateStatement<TEntity> For(TEntity entity);
        IUpdateStatement<TEntity> NestedAnd(Expression<Func<TEntity, bool>> expression);
        IUpdateStatement<TEntity> NestedOr(Expression<Func<TEntity, bool>> expression);
        IUpdateStatement<TEntity> Or(Expression<Func<TEntity, bool>> expression);
        IUpdateStatement<TEntity> Set<TMember>(Expression<Func<TEntity, TMember>> selector, TMember @value, string tableSchema = null, string tableName = null);
        IUpdateStatement<TEntity> Where(Expression<Func<TEntity, bool>> expression);
        IUpdateStatement<TEntity> WhereIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values);
    }
}