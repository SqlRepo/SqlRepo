using System;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityMappingProfileFactory : IEntityMappingProfileFactory
    {
        public IEntityMappingProfile<TEntity> Create<TEntity>()
            where TEntity: class, new()
        {
            return new EntityMappingProfile<TEntity>();
        }
    }
}