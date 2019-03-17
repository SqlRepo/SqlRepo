using System;

namespace SqlRepo.Abstractions
{
    public interface IEntityMapperDefinitionProvider
    {
        EntityMapperDefinition<T> Get<T>()
            where T: class, new();
    }
}