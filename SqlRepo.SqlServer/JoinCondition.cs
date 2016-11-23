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
            var actualOperator = LogicalOperator == LogicalOperator.NotSet
                ? "ON"
                : LogicalOperator.ToString()
                    .ToUpperInvariant();
            var leftIdentifier = string.IsNullOrWhiteSpace(LeftTableAlias)
                ? $"[{LeftTableSchema}].[{LeftTableName}].[{LeftIdentifier}]"
                : $"[{LeftTableAlias}].[{LeftIdentifier}]";

            var rightIdentifier = string.IsNullOrWhiteSpace(RightTableAlias)
                ? $"[{RightTableSchema}].[{RightTableName}].[{RightIdentifier}]"
                : $"[{RightTableAlias}].[{RightIdentifier}]";
            return string.Format(Template, actualOperator, leftIdentifier, Operator, rightIdentifier);
        }
    }
}