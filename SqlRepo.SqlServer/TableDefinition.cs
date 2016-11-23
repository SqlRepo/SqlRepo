using System;

namespace SqlRepo.SqlServer
{
    public class TableDefinition
    {
        public Type TableType { get; set; }
        public string Name { get; set; }
        public string Schema { get; set; }
        public string Alias { get; set; }
    }
}