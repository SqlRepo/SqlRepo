using System;

namespace SqlRepo.SqlServer
{
    internal class OrderBySpecification
    {
        public string Alias { get; set; }
        public OrderByDirection Direction { get; set; }
        public string Name { get; set; }
        public string Schema { get; set; }
        public string Table { get; set; }

        public override string ToString()
        {
            var prefix = string.IsNullOrWhiteSpace(this.Alias)
                       ? $"[{this.Schema}].[{this.Table}]."
                             : $"[{this.Alias}].";

            var direction = this.Direction == OrderByDirection.Ascending? "ASC": "DESC";

            return $"{prefix}[{this.Name}] {direction}";
        }
    }
}