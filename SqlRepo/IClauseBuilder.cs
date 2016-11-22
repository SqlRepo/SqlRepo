using System;

namespace SqlRepo
{
    public interface IClauseBuilder
    {
        string Sql();
        bool IsClean { get; set; }
    }
}