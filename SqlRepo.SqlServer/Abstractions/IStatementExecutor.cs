using System;
using System.Data;
using System.Threading.Tasks;

namespace SqlRepo.SqlServer.Abstractions
{
    public interface IStatementExecutor
    {
        int ExecuteNonQuery(string sql);
        Task<int> ExecuteNonQueryAsync(string sql);
        IDataReader ExecuteReader(string sql);
        Task<IDataReader> ExecuteReaderAsync(string sql);

        IDataReader ExecuteStoredProcedure(
            string name,
            params ParameterDefinition[] parametersDefinitions);

        Task<IDataReader> ExecuteStoredProcedureAsync(
            string name,
            params ParameterDefinition[] parametersDefinitions);
    }
}