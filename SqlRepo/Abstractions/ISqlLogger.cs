using System;

namespace SqlRepo.Abstractions
{
    public interface ISqlLogger
    {
        void Log(string sql);
    }
}