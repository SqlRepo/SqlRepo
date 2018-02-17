using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
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

        public void Open()
        {
            this.connection.Open();
        }
        public ISqlCommand CreateCommand()
        { 
            return new SqlCommandAdapter(this.connection.CreateCommand());
        }

        public Task OpenAsync()
        {
            return this.connection.OpenAsync();
        }

        public void Dispose()
        {
            this.connection.Dispose();
        }
    }
}
