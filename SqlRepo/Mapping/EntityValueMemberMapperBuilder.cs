using System;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityValueMemberMapperBuilder : IEntityValueMemberMapperBuilder
    {
        private readonly EntityValueMemberMapper mapper;

        public EntityValueMemberMapperBuilder(MemberInfo memberInfo)
        {
            this.mapper = new EntityValueMemberMapper(memberInfo);
        }

        public IEntityValueMemberMapper Build()
        {
            return this.mapper;
        }

        public void MapFromColumnName(string columnName)
        {
            this.mapper.SetMappingStrategy(EntityMemberMappingStrategy.ColumnName);
            this.mapper.SetColumnName(columnName);
        }

        public void MapFromIndex(int columnIndex)
        {
            this.mapper.SetMappingStrategy(EntityMemberMappingStrategy.ColumnIndex);
            this.mapper.SetColumnIndex(columnIndex);
        }
    }
}