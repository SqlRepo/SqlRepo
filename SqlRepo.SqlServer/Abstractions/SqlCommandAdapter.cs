using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlRepo.SqlServer.Abstractions {
    public class SqlCommandAdapter : ISqlCommand
    {
        private readonly SqlCommand command;

        public SqlCommandAdapter(SqlCommand command, ISqlParameterCollection parameters)
        {
            this.command = command;
            this.Parameters = parameters;
        }

        public SqlCommandAdapter(SqlCommand command)
        {
            this.command = command;
        }


        public void Dispose() { }
        public int CommandTimeout
        {
            get => this.command.CommandTimeout;
            set => this.command.CommandTimeout = value;
        }
        public CommandType CommandType
        {
            get => this.command.CommandType;
            set => this.command.CommandType = value;
        }
        public string CommandText
        {
            get => this.command.CommandText;
            set => this.command.CommandText = value;
        }
        public ISqlParameterCollection Parameters { get; }

        public int ExecuteNonQuery()
        {
            return this.command.ExecuteNonQuery();
        }

        public Task<int> ExecuteNonQueryAsync()
        {
            return this.command.ExecuteNonQueryAsync();
        }

        public IDataReader ExecuteReader(CommandBehavior commandBehavior)
        {
            return this.command.ExecuteReader(commandBehavior);
        }

        public async Task<IDataReader> ExecuteReaderAsync(CommandBehavior commandBehavior)
        {
            return await this.command.ExecuteReaderAsync(commandBehavior);
        }
    }
}
