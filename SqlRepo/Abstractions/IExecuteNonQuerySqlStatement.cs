using System;

namespace SqlRepo.Abstractions
{
    public interface IExecuteNonQuerySqlStatement : IExecuteSqlStatement<int> { }
}