using System.Collections.Generic;
using System.Linq;

namespace SqlRepo.SqlServer
{
    public class FilterGroup
    {
        public FilterGroup()
        {
            this.Groups = new List<FilterGroup>();
            this.Conditions = new List<FilterCondition>();
        }

        public IList<FilterCondition> Conditions { get; internal set; }
        public IList<FilterGroup> Groups { get; internal set; }
        public FilterGroupType GroupType { get; internal set; }
        public FilterGroup Parent { get; internal set; }

        public override string ToString()
        {
            var prefix = $"{this.GroupType.ToString().ToUpperInvariant()}";
            var groups = this.Groups.Any()? $"\n{string.Join("\n", this.Groups)}":string.Empty;
            var conditions = $"({string.Join("\n", this.Conditions)}{groups})";
            return $"{prefix} {conditions}";
        }
    }
}