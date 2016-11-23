namespace SqlRepo.SqlServer
{
    public class ColumnSelection
    {
        public Aggregation Aggregation { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Schema { get; set; }
        public string Table { get; set; }

        public override string ToString()
        {
            var prefix = string.IsNullOrWhiteSpace(this.Alias)
                             ? $"[{this.Schema}].[{this.Table}]."
                             : $"[{this.Alias}].";

            var columnExpression = this.Name == "*"? $"{prefix}*": $"{prefix}[{this.Name}]";
            return this.Aggregation == Aggregation.None
                       ? columnExpression
                       : this.ApplyAggregation(columnExpression);
        }

        private string ApplyAggregation(string columnExpression)
        {
            if(this.Aggregation == Aggregation.Count && this.Name == "*")
            {
                return "COUNT(*)";
            }

            return $"{this.Aggregation.ToString() .ToUpperInvariant()}({columnExpression}) AS [{this.Name}]";
        }
    }
}