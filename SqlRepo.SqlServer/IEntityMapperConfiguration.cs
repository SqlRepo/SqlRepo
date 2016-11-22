using System;
using SqlRepo.Model;

namespace SqlRepo.SqlServer
{
    public interface IEntityMapperConfiguration
    {
        bool CanHandle<TEntity>() where TEntity: IEntity<int>;
    }
}