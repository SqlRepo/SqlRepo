using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlRepo.SqlServer.Abstractions
{
    public class SqlConnectionAdapter : ISqlConnection
    {
        private readonly SqlConnection connection;

        public SqlConnectionAdapter(string connectionString)
        {
            this.connection = new SqlConnection(connectionString);
        }

        public SqlConnectionAdapter()
        {
            this.connection = new SqlConnection();
        }

        public ISqlCommand CreateCommand()
        {
            return new SqlCommandAdapter(this.connection.CreateCommand());
        }

        public void Dispose()
        {
            this.connection.Dispose();
        }

        public void Open()
        {
            this.connection.Open();
        }

        public Task OpenAsync()
        {
            return this.connection.OpenAsync();
        }
    }
}