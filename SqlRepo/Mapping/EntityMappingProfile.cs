using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityMappingProfile<T> : IEntityMappingProfile<T>
        where T: class, new()
    {
        private readonly IList<IEntityValueMemberMapper> keyMemberMappers;
        private readonly IDictionary<MemberInfo, IEntityMemberMapper> memberMappers;
        private readonly IDictionary<MemberInfo, IEntityMappingProfile> memberMappingProfiles;

        public EntityMappingProfile()
        {
            this.TargetType = typeof(T);
            this.keyMemberMappers = new List<IEntityValueMemberMapper>();
            this.memberMappers = new Dictionary<MemberInfo, IEntityMemberMapper>();
            this.memberMappingProfiles = new Dictionary<MemberInfo, IEntityMappingProfile>();
        }

        public Type TargetType { get; }

        public bool DataRecordMatchesEntity(object entity, IDataRecord dataRecord)
        {
            return this.keyMemberMappers.All(m => m.ValueMatches(entity, dataRecord));
        }

        public bool EntitiesMatch(object entity1, object entity2)
        {
            return this.keyMemberMappers.All(m => m.ValuesMatch(entity1, entity2));
        }

        public IEntityMappingProfile<T> ForArrayMember<TItem>(
            Expression<Func<T, TItem[]>> selector,
            IEntityMappingProfile<TItem> mappingProfile)
            where TItem: class, new()
        {
            var memberInfo = selector.GetMemberExpression()
                                     .Member;
            var mapper =
                new EntityArrayMemberMapper<TItem>(memberInfo, mappingProfile);
            this.memberMappers.Add(memberInfo, mapper);

            return this;
        }

        public IEntityMappingProfile<T> ForArrayMember<TItem>(Expression<Func<T, TItem[]>> selector,
            Action<IEntityMappingProfile<TItem>> config)
            where TItem: class, new()
        {

            var entityMappingProfile = new EntityMappingProfile<TItem>();
            config(entityMappingProfile);
            
            return this.ForArrayMember(selector, entityMappingProfile);
        }

        public IEntityMappingProfile<T> ForGenericCollectionMember<TEnumerable, TItem>(
            Expression<Func<T, IEnumerable<TItem>>> selector,
            IEntityMappingProfile<TItem> mappingProfile)
            where TEnumerable: class, IEnumerable<TItem>, new() where TItem: class, new()
        {
            var memberInfo = selector.GetMemberExpression()
                                     .Member;
            var mapper =
                new EntityGenericCollectionMemberMapper<TEnumerable, TItem>(memberInfo, mappingProfile);
            this.memberMappers.Add(memberInfo, mapper);

            return this;
        }

        public IEntityMappingProfile<T> ForGenericCollectionMember<TEnumerable, TItem>(
            Expression<Func<T, IEnumerable<TItem>>> selector,
            Action<IEntityMappingProfile<TItem>> config)
            where TEnumerable: class, IEnumerable<TItem>, new() where TItem: class, new()
        {
            var entityMappingProfile = new EntityMappingProfile<TItem>();
            config(entityMappingProfile);

            return this.ForGenericCollectionMember<TEnumerable, TItem>(selector, entityMappingProfile);
        }

        public IEntityMappingProfile<T> ForMember<TMember>(Expression<Func<T, TMember>> selector,
            Action<IEntityValueMemberMapperBuilderConfig> config)
        {
            var memberInfo = selector.GetMemberExpression()
                                     .Member;

            var entityMemberMapperBuilder = new EntityValueMemberMapperBuilder(memberInfo);
            config(entityMemberMapperBuilder);

            var entityValueMemberMapper = entityMemberMapperBuilder.Build();
            if(entityValueMemberMapper.IsKey)
            {
                this.keyMemberMappers.Add(entityValueMemberMapper);
            }
            this.memberMappers.Add(memberInfo, entityValueMemberMapper);

            return this;
        }

        public IEntityMappingProfile<T> ForMember<TMember>(Expression<Func<T, TMember>> selector,
            IEntityMappingProfile mappingProfile)
            where TMember: class, new()
        {
            var memberInfo = selector.GetMemberExpression()
                                     .Member;

            this.memberMappingProfiles.Add(memberInfo, mappingProfile);

            return this;
        }

        public IEntityMappingProfile<T> ForMember<TMember>(Expression<Func<T, TMember>> selector,
            Action<IEntityMappingProfile<TMember>> config)
            where TMember: class, new()
        {
            var entityMappingProfile = new EntityMappingProfile<TMember>();
            config(entityMappingProfile);

            return this.ForMember(selector, entityMappingProfile);
        }

        public void Map(object entity, IDataRecord dataRecord)
        {
            if(!this.keyMemberMappers.Any())
            {
                throw new InvalidOperationException("Missing key member mapping, did you forget to set at least one member as the entity key");
            }

            foreach(var mapper in this.memberMappers.Values)
            {
                mapper.Map(entity, dataRecord);
            }

            foreach(var memberMappingProfile in this.memberMappingProfiles)
            {
                var memberInfo = memberMappingProfile.Key;
                var profile = memberMappingProfile.Value;
                var memberInstance = profile.TargetType.CreateInstance();
                profile.Map(memberInstance, dataRecord);
                memberInfo.SetValue(entity, memberInstance);
            }
        }
    }
}