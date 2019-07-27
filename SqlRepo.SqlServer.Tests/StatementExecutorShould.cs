using System;
using System.Data;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class StatementExecutorShould
    {
        [SetUp]
        public void SetUp()
        {
            this.AssumeSqlLoggerIsInitialised();
            this.AssumeConnectionProviderIsInitialised();
            this.target = new StatementExecutor(this.logger, this.connectionProvider);
        }

        [Test]
        public void LogQueryWhenExecuteNonQuery()
        {
            this.target.ExecuteNonQuery("sql");
            this.logger.Received()
                .Log($"Executing SQL:{Environment.NewLine}sql");
        }

        [Test]
        public async Task LogQueryWhenExecuteNonQueryAsync()
        {
            await this.target.ExecuteNonQueryAsync("sql");
            this.logger.Received()
                .Log($"Executing SQL:{Environment.NewLine}sql");
        }

        [Test]
        public void LogQueryWhenExecuteReader()
        {
            this.target.ExecuteReader("sql");
            this.logger.Received()
                .Log($"Executing SQL:{Environment.NewLine}sql");
        }

        [Test]
        public async Task LogQueryWhenExecuteReaderAsync()
        {
            await this.target.ExecuteReaderAsync("sql");
            this.logger.Received()
                .Log($"Executing SQL:{Environment.NewLine}sql");
        }

        [Test]
        public void LogQueryWhenExecuteStoredProcedure()
        {
            this.target.ExecuteStoredProcedure("sql");
            this.logger.Received()
                .Log($"Executing SP: sql");
        }

        [Test]
        public async Task LogQueryWhenExecuteStoredProcedureAsync()
        {
            await this.target.ExecuteStoredProcedureAsync("sql");
            this.logger.Received()
                .Log($"Executing SP: sql");
        }

        [Test]
        public void SupportChangingConnectionProvider()
        {
            var connectionProviderOverride = Substitute.For<ISqlConnectionProvider>();
            this.target.UseConnectionProvider(connectionProviderOverride);
            this.target.ExecuteNonQuery("sql");
            this.connectionProvider.DidNotReceive()
                .Provide<ISqlConnection>();
            connectionProviderOverride.Received()
                                      .Provide<ISqlConnection>();
        }

        [Test]
        public void UseTheCommandAdapterToCreateCommandWhenExecuteStoredProcedure()
        {
            var paramDef = new ParameterDefinition[]
                           {
                               new ParameterDefinition
                               {
                                   Name = "name1",
                                   Value = "value1"
                               },
                               new ParameterDefinition
                               {
                                   Name = "name2",
                                   Value = "value2"
                               }
                           };
            this.target.ExecuteStoredProcedure("test", paramDef);
            this.command.Parameters.Received()
                .AddWithValue("name1", "value1");
            this.command.Parameters.Received()
                .AddWithValue("name2", "value2");
            this.AssertConnectionAdapterReceivedCalls();
            this.AssertCommandWasPreparedForStoredProcedure("test");
            this.command.Received()
                .ExecuteReader(CommandBehavior.CloseConnection);
        }

        [Test]
        public async Task UseTheCommandAdapterToCreateCommandWhenExecuteStoredProcedureAsync()
        {
            var paramDef = new[]
                           {
                               new ParameterDefinition
                               {
                                   Name = "name1",
                                   Value = "value1"
                               },
                               new ParameterDefinition
                               {
                                   Name = "name2",
                                   Value = "value2"
                               }
                           };
            await this.target.ExecuteStoredProcedureAsync("test", paramDef);
            this.command.Parameters.Received()
                .AddWithValue("name1", "value1");
            this.command.Parameters.Received()
                .AddWithValue("name2", "value2");
            await this.AssertConnectionAdapterReceivedAsyncCalls();
            this.AssertCommandWasPreparedForStoredProcedure("test");
            await this.command.Received()
                      .ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }

        [Test]
        public void UseTheConnectionAdapterToCreateCommandWhenExecuteExecuteNonQuery()
        {
            this.target.ExecuteNonQuery("test");
            this.AssertConnectionAdapterReceivedCalls();
            this.AssertCommand("test");
            this.command.Received()
                .ExecuteNonQuery();
        }

        [Test]
        public async Task UseTheConnectionAdapterToCreateCommandWhenExecuteNonQueryAsync()
        {
            await this.target.ExecuteNonQueryAsync("test");
            await this.AssertConnectionAdapterReceivedAsyncCalls();
            this.AssertCommand("test");
            await this.command.Received()
                      .ExecuteNonQueryAsync();
        }

        [Test]
        public void UseTheConnectionAdapterToCreateCommandWhenExecuteReader()
        {
            this.target.ExecuteReader("test");
            this.AssertConnectionAdapterReceivedCalls();
            this.AssertCommand("test");
            this.command.Received()
                .ExecuteReader(CommandBehavior.CloseConnection);
        }

        [Test]
        public async Task UseTheConnectionAdapterToCreateCommandWhenExecuteReaderAsync()
        {
            await this.target.ExecuteReaderAsync("test");
            await this.AssertConnectionAdapterReceivedAsyncCalls();
            this.AssertCommand("test");
            await this.command.Received()
                      .ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }

        [Test]
        public void UseTheConnectionProviderToGetConnectionWhenExecuteNonQuery()
        {
            this.target.ExecuteNonQuery("test");
            this.connectionProvider.Received()
                .Provide<ISqlConnection>();
        }

        [Test]
        public async Task UseTheConnectionProviderToGetConnectionWhenExecuteNonQueryAsync()
        {
            await this.target.ExecuteNonQueryAsync("test");
            this.connectionProvider.Received()
                .Provide<ISqlConnection>();
        }

        [Test]
        public void UseTheConnectionProviderToGetConnectionWhenExecuteReader()
        {
            this.target.ExecuteReader("test");
            this.connectionProvider.Received()
                .Provide<ISqlConnection>();
        }

        [Test]
        public async Task UseTheConnectionProviderToGetConnectionWhenExecuteReaderAsync()
        {
            await this.target.ExecuteReaderAsync("test");
            this.connectionProvider.Received()
                .Provide<ISqlConnection>();
        }

        [Test]
        public void UseTheConnectionProviderToGetConnectionWhenExecuteStoredProcedure()
        {
            this.target.ExecuteStoredProcedure("test");
            this.connectionProvider.Received()
                .Provide<ISqlConnection>();
        }

        [Test]
        public async Task UseTheConnectionProviderToGetConnectionWhenExecuteStoredProcedureAsync()
        {
            await this.target.ExecuteStoredProcedureAsync("test");
            this.connectionProvider.Received()
                .Provide<ISqlConnection>();
        }

        private ISqlCommand command;
        private ISqlConnection connection;
        private ISqlConnectionProvider connectionProvider;
        private ISqlLogger logger;
        private IStatementExecutor target;

        private void AssertCommand(string sql)
        {
            this.command.CommandType.Should()
                .BeEquivalentTo(CommandType.Text);
            this.command.CommandTimeout.Should()
                .Be(300000);
            this.command.CommandText.Should()
                .Be(sql);
        }

        private void AssertConnectionAdapterReceivedCalls()
        {
            this.connection.Received()
                .Open();
            this.connection.Received()
                .CreateCommand();
        }

        private async Task AssertConnectionAdapterReceivedAsyncCalls()
        {
            await this.connection.Received()
                .OpenAsync();
            this.connection.Received()
                .CreateCommand();
        }

        private void AssertCommandWasPreparedForStoredProcedure(string sql)
        {
            this.command.CommandType.Should()
                .BeEquivalentTo(CommandType.StoredProcedure);
            this.command.CommandTimeout.Should()
                .Be(300000);
            this.command.CommandText.Should()
                .Be(sql);
        }

        private void AssumeConnectionProviderIsInitialised()
        {
            this.command = Substitute.For<ISqlCommand>();

            this.connection = Substitute.For<ISqlConnection>();
            this.connection.CreateCommand()
                .Returns(this.command);
            this.connectionProvider = Substitute.For<ISqlConnectionProvider>();
            this.connectionProvider.Provide<ISqlConnection>()
                .Returns(this.connection);
        }

        private void AssumeSqlLoggerIsInitialised()
        {
            this.logger = Substitute.For<ISqlLogger>();
        }
    }
}