using System;
using System.Collections.Generic;

namespace SqlRepo.Abstractions
{
    public interface IExecuteQuerySqlStatement<TEntity> : IExecuteSqlStatement<IEnumerable<TEntity>>
        where TEntity: class, new()
    {
        IExecuteQuerySqlStatement<TEntity> UsingMappingProfile(IEntityMappingProfile mappingProfile);
    }
}