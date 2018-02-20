using System;

namespace SqlRepo
{
    public class NoOpSqlLogger : ISqlLogger
    {
        public void Log(string sql) { }
    }
}