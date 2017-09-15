using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlRepo.SqlServer
{
    public class CommandExecutor : ICommandExecutor
    {
        private const int CommandTimeout = 300000;
        private readonly ISqlLogger logger;

        public CommandExecutor(ISqlLogger logger)
        {
            this.logger = logger;
        }

        public int ExecuteNonQuery(string connectionString, string sql)
        {
            this.LogQuery(sql);
            using(var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using(var command = connection.CreateCommand())
                {
                    command.CommandTimeout = CommandTimeout;
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    return command.ExecuteNonQuery();
                }
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string connectionString, string sql)
        {
            this.LogQuery(sql);
            using(var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using(var command = connection.CreateCommand())
                {
                    command.CommandTimeout = CommandTimeout;
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public IDataReader ExecuteReader(string connectionString, string sql)
        {
            this.LogQuery(sql);
            var connection = new SqlConnection(connectionString);
            connection.Open();
            using(var command = connection.CreateCommand())
            {
                command.CommandTimeout = CommandTimeout;
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public async Task<IDataReader> ExecuteReaderAsync(string connectionString, string sql)
        {
            this.LogQuery(sql);
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using(var command = connection.CreateCommand())
            {
                command.CommandTimeout = CommandTimeout;
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
        }

        public IDataReader ExecuteStoredProcedure(string connectionString,
            string name,
            params ParameterDefinition[] parametersDefinitions)
        {
            this.logger.Log($"Executing SP: {name}");
            var connection = new SqlConnection(connectionString);
            connection.Open();
            using(var command = connection.CreateCommand())
            {
                command.CommandTimeout = CommandTimeout;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = name;
                foreach(var parametersDefinition in parametersDefinitions)
                {
                    command.Parameters.AddWithValue(parametersDefinition.Name, parametersDefinition.Value);
                }
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public async Task<IDataReader> ExecuteStoredProcedureAsync(string connectionString,
            string name,
            params ParameterDefinition[] parametersDefinitions)
        {
            this.logger.Log($"Executing SP: {name}");
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using(var command = connection.CreateCommand())
            {
                command.CommandTimeout = CommandTimeout;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = name;
                foreach(var parametersDefinition in parametersDefinitions)
                {
                    command.Parameters.AddWithValue(parametersDefinition.Name, parametersDefinition.Value);
                }
                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
        }

        private void LogQuery(string sql)
        {
            this.logger.Log($"Executing SQL:{Environment.NewLine}{sql}");
        }
    }
}