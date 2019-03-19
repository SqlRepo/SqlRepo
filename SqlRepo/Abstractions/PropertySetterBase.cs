using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SqlRepo.Abstractions
{
    public abstract class PropertySetterBase : IPropertySetter
    {
        protected PropertySetterBase(IEnumerable<Type> supportedTypes)
        {
            this.SupportedTypes = supportedTypes;
        }

        public IEnumerable<Type> SupportedTypes { get; }

        public virtual bool CanSet(Type type)
        {
            return this.SupportedTypes.Contains(type);
        }

        public void SetFromColumnIndex<T>(MemberInfo memberInfo,
            T entity,
            IDataRecord dataRecord,
            int columnIndex)
        {
            var value = this.GetValueByColumnIndex(dataRecord, columnIndex);
            if(value == null)
            {
                return;
            }

            if(memberInfo.MemberType == MemberTypes.Field)
            {
                ((FieldInfo)memberInfo).SetValue(entity, value);
            }

            if(memberInfo.MemberType == MemberTypes.Property)
            {
                ((PropertyInfo)memberInfo).SetValue(entity, value);
            }
        }

        public void SetFromColumnName<T>(MemberInfo memberInfo,
            T entity,
            IDataRecord dataRecord,
            string columnName)
            where T: class, new()
        {
            var index = dataRecord.GetOrdinal(columnName);
            this.SetFromColumnIndex(memberInfo, entity, dataRecord, index);
        }

        protected abstract object GetValueByColumnIndex(IDataRecord dataRecord, int columnIndex);
    }
}