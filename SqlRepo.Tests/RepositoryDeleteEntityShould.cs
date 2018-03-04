using System;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class RepositoryDeleteEntityShould : RepositoryTestBase
    {
        [Test]
        public void UseFactoryToCreateCommand()
        {
            this.AssumeDeleteEntityIsRequested();
            this.StatementFactory.Received()
                .CreateDelete<TestEntity>();
        }

        [Test]
        public void UseForOnCommandToGeneratedSql()
        {
            this.AssumeDeleteEntityIsRequested();
            this.DeleteStatement.Received()
                .For(this.Entity);
        }

        [Test]
        public void ExecuteCommandImmediately()
        {
            this.AssumeDeleteEntityIsRequested();
            this.DeleteStatement.Received()
                .Go();
        }
    }
}