using System;

namespace SqlRepo.SqlServer
{
    public class SelectCommandHavingSpecification
    {
        public Aggregation Aggregation { get; internal set; }
        public string Alias { get; internal set; }
        public Type EntityType { get; internal set; }
        public string Identifier { get; internal set; }
        public string Operator { get; internal set; }
        public object Value { get; internal set; }
        internal string Schema { get; set; }
        internal string TableName { get; set; }

        public override string ToString()
        {
            var identifierPrefix = string.IsNullOrEmpty(this.Alias)
                                       ? $"[{this.Schema}].[{this.TableName}]"
                                       : $"[{this.Alias}]";
            var identifer = $"{identifierPrefix}.[{this.Identifier}]";
            return $"{this.ApplyAggregation(identifer)} {this.Operator} {this.Value}";
        }

        private string ApplyAggregation(string columnExpression)
        {
            if(this.Aggregation == Aggregation.Count && this.Identifier == "*")
            {
                return "COUNT(*)";
            }

            return $"{this.Aggregation.ToString() .ToUpperInvariant()}({columnExpression})";
        }
    }
}