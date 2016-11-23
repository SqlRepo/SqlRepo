using System;

namespace SqlRepo.SqlServer
{
    public class SelectCommandTableSpecification
    {
        public string Alias { get; internal set; }
        public Type EntityType { get; internal set; }
        public JoinType JoinType { get; set; }
        public bool NoLocks { get; internal set; }
        public string Schema { get; internal set; }
        public string TableName { get; internal set; }

        public override string ToString()
        {
            var prefix = this.GetPrefix();
            var qualifiedTableName = $"[{this.Schema}].[{this.TableName}]";
            var alias = string.IsNullOrEmpty(this.Alias)? string.Empty: $" AS [{this.Alias}]";
            var options = this.NoLocks? $"\nWITH ( NOLOCK )": string.Empty;
            return $"\n{prefix} {qualifiedTableName}{alias}{options}";
        }

        private string GetPrefix()
        {
            switch(this.JoinType)
            {
                case JoinType.Cross:
                    return "CROSS JOIN";
                case JoinType.Full:
                    return "FULL JOIN";
                case JoinType.Inner:
                    return "INNER JOIN";
                case JoinType.LeftOuter:
                    return "LEFT OUTER JOIN";
                case JoinType.RightOuter:
                    return "RIGHT OUTER JOIN";
                default:
                    return "FROM";
            }
        }
    }
}