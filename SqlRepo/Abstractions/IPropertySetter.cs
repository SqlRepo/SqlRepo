using System;
using System.Data;
using System.Reflection;

namespace SqlRepo.Abstractions
{
    public interface IPropertySetter
    {
        bool CanSet(Type type);

        void SetFromColumnName<T>(MemberInfo memberInfo, T entity, IDataRecord dataRecord, string columnName)
            where T: class, new();

        void SetFromColumnIndex<T>(MemberInfo memberInfo, T entity, IDataRecord dataRecord, int columnIndex);
    }
}