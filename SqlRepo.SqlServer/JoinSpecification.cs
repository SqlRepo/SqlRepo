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
            var leftPrefix = string.IsNullOrEmpty(LeftTableAlias)
                ? $"[{LeftSchema}].[{LeftTableName}]"
                : $"[{LeftTableAlias}]";

            var rightPrefix = string.IsNullOrEmpty(RightTableAlias)
                ? $"[{RightSchema}].[{RightTableName}]"
                : $"[{RightTableAlias}]";

            var prefix = GetPrefix();

            return
                $"\n{prefix} {leftPrefix}.[{LeftIdentifier}] {Operator} {rightPrefix}.[{RightIdentifier}]";
        }

        private string GetPrefix()
        {
            switch (LogicalOperator)
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