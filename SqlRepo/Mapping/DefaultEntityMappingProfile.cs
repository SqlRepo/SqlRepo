using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public sealed class DefaultEntityMappingProfile : IEntityMappingProfile
    {
        private readonly IDictionary<MemberInfo, IEntityValueMemberMapper> mappers = new Dictionary<MemberInfo, IEntityValueMemberMapper>();

        public DefaultEntityMappingProfile(Type targetType)
        {
            this.TargetType = targetType;
            this.PrepareMemberMappers();
        }

        private void PrepareMemberMappers()
        {
            var members = this.TargetType.GetPropertyAndFieldMembers();
            foreach(var memberInfo in members)
            {
                if(!memberInfo.GetUnderlyingType().IsMappableType())
                {
                    continue;
                }

                var builder = new EntityValueMemberMapperBuilder(memberInfo);
                builder.MapFromColumnName(memberInfo.Name);
                
                this.mappers.Add(memberInfo, builder.Build());
            }
        }

        public Type TargetType { get; }
        
        public void Map(object entity, IDataRecord dataRecord)
        {
            foreach(var mapper in this.mappers.Values)
            {
                mapper.Map(entity, dataRecord);
            }
        }

        public bool DataRecordMatchesEntity(object entity, IDataRecord dataRecord)
        {
            return false;
        }
    }
}