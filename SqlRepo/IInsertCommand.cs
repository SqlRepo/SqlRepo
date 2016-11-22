using System;
using System.Linq.Expressions;

namespace SqlRepo
{
    public interface IInsertCommand<TEntity> : ISqlCommand<TEntity>
        where TEntity: class, new()
    {
        IInsertCommand<TEntity> For(TEntity entity);
        IInsertCommand<TEntity> FromScratch();
        IInsertCommand<TEntity> UsingTableName(string tableName);
        IInsertCommand<TEntity> UsingTableSchema(string tableSchema);
        IInsertCommand<TEntity> With<TMember>(Expression<Func<TEntity, TMember>> selector, TMember @value);
    }
}