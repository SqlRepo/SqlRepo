using System;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityMemberMapperBuilder : IEntityMemberMapperBuilder
    {
        private readonly EntityMemberMapper mapper;

        public EntityMemberMapperBuilder(MemberInfo memberInfo)
        {
            this.mapper = new EntityMemberMapper(memberInfo);
        }

        public IEntityMemberMapper Build()
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