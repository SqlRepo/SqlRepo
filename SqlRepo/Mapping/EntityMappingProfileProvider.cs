using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityMappingProfileProvider : IEntityMappingProfileProvider
    {
        private readonly IDictionary<Type, IEntityMappingProfile> profiles =
            new Dictionary<Type, IEntityMappingProfile>();

        public void Add<T>(IEntityMappingProfile<T> profile)
            where T: class, new()
        {
            this.profiles[typeof(T)] = profile;
        }

        public void AddFromAssembly(Assembly assembly)
        {
            var entityMappingProfiles = assembly.GetTypes()
                                   .Where(t => !t.IsAbstract && !t.IsInterface &&  t.GetInterfaces().Contains(typeof(IEntityMappingProfile)))
                                   .Select(t => Activator.CreateInstance(t) as IEntityMappingProfile)
                                   .ToList();

            foreach(var entityMappingProfile in entityMappingProfiles)
            {
                this.profiles.Add(entityMappingProfile.TargetType, entityMappingProfile);
            }
        }

        public IEntityMappingProfile Get<T>()
            where T: class, new()
        {
            return this.Get(typeof(T));
        }

        public IEntityMappingProfile Get(Type type)
        {
            if(this.profiles.TryGetValue(type, out var profile))
            {
                return profile;
            }

            var defaultProfile = new DefaultEntityMappingProfile(type);
            this.profiles.Add(type, defaultProfile);
            return defaultProfile;
        }
    }
}