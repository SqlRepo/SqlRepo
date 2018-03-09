using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.SqlServer.IntegrationTests.Model;

namespace SqlRepo.SqlServer.IntegrationTests.Examples
{
    [TestFixture]
    public class ExecutingAdHocSql : ExampleBase
    {
        [Test]
        public void QuerySql()
        {
            var result = this.RepositoryFactory.Create<Contact>()
                             .ExecuteQuerySql()
                             .WithSql("SELECT * FROM [dbo].[Contact]")
                             .Go()
                             .ToList();
            result.Should()
                  .NotBeNullOrEmpty();
            result.Count.Should()
                  .BeGreaterOrEqualTo(3);
        }

        [Test]
        public void NonQuerySql()
        {
            const int ExpectedRowsAffected = 1;
            var result = this.RepositoryFactory.Create<Contact>()
                             .ExecuteNonQuerySql()
                             .WithSql("UPDATE [dbo].[Contact] SET [LastName] = 'Hanson' WHERE [Id] = 1")
                             .Go();
            result.Should().Be(ExpectedRowsAffected);
        }
    }
}