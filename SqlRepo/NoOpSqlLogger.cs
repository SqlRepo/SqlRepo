using System;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class NoOpSqlLogger : ISqlLogger
    {
        public void Log(string sql) { }
    }
}