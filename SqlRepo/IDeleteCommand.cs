using System;
using System.Linq.Expressions;

namespace SqlRepo
{
    public interface IDeleteCommand<TEntity> : ISqlCommand<int>
        where TEntity : class, new()
    {
        IDeleteCommand<TEntity> And(Expression<Func<TEntity, bool>> expression);
        IDeleteCommand<TEntity> For(TEntity entity);
        IDeleteCommand<TEntity> NestedAnd(Expression<Func<TEntity, bool>> expression);
        IDeleteCommand<TEntity> NestedOr(Expression<Func<TEntity, bool>> expression);
        IDeleteCommand<TEntity> Or(Expression<Func<TEntity, bool>> expression);
        IDeleteCommand<TEntity> Where(Expression<Func<TEntity, bool>> expression);
        IDeleteCommand<TEntity> UsingTableName(string tableName);
        IDeleteCommand<TEntity> UsingTableSchema(string tableSchema);

        IDeleteCommand<TEntity> WhereIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values);
    }
}