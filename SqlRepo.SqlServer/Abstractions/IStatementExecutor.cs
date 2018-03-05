using System;
using System.Data;
using System.Threading.Tasks;
using SqlRepo.Abstractions;

namespace SqlRepo.SqlServer.Abstractions
{
    public interface IStatementExecutor
    {
        int ExecuteNonQuery(string sql);
        Task<int> ExecuteNonQueryAsync(string sql);

        int ExecuteNonQueryStoredProcedure(string name, params ParameterDefinition[] parameterDefinitions);

        Task<int> ExecuteNonQueryStoredProcedureAsync(string name,
            params ParameterDefinition[] parameterDefinitions);

        IDataReader ExecuteReader(string sql);
        Task<IDataReader> ExecuteReaderAsync(string sql);

        IDataReader ExecuteStoredProcedure(string name, params ParameterDefinition[] parametersDefinitions);

        Task<IDataReader> ExecuteStoredProcedureAsync(string name,
            params ParameterDefinition[] parametersDefinitions);

        IStatementExecutor UseConnectionProvider(IConnectionProvider connectionProvider);
    }
}