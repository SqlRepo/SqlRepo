using System;
using System.Linq.Expressions;

namespace SqlRepo
{
    public interface IUpdateCommand<TEntity> : ISqlCommand<int>
        where TEntity: class, new()
    {
        IUpdateCommand<TEntity> And(Expression<Func<TEntity, bool>> expression);
        IUpdateCommand<TEntity> For(TEntity entity);
        IUpdateCommand<TEntity> FromScratch();
        IUpdateCommand<TEntity> NestedAnd(Expression<Func<TEntity, bool>> expression);
        IUpdateCommand<TEntity> NestedOr(Expression<Func<TEntity, bool>> expression);
        IUpdateCommand<TEntity> Or(Expression<Func<TEntity, bool>> expression);
        IUpdateCommand<TEntity> Set<TMember>(Expression<Func<TEntity, TMember>> selector, TMember @value);
        IUpdateCommand<TEntity> UsingTableName(string tableName);
        IUpdateCommand<TEntity> UsingTableSchema(string tableSchema);
        IUpdateCommand<TEntity> Where(Expression<Func<TEntity, bool>> expression);
    }
}