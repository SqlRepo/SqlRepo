using System;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class RepositoryUpdateEntityShould : RepositoryTestBase
    {
        [Test]
        public void UseFactoryToCreateStatement()
        {
            this.AssumeUpdateEntityIsRequested();
            this.StatementFactory.Received()
                .CreateUpdate<TestEntity>();
        }

        [Test]
        public void UseForOnStatementToGeneratedSql()
        {
            this.AssumeUpdateEntityIsRequested();
            this.UpdateStatement.Received()
                .For(this.Entity);
        }

        [Test]
        public void ExecuteCommandImmediately()
        {
            this.AssumeUpdateEntityIsRequested();
            this.UpdateStatement.Received()
                .Go();
        }
    }
}