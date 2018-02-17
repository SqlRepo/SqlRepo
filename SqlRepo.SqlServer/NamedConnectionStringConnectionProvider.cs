using System;
using System.Configuration;
using System.Data.SqlClient;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class NamedConnectionStringConnectionProvider : ISqlConnectionProvider
    {
        private readonly string connectionName;

        public NamedConnectionStringConnectionProvider(string connectionName)
        {
            this.connectionName = connectionName;
        }

        public ISqlConnection Provide()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[this.connectionName]
                                                       .ConnectionString;
            return new SqlConnectionAdapter(connectionString);
        }
    }
}