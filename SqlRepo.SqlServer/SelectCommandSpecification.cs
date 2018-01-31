using System.Collections.Generic;
using System.Linq;

namespace SqlRepo.SqlServer
{
    public class SelectCommandSpecification
    {
        private const string CommandTemplate = "{0}{1}{2}{3}{4}{5};";

        public SelectCommandSpecification()
        {
            Columns = new List<ColumnSpecification>();
            Tables = new List<SelectCommandTableSpecification>();
            Joins = new List<JoinSpecification>();
            Filters = new List<FilterGroup>();
            Orderings = new List<OrderSpecification>();
            Groupings = new List<GroupSpecification>();
            Havings = new List<SelectCommandHavingSpecification>();
        }

        public IList<ColumnSpecification> Columns { get; }
        public IList<FilterGroup> Filters { get; set; }
        public IList<GroupSpecification> Groupings { get; }
        public IList<SelectCommandHavingSpecification> Havings { get; }
        public IList<JoinSpecification> Joins { get; }
        public IList<OrderSpecification> Orderings { get; }
        public IList<SelectCommandTableSpecification> Tables { get; }
        public int? Top { get; internal set; }
        public bool UseTopPercent { get; internal set; }
        public bool NoLocks { get; internal set; }

        public override string ToString()
        {
            var selectClause = BuildSelectClause();
            var fromClause = BuildFromClause();
            var whereClause = BuildWhereClause();
            var orderByClause = BuildOrderByClause();
            var groupByClause = BuildGroupByClause();
            var havingClause = BuildHavingClause();
            return string.Format(CommandTemplate,
                selectClause,
                fromClause,
                whereClause,
                groupByClause,
                orderByClause,
                havingClause);
        }

        private string BuildFromClause()
        {
            var result = string.Empty;
            foreach (var specification in Tables)
            {
                specification.NoLocks = NoLocks;
                result += specification.ToString();
                if (specification.JoinType == JoinType.None)
                {
                    continue;
                }

                result = Joins.Where(
                    joinSpecification =>
                        joinSpecification.RightEntityType == specification.EntityType
                        && joinSpecification.RightTableAlias == specification.Alias)
                    .Aggregate(result,
                        (current, joinSpecification) => current + joinSpecification.ToString());
            }

            return result;
        }

        private string BuildGroupByClause()
        {
            return !Groupings.Any() ? string.Empty : $"\nGROUP BY {string.Join("\n, ", Groupings)}";
        }

        private string BuildHavingClause()
        {
            return !Havings.Any() ? string.Empty : $"\nHAVING {string.Join("\n, ", Havings)}";
        }

        private string BuildOrderByClause()
        {
            return !Orderings.Any() ? string.Empty : $"\nORDER BY {string.Join("\n, ", Orderings)}";
        }

        private string BuildSelectClause()
        {
            const string ClauseTemplate = "SELECT {0}{1}";

            string top = Top.HasValue ? $"TOP ({Top}) " : string.Empty;

            var selections = string.Join("\n, ",
                Columns.Select(c => c.ToString())
                    .ToArray());
            return string.Format(ClauseTemplate, top, string.IsNullOrEmpty(selections) ? "*" : selections);
        }

        private string BuildWhereClause()
        {
            return !Filters.Any() ? string.Empty : $"\n{string.Join("\n", Filters)}";
        }
    }
}