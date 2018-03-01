using System;
using System.Data;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class StatementExecutorShould
    {
        [SetUp]
        public void SetUp()
        {
            AssumeSqlLoggerIsInitialised();
            AssumeConnectionProviderIsInitialised();
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
        public void SupportChangingConnectionProvider()
        {
            var connectionProviderOverride = Substitute.For<ISqlConnectionProvider>();
            this.target.UseConnectionProvider(connectionProviderOverride);
            this.target.ExecuteNonQuery("sql");
            this.connectionProvider.DidNotReceive().Provide<ISqlConnection>();
            connectionProviderOverride.Received().Provide<ISqlConnection>();
        }

        [Test]
        public void UseTheConnectionProviderToGetConnectionWhenExecuteExecuteNonQuery()
        {
            this.target.ExecuteNonQuery(Arg.Any<string>());
            this.connectionProvider.Received()
                .Provide<ISqlConnection>();
        }

        [Test]
        public void UseTheConnectionAdaptorToCreateCommandWhenExecuteExecuteNonQuery()
        {
            this.target.ExecuteNonQuery("test");
            this.AssertConnectionAdaptor();
            this.AssertCommand("test");
            this.command.Received()
                .ExecuteNonQuery();
        }

        [Test]
        public void LogQueryWhenExecuteNonQueryAsync()
        {
            this.target.ExecuteNonQueryAsync("sql");
            this.logger.Received()
                .Log($"Executing SQL:{Environment.NewLine}sql");
        }

        [Test]
        public void UseTheConnectionProviderToGetConnectionWhenExecuteNonQueryAsync()
        {
            this.target.ExecuteNonQueryAsync(Arg.Any<string>());
            this.connectionProvider.Received()
                .Provide<ISqlConnection>();
        }

        [Test]
        public void UseTheConnectionAdaptorToCreateCommandWhenExecuteNonQueryAsync()
        {
            this.target.ExecuteNonQueryAsync("test");
            this.AssertConnectionAdaptorAsync();
            this.AssertCommand("test");
            this.command.Received()
                .ExecuteNonQueryAsync();
        }

        [Test]
        public void LogQueryWhenExecuteReader()
        {
            this.target.ExecuteReader("sql");
            this.logger.Received()
                .Log($"Executing SQL:{Environment.NewLine}sql");
        }

        [Test]
        public void UseTheConnectionProviderToGetConnectionWhenExecuteReader()
        {
            this.target.ExecuteReader(Arg.Any<string>());
            this.connectionProvider.Received()
                .Provide<ISqlConnection>();
        }

        [Test]
        public void UseTheConnectionAdaptorToCreateCommandWhenExecuteReader()
        {
            this.target.ExecuteReader("test");
            this.AssertConnectionAdaptor();
            this.AssertCommand("test");
            this.command.Received()
                .ExecuteReader(CommandBehavior.CloseConnection);
        }

        [Test]
        public void LogQueryWhenExecuteReaderAsync()
        {
            this.target.ExecuteReaderAsync("sql");
            this.logger.Received()
                .Log($"Executing SQL:{Environment.NewLine}sql");
        }

        [Test]
        public void UseTheConnectionProviderToGetConnectionWhenExecuteReaderAsync()
        {
            this.target.ExecuteReaderAsync(Arg.Any<string>());
            this.connectionProvider.Received()
                .Provide<ISqlConnection>();
        }

        [Test]
        public void UseTheConnectionAdaptorToCreateCommandWhenExecuteReaderAsync()
        {
            this.target.ExecuteReaderAsync("test");
            this.AssertConnectionAdaptorAsync();
            this.AssertCommand("test");
            this.command.Received()
                .ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }

        [Test]
        public void LogQueryWhenExecuteStoredProcedure()
        {
            this.target.ExecuteStoredProcedure("sql");
            this.logger.Received()
                .Log($"Executing SP: sql");
        }

        [Test]
        public void UseTheConnectionProviderToGetConnectionWhenExecuteStoredProcedure()
        {
            this.target.ExecuteStoredProcedure(Arg.Any<string>());
            this.connectionProvider.Received()
                .Provide<ISqlConnection>();
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

        [Test]
        public void UseTheCommandAdaptorToCreateCommandWhenExecuteStoredProcedure()
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
            this.command.Parameters.Received().AddWithValue("name1", "value1");
            this.command.Parameters.Received().AddWithValue("name2", "value2");
            this.AssertConnectionAdaptor();
            this.AssertStoredProcedure("test");
            this.command.Received()
                .ExecuteReader(CommandBehavior.CloseConnection);
        }

        [Test]
        public void LogQueryWhenExecuteStoredProcedureAsync()
        {
            this.target.ExecuteStoredProcedureAsync("sql");
            this.logger.Received()
                .Log($"Executing SP: sql");
        }

        [Test]
        public void UseTheConnectionProviderToGetConnectionWhenExecuteStoredProcedureAsync()
        {
            this.target.ExecuteStoredProcedureAsync(Arg.Any<string>());
            this.connectionProvider.Received()
                .Provide<ISqlConnection>();
        }

        [Test]
        public void UseTheCommandAdaptorToCreateCommandWhenExecuteStoredProcedureAsync()
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
            this.target.ExecuteStoredProcedureAsync("test", paramDef);
            this.command.Parameters.Received()
                .AddWithValue("name1", "value1");
            this.command.Parameters.Received()
                .AddWithValue("name2", "value2");
            this.AssertConnectionAdaptorAsync();
            this.AssertStoredProcedure("test");
            this.command.Received()
                .ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }










        private ISqlCommand command;
        private ISqlConnection connection;

        private ISqlConnectionProvider connectionProvider;
        private ISqlLogger logger;

        private IStatementExecutor target;

        private void AssertConnectionAdaptor()
        {
            this.connection.Received()
                .Open();
            this.connection.Received()
                .CreateCommand();
           
        }

        private void AssertCommand(string sql)
        {
            this.command.CommandType.ShouldBeEquivalentTo(CommandType.Text);
            this.command.CommandTimeout.Should()
                .Be(300000);
            this.command.CommandText.Should()
                .Be(sql);
        }
        


        private void AssertStoredProcedure(string sql)
        {
            this.command.CommandType.ShouldBeEquivalentTo(CommandType.StoredProcedure);
            this.command.CommandTimeout.Should()
                .Be(300000);
            this.command.CommandText.Should()
                .Be(sql);
        }

        private void AssertConnectionAdaptorAsync()
        {
            this.connection.Received()
                .OpenAsync();
            this.connection.Received()
                .CreateCommand();
        }
    }
}