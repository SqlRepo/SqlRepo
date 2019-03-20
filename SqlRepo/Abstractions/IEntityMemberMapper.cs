using System;
using System.Data;
using System.Reflection;

namespace SqlRepo.Abstractions
{
    public interface IEntityMemberMapper
    {
        int ColumnIndex { get; }
        string ColumnName { get; }
        EntityMemberMappingStrategy MappingStrategy { get; }
        MemberInfo MemberInfo { get; }
        void Map(object entity, IDataRecord dataRecord);
    }
}