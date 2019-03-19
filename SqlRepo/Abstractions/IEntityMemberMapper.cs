using System;
using System.Data;
using System.Reflection;

namespace SqlRepo.Abstractions
{
    public interface IEntityMemberMapper<T>
        where T: class, new()
    {
        int ColumnIndex { get; }
        string ColumnName { get; }
        EntityMemberMappingStrategy MappingStrategy { get; }
        MemberInfo MemberInfo { get; }
        void Map(T entity, IDataRecord dataRecord);
    }
}