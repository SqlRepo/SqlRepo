using System;
using System.Data;
using System.Threading.Tasks;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class StatementExecutor : IStatementExecutor
    {
        private const int CommandTimeout = 300000;
        private readonly ISqlConnectionProvider connectionProvider;
        private readonly ISqlLogger logger;

        public StatementExecutor(ISqlLogger logger, ISqlConnectionProvider connectionProvider)
        {
            this.logger = logger;
            this.connectionProvider = connectionProvider;
        }

        public int ExecuteNonQuery(string sql)
        {
            this.LogQuery(sql);
            using(var connection = this.connectionProvider.Provide())
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

        public async Task<int> ExecuteNonQueryAsync(string sql)
        {
            this.LogQuery(sql);
            using(var connection = this.connectionProvider.Provide())
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

        public IDataReader ExecuteReader(string sql)
        {
            this.LogQuery(sql);
            var connection = this.connectionProvider.Provide();
            connection.Open();
            using(var command = connection.CreateCommand())
            {
                command.CommandTimeout = CommandTimeout;
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public async Task<IDataReader> ExecuteReaderAsync(string sql)
        {
            this.LogQuery(sql);
            var connection = this.connectionProvider.Provide();
            await connection.OpenAsync();
            using(var command = connection.CreateCommand())
            {
                command.CommandTimeout = CommandTimeout;
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
        }

        public IDataReader ExecuteStoredProcedure(string name,
            params ParameterDefinition[] parametersDefinitions)
        {
            this.logger.Log($"Executing SP: {name}");
            var connection = this.connectionProvider.Provide();
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

        public async Task<IDataReader> ExecuteStoredProcedureAsync(string name,
            params ParameterDefinition[] parametersDefinitions)
        {
            this.logger.Log($"Executing SP: {name}");
            var connection = this.connectionProvider.Provide();
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