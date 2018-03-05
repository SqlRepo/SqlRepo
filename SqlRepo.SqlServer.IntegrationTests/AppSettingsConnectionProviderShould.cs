using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;
using SqlRepo.SqlServer.ConnectionProviders;

namespace SqlRepo.SqlServer.IntegrationTests
{
    [TestFixture]
    public class AppSettingsConnectionProviderShould
    {
        [SetUp]
        public void SetUp()
        {
            this.BuildConfiguration();
            this.logger = Substitute.For<ISqlLogger>();
            this.target = new AppSettingsConnectionProvider(this.configuration, "TestingDbConn");
        }

        [Test]
        public void ProvideASqlConnection()
        {
            var connection = this.target.Provide<ISqlConnection>();
            connection.Should()
                      .BeAssignableTo<ISqlConnection>();
        }

        [Test]
        public void ExecuteQueryUsingProvidedConnection()
        {
            var statementExecutor = new StatementExecutor(this.logger, this.target);
            const string Sql = "SELECT * FROM Test";

            var rowsCount = 0;
            using(var reader = statementExecutor.ExecuteReader(Sql))
            {
                while(reader.Read())
                {
                    rowsCount++;
                }
            }

            rowsCount.Should()
                     .Be(4);
        }

        [Test]
        public async Task ExecuteAsyncQueryUsingProvidedConnectionAsync()
        {
            var statementExecutor = new StatementExecutor(this.logger, this.target);
            const string Sql = "SELECT * FROM Test";

            var rowsCount = 0;
            using(var reader = await statementExecutor.ExecuteReaderAsync(Sql))
            {
                while(reader.Read())
                {
                    rowsCount++;
                }
            }

            rowsCount.Should()
                     .Be(4);
        }

        private IConfiguration configuration;
        private ISqlLogger logger;
        private ISqlConnectionProvider target;

        private void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            this.configuration = builder.Build();
        }
    }
}