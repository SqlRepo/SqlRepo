using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace SqlRepo.Abstractions
{
    public interface IEntityMappingProfile
    {
        Type TargetType { get; }
        void Map(object entity, IDataRecord dataRecord);
    }

    public interface IEntityMappingProfile<T> : IEntityMappingProfile
        where T: class, new()
    {
        IEntityMappingProfile<T> ForEnumerableMember<TEnumerable, TItem>(
            Expression<Func<T, IEnumerable<TItem>>> selector,
            IEntityMappingProfile<TItem> mappingProfile)
            where TEnumerable: class, IEnumerable<TItem>, new() where TItem: class, new();

        IEntityMappingProfile<T> ForEnumerableMember<TEnumerable, TItem>(
            Expression<Func<T, IEnumerable<TItem>>> selector,
            Action<IEntityMappingProfile<TItem>> config)
            where TEnumerable: class, IEnumerable<TItem>, new() where TItem: class, new();

        IEntityMappingProfile<T> ForMember<TMember>(Expression<Func<T, TMember>> selector,
            Action<IEntityValueMemberMapperBuilderConfig> config);

        IEntityMappingProfile<T> ForMember<TMember>(Expression<Func<T, TMember>> selector,
            IEntityMappingProfile mappingProfile)
            where TMember: class, new();

        IEntityMappingProfile<T> ForMember<TMember>(Expression<Func<T, TMember>> selector,
            Action<IEntityMappingProfile<TMember>> config)
            where TMember: class, new();
    }
}