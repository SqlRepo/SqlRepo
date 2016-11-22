using System;

namespace SqlRepo.SqlServer
{
    public class ColumnMapping
    {
        public ColumnMapping(int index, string name)
        {
            this.Index = index;
            this.Name = name;
        }

        public int Index { get; private set; }
        public string Name { get; private set; }
    }
}