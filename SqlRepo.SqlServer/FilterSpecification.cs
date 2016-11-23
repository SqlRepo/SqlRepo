using System;

namespace SqlRepo.SqlServer
{
    public class FilterSpecification
    {
        public string Alias { get; set; }
        public Type EntityType { get; set; }
        public string Identifier { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }
}