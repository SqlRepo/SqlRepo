using System;

namespace SqlRepo.Abstractions
{
    public interface ISqlLogWriter
    {
        void Log(string sql);
    }
}