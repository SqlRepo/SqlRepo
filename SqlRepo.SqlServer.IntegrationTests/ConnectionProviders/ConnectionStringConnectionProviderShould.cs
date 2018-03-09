using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;
using SqlRepo.SqlServer.ConnectionProviders;

namespace SqlRepo.SqlServer.IntegrationTests.ConnectionProviders
{
    [TestFixture]
    public class ConnectionStringConnectionProviderShould
    {
        [SetUp]
        public void SetUp()
        {
            this.logger = Substitute.For<ISqlLogger>();
            this.target = new ConnectionStringConnectionProvider(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\_src\SqlRepo\SqlRepo.SqlServer.IntegrationTests\Testing.mdf; Integrated Security = True; Connect Timeout = 30");
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

        private ISqlLogger logger;
        private ISqlConnectionProvider target;
    }
}