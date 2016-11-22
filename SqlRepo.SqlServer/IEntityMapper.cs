using System;
using System.Collections.Generic;
using System.Data;

namespace SqlRepo.SqlServer
{
    public interface IEntityMapper
    {
        IEnumerable<TEntity> Map<TEntity>(IDataReader reader) where TEntity: class, new();
    }
}