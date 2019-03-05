using System;

namespace SqlRepo.SqlServer
{
    public class MissingSqlException : Exception
    {
        public MissingSqlException()
            : base(
                "The SQL to execute is missing, you need to call WithSql before executing the statement.") { }
    }
}