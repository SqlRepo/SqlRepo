using System;
using System.Linq.Expressions;

namespace SqlRepo.Abstractions
{
    public interface IDeleteStatement<TEntity> : ISqlStatement<int>
        where TEntity: class, new()
    {
        IDeleteStatement<TEntity> And(Expression<Func<TEntity, bool>> expression);
        IDeleteStatement<TEntity> For(TEntity entity);
        IDeleteStatement<TEntity> NestedAnd(Expression<Func<TEntity, bool>> expression);
        IDeleteStatement<TEntity> NestedOr(Expression<Func<TEntity, bool>> expression);
        IDeleteStatement<TEntity> Or(Expression<Func<TEntity, bool>> expression);
        IDeleteStatement<TEntity> UsingTableName(string tableName);
        IDeleteStatement<TEntity> UsingTableSchema(string tableSchema);
        IDeleteStatement<TEntity> Where(Expression<Func<TEntity, bool>> expression);
        IDeleteStatement<TEntity> WhereIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values);
    }
}