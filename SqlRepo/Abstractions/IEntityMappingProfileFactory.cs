using System;

namespace SqlRepo.Abstractions
{
    public interface IEntityMappingProfileFactory
    {
        IEntityMappingProfile<T> Create<T>()
            where T: class, new();
    }
}