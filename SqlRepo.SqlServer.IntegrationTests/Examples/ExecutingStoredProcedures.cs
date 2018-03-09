using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.SqlServer.IntegrationTests.Model;

namespace SqlRepo.SqlServer.IntegrationTests.Examples
{
    [TestFixture]
    public class ExecutingStoredProcedures : ExampleBase
    {
        [Test]
        public void QueryWithoutParameters()
        {
            var result = this.RepositoryFactory.Create<Contact>()
                             .ExecuteQueryProcedure()
                             .WithName("FetchAllContacts")
                             .Go()
                             .ToList();
            result.Should()
                  .NotBeNullOrEmpty();
            result.Count.Should()
                  .BeGreaterOrEqualTo(3);
        }

        [Test]
        public void QueryWithParameters()
        {
            const int ExpectedId = 1;
            var result = this.RepositoryFactory.Create<Contact>()
                             .ExecuteQueryProcedure()
                             .WithName("GetContactById")
                             .WithParameter("@id", ExpectedId)
                             .Go()
                             .ToList();
            result.Should()
                  .NotBeNullOrEmpty();
            result.Count.Should()
                  .BeGreaterOrEqualTo(1);
            result.First()
                  .Id.Should()
                  .Be(ExpectedId);
        }

        [Test]
        public void NonQueryWithoutParameters()
        {
            const int ExpectedRowsAffected = 1;
            var result = this.RepositoryFactory.Create<Contact>()
                             .ExecuteNonQueryProcedure()
                             .WithName("LogStartup")
                             .Go();
            result.Should().Be(ExpectedRowsAffected);
        }

        [Test]
        public void NonQueryWithParameters()
        {
            const int ExpectedRowsAffected = 1;
            var result = this.RepositoryFactory.Create<Contact>()
                             .ExecuteNonQueryProcedure()
                             .WithName("SetLastName")
                             .WithParameter("@id", 1)
                             .WithParameter("@lastName", "Hanson")
                             .Go();
            result.Should()
                  .Be(ExpectedRowsAffected);
        }
    }
}