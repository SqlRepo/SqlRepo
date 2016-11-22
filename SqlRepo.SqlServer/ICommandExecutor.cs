using System;
using System.Data;

namespace SqlRepo.SqlServer
{
    public interface ICommandExecutor
    {
        int ExecuteNonQuery(string sql);
        IDataReader ExecuteReader(string sql);
        IDataReader ExecuteStoredProcedure(string name, params ParameterDefinition[] parametersDefinitions);
    }
}