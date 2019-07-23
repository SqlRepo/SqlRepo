using System;
using System.Data;
using System.Reflection;
using SqlRepo.Abstractions;
using SqlRepo.ValueMemberSetters;

namespace SqlRepo
{
    public class EntityValueMemberMapper : IEntityValueMemberMapper
    {
        public EntityValueMemberMapper(MemberInfo memberInfo)
        {
            this.MemberInfo = memberInfo;
        }

        public MemberInfo MemberInfo { get; }
        public int ColumnIndex { get; private set; }
        public string ColumnName { get; private set; }
        public bool IsKey { get; private set; }
        public EntityValueMemberMappingStrategy MappingStrategy { get; private set; }

        public bool ValueMatches(object entity, IDataRecord dataRecord)
        {
            var index = this.ColumnIndex;
            if(this.MappingStrategy == EntityValueMemberMappingStrategy.ColumnName)
            {
                index = dataRecord.GetOrdinal(this.ColumnName);
            }

            var dataValue = Convert.ChangeType(dataRecord.GetValue(index), this.MemberInfo.GetUnderlyingType());
            var entityValue = Convert.ChangeType(this.MemberInfo.GetValue(entity), this.MemberInfo.GetUnderlyingType());

            return dataValue.Equals(entityValue);
        }

        public bool ValuesMatch(object entity1, object entity2)
        {
            var value1 = this.MemberInfo.GetValue(entity1);
            var value2 = this.MemberInfo.GetValue(entity2);

            return value1 == value2;
        }

        public void Map(object entity, IDataRecord dataRecord)
        {
            var propertySetter = ValueMemberSetterProvider.Get(this.MemberInfo.GetUnderlyingType());
            if(this.MappingStrategy == EntityValueMemberMappingStrategy.ColumnName)
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

        internal void SetIsKey(bool isKey)
        {
            this.IsKey = isKey;
        }

        internal void SetMappingStrategy(EntityValueMemberMappingStrategy mappingStrategy)
        {
            this.MappingStrategy = mappingStrategy;
        }
    }
}