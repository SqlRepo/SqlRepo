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

        public T Provide<T>()
            where T: class, IConnection
        {
            var connectionString = this.configuration.GetConnectionString(this.connectionName);

            return new SqlConnectionAdapter(connectionString) as T;
        }
    }
}