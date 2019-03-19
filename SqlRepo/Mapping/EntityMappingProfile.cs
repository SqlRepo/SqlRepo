using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityMappingProfile<T> : IEntityMappingProfile<T>
        where T: class, new()
    {
        private readonly IDictionary<MemberInfo, IEntityMemberMapper<T>> mappers;

        public EntityMappingProfile()
        {
            this.TargetType = typeof(T);
            this.mappers = new Dictionary<MemberInfo, IEntityMemberMapper<T>>();
        }

        public Type TargetType { get; }

        public IEntityMappingProfile<T> ForMember<TMember>(Expression<Func<T, TMember>> selector,
            Action<IEntityMemberMapperBuilderConfig<T, TMember>> config)
        {
            var memberInfo = selector.GetMemberExpression()
                                     .Member;

            var entityMemberMapperBuilder = new EntityMemberMapperBuilder<T, TMember>(memberInfo);
            config(entityMemberMapperBuilder);

            this.mappers.Add(memberInfo, entityMemberMapperBuilder.Build());

            return this;
        }

        public IEntityMemberMapper<T> GetMapper(MemberInfo memberInfo)
        {
            this.mappers.TryGetValue(memberInfo, out var mapper);
            return mapper;
        }
    }
}