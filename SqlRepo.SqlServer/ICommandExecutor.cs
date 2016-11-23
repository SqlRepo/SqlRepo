using System.Data;

namespace SqlRepo.SqlServer
{
    public interface ICommandExecutor
    {
        int ExecuteNonQuery(string connectionString, string sql);
        IDataReader ExecuteReader(string connectionString, string sql);

        IDataReader ExecuteStoredProcedure(string connectionString,
            string name,
            params ParameterDefinition[] parametersDefinitions);
    }
}