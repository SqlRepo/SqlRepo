using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlRepo.SqlServer
{
    internal class TableSpecification
    {
        private const string Template = "{0} [{1}].[{2}]{3}{4}";

        public TableSpecification()
        {
            this.Conditions = new List<JoinCondition>();
        }

        public IList<JoinCondition> Conditions { get; }
        public string LeftAlias { get; set; }
        public string LeftSchema { get; set; }
        public string LeftTable { get; set; }
        public Type LeftType { get; set; }
        public string RightAlias { get; set; }
        public string RightSchema { get; set; }
        public string RightTable { get; set; }
        public Type RightType { get; set; }
        public string SpecificationType { get; set; }

        public override string ToString()
        {
            var conditions = this.Conditions.Any()
                                 ? $"\n{string.Join("\n", this.Conditions)}"
                                 : string.Empty;
            return string.Format(Template,
                this.SpecificationType,
                this.RightSchema,
                this.RightTable,
                string.IsNullOrWhiteSpace(this.RightAlias)? string.Empty: $" AS [{this.RightAlias}]",
                conditions);
        }
    }
}