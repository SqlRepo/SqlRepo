using System;
using System.Threading.Tasks;

namespace SqlRepo.Abstractions
{
    public interface IExecuteSqlStatement<TReturn>
    {
        TReturn Go();
        Task<TReturn> GoAsync();
        IExecuteSqlStatement<TReturn> WithSql(string sql);
        IExecuteSqlStatement<TReturn> UseConnectionProvider(IConnectionProvider connectionProvider);
    }
}