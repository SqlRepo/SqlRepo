using System;
using System.Data;
using System.Reflection;

namespace SqlRepo.Abstractions
{
    public interface IEntityMemberMapper
    {
        MemberInfo MemberInfo { get; }
        void Map(object entity, IDataRecord dataRecord);
    }
}