using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class RepositoryExecuteNonQueryProcedureShould : RepositoryTestBase
    {
        [Test]
        public void ReturnTheStatement()
        {
            this.AssumeExecuteNonQueryProcedureIsRequested()
                .Should()
                .Be(this.ExecuteNonQueryProcedureStatement);
        }

        [Test]
        public void UseFactoryToCreateStatement()
        {
            this.AssumeExecuteNonQueryProcedureIsRequested();
            this.StatementFactory.Received()
                .CreateExecuteNonQueryProcedure();
        }
    }
}