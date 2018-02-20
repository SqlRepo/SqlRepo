using System;
using Microsoft.Extensions.Configuration;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class AppSettingsConnectionProvider : ISqlConnectionProvider
    {
        private readonly IConfiguration configuration;
        private readonly string connectionName;

        public AppSettingsConnectionProvider(IConfiguration configuration, string connectionName)
        {
            this.configuration = configuration;
            this.connectionName = connectionName;
        }

        public ISqlConnection Provide()
        {
            var connectionString = this.configuration.GetConnectionString(this.connectionName);
            return new SqlConnectionAdapter(connectionString);
        }
    }
}