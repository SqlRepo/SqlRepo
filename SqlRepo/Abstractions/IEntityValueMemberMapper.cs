using System;
using System.Data;

namespace SqlRepo.Abstractions
{
    public interface IEntityValueMemberMapper : IEntityMemberMapper
    {
        int ColumnIndex { get; }
        string ColumnName { get; }
        bool IsKey { get; }
        EntityValueMemberMappingStrategy MappingStrategy { get; }
        bool ValueMatches(object entity, IDataRecord dataRecord);
        bool ValuesMatch(object entity1, object entity2);
    }
}