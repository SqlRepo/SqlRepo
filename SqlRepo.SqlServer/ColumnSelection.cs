using System;

namespace SqlRepo.SqlServer
{
    public class ColumnSelection
    {
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Table { get; set; }
        public string Schema { get; set; }

        public override string ToString()
        {
            var prefix = string.IsNullOrWhiteSpace(this.Alias)
                       ? $"[{this.Schema}].[{this.Table}]."
                             : $"[{this.Alias}].";

            return this.Name == "*"? $"{prefix}*": $"{prefix}[{this.Name}]";
        }
    }
}