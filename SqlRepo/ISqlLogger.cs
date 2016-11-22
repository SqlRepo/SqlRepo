using System;

namespace SqlRepo
{
    public interface ISqlLogger
    {
        void Log(string sql);
    }
}