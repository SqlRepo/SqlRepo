using System;

namespace SqlRepo.Abstractions
{
    public interface IEntityValueMemberMapperBuilderConfig
    {
        void MapFromColumnName(string columnName, bool isKeyColumn = false);
        void MapFromIndex(int columnIndex, bool isKeyColumn = false);
    }
}