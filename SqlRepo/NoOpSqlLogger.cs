using System;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class NoOpSqlLogger : ISqlLogWriter
    {
        public void Log(string sql) { }
    }
}