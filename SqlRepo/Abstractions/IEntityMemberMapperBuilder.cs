using System;

namespace SqlRepo.Abstractions
{
    public interface IEntityMemberMapperBuilder<T, TMember> : IEntityMemberMapperBuilderConfig<T, TMember>
        where T: class, new()
    {
        IEntityMemberMapper<T> Build();
        void MapFromColumnName(string columnName);
        void MapFromIndex(int columnIndex);
    }
}