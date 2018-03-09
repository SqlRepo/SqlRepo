using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class RepositoryExecuteQuerySqlShould : RepositoryTestBase
    {
        [Test]
        public void ReturnTheStatement()
        {
            this.AssumeExecuteQuerySqlIsRequested()
                .Should()
                .Be(this.ExecuteQuerySqlStatement);
        }

        [Test]
        public void UseFactoryToCreateStatement()
        {
            this.AssumeExecuteQuerySqlIsRequested();
            this.StatementFactory.Received()
                .CreateExecuteQuerySql<TestEntity>();
        }
    }
}