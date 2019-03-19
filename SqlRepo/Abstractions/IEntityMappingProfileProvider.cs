using System;
using System.Reflection;

namespace SqlRepo.Abstractions
{
    public interface IEntityMappingProfileProvider
    {
        void Add<T>(IEntityMappingProfile<T> profile)
            where T: class, new();

        void AddFromAssembly(Assembly assembly);

        IEntityMappingProfile<T> Get<T>()
            where T: class, new();

        IEntityMappingProfile Get(Type type);
    }
}