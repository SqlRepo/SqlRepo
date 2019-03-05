using System;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer.ConnectionProviders
{
    public class ConnectionStringConnectionProvider : ISqlConnectionProvider
    {
        private readonly string connectionString;

        public ConnectionStringConnectionProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public TConnection Provide<TConnection>()
            where TConnection: class, IConnection
        {
            return new SqlConnectionAdapter(this.connectionString) as TConnection;
        }
    }
}