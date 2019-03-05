using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class RepositoryExecuteQueryProcedureShould : RepositoryTestBase
    {
        [Test]
        public void UseFactoryToCreateStatement()
        {
            this.AssumeExecuteQueryProcedureIsRequested();
            this.StatementFactory.Received()
                .CreateExecuteQueryProcedure<TestEntity>();
        }

        [Test]
        public void ReturnTheStatement()
        {
            this.AssumeExecuteQueryProcedureIsRequested()
                .Should()
                .Be(this.ExecuteQueryProcedureStatement);
        }
    }
}