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

        public void MapFromColumnName(string columnName, bool isKeyColumn = false)
        {
            this.mapper.SetMappingStrategy(EntityValueMemberMappingStrategy.ColumnName);
            this.mapper.SetColumnName(columnName);
            this.mapper.SetIsKey(isKeyColumn);
        }

        public void MapFromIndex(int columnIndex, bool isKeyColumn = false)
        {
            this.mapper.SetMappingStrategy(EntityValueMemberMappingStrategy.ColumnIndex);
            this.mapper.SetColumnIndex(columnIndex);
            this.mapper.SetIsKey(isKeyColumn);
        }
    }
}