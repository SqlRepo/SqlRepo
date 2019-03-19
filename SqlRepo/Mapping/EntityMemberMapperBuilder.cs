using System;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityMemberMapperBuilder<T, TMember> : IEntityMemberMapperBuilder<T, TMember>
        where T: class, new()
    {
        private readonly EntityMemberMapper<T> mapper;

        public EntityMemberMapperBuilder(MemberInfo memberInfo)
        {
            this.mapper = new EntityMemberMapper<T>(memberInfo);
        }

        public IEntityMemberMapper<T> Build()
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