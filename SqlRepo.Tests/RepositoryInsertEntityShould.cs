using System;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class RepositoryInsertEntityShould : RepositoryTestBase
    {
        [Test]
        public void UseFactoryToCreateStatement()
        {
            this.AssumeInsertEntityIsRequested();
            this.StatementFactory.Received()
                .CreateInsert<TestEntity>();
        }

        [Test]
        public void UseForOnStatementToGeneratedSql()
        {
            this.AssumeInsertEntityIsRequested();
            this.InsertStatement.Received()
                .For(this.Entity);
        }

        [Test]
        public void ExecuteCommandImmediately()
        {
            this.AssumeInsertEntityIsRequested();
            this.InsertStatement.Received()
                .Go();
        }
    }
}