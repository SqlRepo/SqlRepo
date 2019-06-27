using System;
using System.Linq.Expressions;

namespace SqlRepo.Abstractions
{
    public interface IInsertStatement<TEntity> : ISqlStatement<TEntity>
        where TEntity : class, new()
    {
        IInsertStatement<TEntity> For(TEntity entity);
        IInsertStatement<TEntity> FromScratch();
        IInsertStatement<TEntity> UsingMappingProfile(IEntityMappingProfile mappingProfile);
        IInsertStatement<TEntity> UsingTableName(string tableName);
        IInsertStatement<TEntity> UsingTableSchema(string tableSchema);
        IInsertStatement<TEntity> With<TMember>(Expression<Func<TEntity, TMember>> selector, TMember @value);
    }
}