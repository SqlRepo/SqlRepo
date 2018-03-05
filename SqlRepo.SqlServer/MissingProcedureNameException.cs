using System;

namespace SqlRepo.SqlServer
{
    public class MissingProcedureNameException : Exception
    {
        public MissingProcedureNameException()
            : base(
                "The name of the procedure is missing, you need to call WithName to set the procedure name before executing the statement.") { }
    }
}