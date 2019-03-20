using System;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace SqlRepo.Abstractions
{
    public interface IEntityMappingProfile
    {
        Type TargetType { get; }
        IEntityMemberMapper GetMapper(MemberInfo memberInfo);
        void Map(object entity, IDataRecord dataRecord);
    }

    public interface IEntityMappingProfile<T> : IEntityMappingProfile
        where T: class, new()
    {
        IEntityMappingProfile<T> ForMember<TMember>(Expression<Func<T, TMember>> selector,
            Action<IEntityMemberMapperBuilderConfig> builder);
    }
}