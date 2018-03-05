using System;
using System.Threading.Tasks;
using SqlRepo.Abstractions;

namespace SqlRepo.SqlServer.Abstractions
{
    public abstract class ExecuteSqlStatementBase<TResult> : IExecuteSqlStatement<TResult>
    {
        protected ExecuteSqlStatementBase(IStatementExecutor statementExecutor)
        {
            this.StatementExecutor = statementExecutor;
        }

        public string Sql { get; private set; }
        protected IStatementExecutor StatementExecutor { get; }

        public abstract TResult Go();
        public abstract Task<TResult> GoAsync();

        public IExecuteSqlStatement<TResult> UseConnectionProvider(IConnectionProvider connectionProvider)
        {
            this.StatementExecutor.UseConnectionProvider(connectionProvider);
            return this;
        }

        public IExecuteSqlStatement<TResult> WithSql(string sql)
        {
            this.Sql = sql;
            return this;
        }
    }
}