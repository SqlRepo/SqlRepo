using System.Data;
using System.Threading.Tasks;

namespace SqlRepo.SqlServer
{
    public interface ICommandExecutor
    {
        int ExecuteNonQuery(string connectionString, string sql);
        Task<int> ExecuteNonQueryAsync(string connectionString, string sql);
        IDataReader ExecuteReader(string connectionString, string sql);
        Task<IDataReader> ExecuteReaderAsync(string connectionString, string sql);

        IDataReader ExecuteStoredProcedure(string connectionString,
            string name,
            params ParameterDefinition[] parametersDefinitions);

        Task<IDataReader> ExecuteStoredProcedureAsync(string connectionString,
            string name,
            params ParameterDefinition[] parametersDefinitions);
    }
}