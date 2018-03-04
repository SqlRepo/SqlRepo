using System;
using System.Configuration;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer.ConnectionProviders
{
    public class AppConfigFirstConnectionProvider : ISqlConnectionProvider
    {
       
        public TConnection Provide<TConnection>()
            where TConnection: class, IConnection
        {
            var connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
            return new SqlConnectionAdapter(connectionString) as TConnection;
        }
    }
}