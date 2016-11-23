using System;

namespace SqlRepo.SqlServer
{
    public class ColumnSpecification
    {
        public Aggregation Aggregation { get; set; }
        public string Alias { get; set; }
        public Type EntityType { get; set; }
        public string Identifier { get; set; }
        internal string Table { get; set; }
        internal string Schema { get; set; }

        public override string ToString()
        {
            var prefix = string.IsNullOrWhiteSpace(this.Alias)
                             ? $"[{this.Schema}].[{this.Table}]."
                             : $"[{this.Alias}].";

            var columnExpression = this.Identifier == "*"? $"{prefix}*": $"{prefix}[{this.Identifier}]";
            return this.Aggregation == Aggregation.None? columnExpression: this.ApplyAggregation(columnExpression);
        }

        private string ApplyAggregation(string columnExpression)
        {
            if(this.Aggregation == Aggregation.Count && this.Identifier == "*")
            {
                return "COUNT(*) AS [Count]";
            }

            return $"{this.Aggregation.ToString().ToUpperInvariant()}({columnExpression}) AS [{this.Identifier}]";
        }
    }
}