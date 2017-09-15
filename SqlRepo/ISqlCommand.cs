using System;
using System.Threading.Tasks;

namespace SqlRepo
{
    public interface ISqlCommand<TResult> : IClauseBuilder
    {
        string ConnectionString { get; }
        string TableName { get; }
        string TableSchema { get; }
        TResult Go(string connectionString = null);
        Task<TResult> GoAsync(string connectionString = null);
        void UseConnectionString(string connectionString);
    }
}