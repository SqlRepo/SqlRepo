using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace SqlRepo.Abstractions
{
    public interface IEntityMappingProfile
    {
        Type TargetType { get; }
    }

    public interface IEntityMappingProfile<T>: IEntityMappingProfile
        where T: class, new()
    {
        IEntityMappingProfile<T> ForMember<TMember>(Expression<Func<T, TMember>> selector,
            Action<IEntityMemberMapperBuilderConfig<T, TMember>> builder);

        IEntityMemberMapper<T> GetMapper(MemberInfo memberInfo);
    }
}