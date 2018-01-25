using System;

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