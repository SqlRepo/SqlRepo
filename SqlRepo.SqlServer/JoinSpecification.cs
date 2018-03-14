using System;

namespace SqlRepo.SqlServer
{
    public class JoinSpecification
    {
        public Type LeftEntityType { get; internal set; }
        public string LeftIdentifier { get; internal set; }
        public string LeftTableAlias { get; internal set; }
        public LogicalOperator LogicalOperator { get; internal set; }
        public string Operator { get; internal set; }
        public Type RightEntityType { get; internal set; }
        public string RightIdentifier { get; internal set; }
        public string RightTableAlias { get; internal set; }
        internal string LeftSchema { get; set; }
        internal string LeftTableName { get; set; }
        internal string RightSchema { get; set; }
        internal string RightTableName { get; set; }

        public override string ToString()
        {
            var leftPrefix = string.IsNullOrEmpty(this.LeftTableAlias)
                                 ? $"[{this.LeftSchema}].[{this.LeftTableName}]"
                                 : $"[{this.LeftTableAlias}]";

            var rightPrefix = string.IsNullOrEmpty(this.RightTableAlias)
                                  ? $"[{this.RightSchema}].[{this.RightTableName}]"
                                  : $"[{this.RightTableAlias}]";

            var prefix = this.GetPrefix();

            return
                $"\n{prefix} {leftPrefix}.[{this.LeftIdentifier}] {this.Operator} {rightPrefix}.[{this.RightIdentifier}]";
        }

        private string GetPrefix()
        {
            switch(this.LogicalOperator)
            {
                case LogicalOperator.And:
                    return "AND";
                case LogicalOperator.Or:
                    return "OR";
                default:
                    return "ON";
            }
        }
    }
}