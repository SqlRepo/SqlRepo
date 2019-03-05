using System;

namespace SqlRepo.Abstractions
{
    public interface IExecuteNonQueryProcedureStatement : IExecuteProcedureStatement<int> { }
}