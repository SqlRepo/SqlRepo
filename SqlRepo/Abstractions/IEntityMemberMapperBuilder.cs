using System;

namespace SqlRepo.Abstractions
{
    public interface IEntityMemberMapperBuilder : IEntityMemberMapperBuilderConfig
    {
        IEntityMemberMapper Build();
        void MapFromColumnName(string columnName);
        void MapFromIndex(int columnIndex);
    }
}