using System;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class ConsoleSqlLogger : ISqlLogWriter
    {
        public void Log(string sql)
        {
            Console.WriteLine(sql);
        }
    }
}