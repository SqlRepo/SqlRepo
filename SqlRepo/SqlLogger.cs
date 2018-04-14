using System;
using System.Collections.Generic;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class SqlLogger : ISqlLogger
    {
        private readonly List<ISqlLogWriter> sqlLogWriters;

        public SqlLogger(List<ISqlLogWriter> sqlLogWriters)
        {
            this.sqlLogWriters = sqlLogWriters;
        }

        public void Log(string sql)
        {
            foreach(var sqlLogWriter in this.sqlLogWriters)
            {
                sqlLogWriter.Log(sql);
            }
        }
    }
}