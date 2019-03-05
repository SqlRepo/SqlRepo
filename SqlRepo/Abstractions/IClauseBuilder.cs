using System;

namespace SqlRepo.Abstractions
{
    public interface IClauseBuilder
    {
        string Sql();
        bool IsClean { get; set; }
    }
}