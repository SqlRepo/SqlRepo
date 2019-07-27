using System;
using System.Collections.Generic;

namespace SqlRepo.Abstractions
{
    public interface
        IExecuteQueryProcedureStatement<TEntity> : IExecuteProcedureStatement<IEnumerable<TEntity>>
        where TEntity: class, new()
    {
        IExecuteQueryProcedureStatement<TEntity> UsingMappingProfile(IEntityMappingProfile mappingProfile);
    }
}