using System;

namespace SqlRepo
{
    public interface ISqlCommand<out TResult> : IClauseBuilder
    {
        string TableName { get; }
        string TableSchema { get; }
        string ConnectionString { get; }
        TResult Go(string connectionString = null);
        void UseConnectionString(string connectionString);
    }
}