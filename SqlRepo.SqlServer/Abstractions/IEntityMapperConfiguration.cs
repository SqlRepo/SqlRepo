using System;
using SqlRepo.Model;

namespace SqlRepo.SqlServer.Abstractions
{
    public interface IEntityMapperConfiguration
    {
        bool CanHandle<TEntity>() where TEntity: IEntity<int>;
    }
}