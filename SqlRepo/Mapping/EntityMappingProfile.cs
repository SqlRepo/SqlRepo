using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityMappingProfile<T> : IEntityMappingProfile<T>
        where T: class, new()
    {
        private readonly IDictionary<MemberInfo, IEntityMemberMapper> mappers;

        public EntityMappingProfile()
        {
            this.TargetType = typeof(T);
            this.mappers = new Dictionary<MemberInfo, IEntityMemberMapper>();
        }

        public Type TargetType { get; }

        public IEntityMappingProfile<T> ForMember<TMember>(Expression<Func<T, TMember>> selector,
            Action<IEntityMemberMapperBuilderConfig> config)
        {
            var memberInfo = selector.GetMemberExpression()
                                     .Member;

            var entityMemberMapperBuilder = new EntityMemberMapperBuilder(memberInfo);
            config(entityMemberMapperBuilder);

            this.mappers.Add(memberInfo, entityMemberMapperBuilder.Build());

            return this;
        }

        public IEntityMemberMapper GetMapper(MemberInfo memberInfo)
        {
            this.mappers.TryGetValue(memberInfo, out var mapper);
            return mapper;
        }

        public void Map(object entity, IDataRecord dataRecord) { }
    }
}