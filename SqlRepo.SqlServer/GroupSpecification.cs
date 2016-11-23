using System;

namespace SqlRepo.SqlServer
{
    public class GroupSpecification
    {
        public string Alias { get; internal set; }
        public Type EntityType { get; internal set; }
        public string Identifer { get; internal set; }
        internal string Schema { get; set; }
        internal string TableName { get; set; }

        public override string ToString()
        {
               var prefix = string.IsNullOrEmpty(this.Alias)
                             ? $"[{this.Schema}].[{this.TableName}]"
                             : $"[{this.Alias}]";
            return $"{prefix}.[{this.Identifer}]";
        }
    }
}