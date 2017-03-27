using System;

namespace SqlRepo.SqlServer
{
    public class FilterCondition
    {
        public string Alias { get; internal set; }
        public Type EntityType { get; internal set; }
        public string Left { get; internal set; }
        public LogicalOperator LocigalOperator { get; internal set; }
        public string Operator { get; internal set; }
        public string Right { get; internal set; }
        internal string Schema { get; set; }
        internal string TableName { get; set; }

        public override string ToString()
        {
            var prefix = this.LocigalOperator == LogicalOperator.NotSet
                             ? string.Empty
                             : $"{this.LocigalOperator.ToString().ToUpperInvariant()} ";
             var identifierPrefix = string.IsNullOrEmpty(this.Alias)
                             ? $"[{this.Schema}].[{this.TableName}]"
                             : $"[{this.Alias}]";

            if (this.Right == "NULL")
            {
                if (this.Operator == "=")
                {
                    this.Operator = "IS";
                }
                else if (this.Operator == "<>")
                {
                    this.Operator = "IS NOT";
                }
            }

            return $"{prefix}{identifierPrefix}.[{this.Left}] {this.Operator} {this.Right}";
        }
    }
}