using System;
using System.Configuration;
using System.Data.SqlClient;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class AppConfigConnectionProvider : ISqlConnectionProvider
    {
        private readonly string connectionName;

        public AppConfigConnectionProvider(string connectionName)
        {
            this.connectionName = connectionName;
        }

      
        public T Provide<T>()
            where T: class, IConnection
        {
            var connectionString = ConfigurationManager.ConnectionStrings[this.connectionName]
                                                       .ConnectionString;
            return new SqlConnectionAdapter(connectionString) as T;
        }
    }
}