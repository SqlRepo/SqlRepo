using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class RepositoryExecuteNonQuerySqlShould : RepositoryTestBase
    {
        [Test]
        public void ReturnTheStatement()
        {
            this.AssumeExecuteNonQuerySqlIsRequested()
                .Should()
                .Be(this.ExecuteNonQuerySqlStatement);
        }

        [Test]
        public void UseFactoryToCreateStatement()
        {
            this.AssumeExecuteNonQuerySqlIsRequested();
            this.StatementFactory.Received()
                .CreateExecuteNonQuerySql();
        }
    }
}