using System;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class ConsoleSqlLogger : ISqlLogger
    {
        public void Log(string sql)
        {
            Console.WriteLine(sql);
        }
    }
}