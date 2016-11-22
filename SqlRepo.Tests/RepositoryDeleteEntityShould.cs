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
            this.CommandFactory.Received()
                .CreateDelete<TestEntity>();
        }

        [Test]
        public void UseForOnCommandToGeneratedSql()
        {
            this.AssumeDeleteEntityIsRequested();
            this.DeleteCommand.Received()
                .For(this.Entity);
        }

        [Test]
        public void ExecuteCommandImmediately()
        {
            this.AssumeDeleteEntityIsRequested();
            this.DeleteCommand.Received()
                .Go();
        }
    }
}