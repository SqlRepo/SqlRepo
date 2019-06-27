using System;

namespace SqlRepo.Abstractions
{
    public interface IEntityValueMemberMapperBuilderConfig
    {
        void MapFromColumnName(string columnName);
        void MapFromIndex(int columnIndex);
    }
}