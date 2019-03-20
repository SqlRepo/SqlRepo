using System;
using System.Data;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityMemberMapper : IEntityMemberMapper
    {
        public EntityMemberMapper(MemberInfo memberInfo)
        {
            this.MemberInfo = memberInfo;
        }

        public MemberInfo MemberInfo { get; }
        public int ColumnIndex { get; private set; }
        public string ColumnName { get; private set; }
        public EntityMemberMappingStrategy MappingStrategy { get; private set; }

        public void Map(object entity, IDataRecord dataRecord)
        {
            var propertySetter = PropertySetterProvider.Get(this.MemberInfo.GetUnderlyingType());
            if(this.MappingStrategy == EntityMemberMappingStrategy.ColumnName)
            {
                propertySetter.SetFromColumnName(this.MemberInfo, entity, dataRecord, this.ColumnName);
            }
            else
            {
                propertySetter.SetFromColumnIndex(this.MemberInfo, entity, dataRecord, this.ColumnIndex);
            }
        }

        internal void SetColumnIndex(int index)
        {
            this.ColumnIndex = index;
        }

        internal void SetColumnName(string columnName)
        {
            this.ColumnName = columnName;
        }

        internal void SetMappingStrategy(EntityMemberMappingStrategy mappingStrategy)
        {
            this.MappingStrategy = mappingStrategy;
        }
    }
}