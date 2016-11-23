using System;

namespace SqlRepo.SqlServer
{
    internal class HavingSpecification
    {
        public Aggregation Aggregation { get; set; }
        public string Alias { get; set; }
        public Comparison Comparison { get; set; }
        public string Name { get; set; }
        public string Schema { get; set; }
        public string Table { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            var prefix = string.IsNullOrWhiteSpace(this.Alias)
                             ? $"[{this.Schema}].[{this.Table}]."
                             : $"[{this.Alias}].";

            var columnExpression = this.Name == "*"? $"{prefix}*": $"{prefix}[{this.Name}]";
            return $"{this.ApplyAggregation(columnExpression)} {this.ComparisonExpression()}";
        }

        private string ApplyAggregation(string columnExpression)
        {
            if(this.Aggregation == Aggregation.Count && this.Name == "*")
            {
                return "COUNT(*)";
            }

            return $"{this.Aggregation.ToString() .ToUpperInvariant()}({columnExpression})";
        }

        private string ComparisonExpression()
        {
            return $"{this.GetOperatorString()} {this.Value}";
        }

        private string GetOperatorString()
        {
            switch(this.Comparison)
            {
                case Comparison.Equal:
                    return "=";
                case Comparison.GreaterThan:
                    return ">";
                case Comparison.LessThan:
                    return "<";
                case Comparison.Like:
                    return "LIKE";
                case Comparison.NotEqual:
                    return "<>";
                case Comparison.NotLike:
                    return "NOT LIKE";
                default:
                    return null;
            }
        }
    }
}