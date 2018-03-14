using System;

namespace SqlRepo.SqlServer
{
    internal class JoinCondition
    {
        private const string Template = "{0} {1} {2} {3}";
        public string LeftIdentifier { get; set; }
        public string LeftTableAlias { get; set; }
        public string LeftTableName { get; set; }
        public string LeftTableSchema { get; set; }
        public LogicalOperator LogicalOperator { get; set; }
        public string Operator { get; set; }
        public string RightIdentifier { get; set; }
        public string RightTableAlias { get; set; }
        public string RightTableName { get; set; }
        public string RightTableSchema { get; set; }

        public override string ToString()
        {
            var actualOperator = this.LogicalOperator == LogicalOperator.NotSet
                                     ? "ON"
                                     : this.LogicalOperator.ToString()
                                           .ToUpperInvariant();
            var leftIdentifier = string.IsNullOrWhiteSpace(this.LeftTableAlias)
                                     ? $"[{this.LeftTableSchema}].[{this.LeftTableName}].[{this.LeftIdentifier}]"
                                     : $"[{this.LeftTableAlias}].[{this.LeftIdentifier}]";

            var rightIdentifier = string.IsNullOrWhiteSpace(this.RightTableAlias)
                                      ? $"[{this.RightTableSchema}].[{this.RightTableName}].[{this.RightIdentifier}]"
                                      : $"[{this.RightTableAlias}].[{this.RightIdentifier}]";
            return string.Format(Template, actualOperator, leftIdentifier, this.Operator, rightIdentifier);
        }
    }
}