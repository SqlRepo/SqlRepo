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
        public void UseFactoryToCreateCommandtOnFirstCall()
        {
            this.AssumeUpdateEntityIsRequested();
            this.CommandFactory.Received()
                .CreateUpdate<TestEntity>();
        }

        [Test]
        public void UseForOnCommandToGeneratedSql()
        {
            this.AssumeUpdateEntityIsRequested();
            this.UpdateCommand.Received()
                .For(this.Entity);
        }

        [Test]
        public void ExecuteCommandImmediately()
        {
            this.AssumeUpdateEntityIsRequested();
            this.UpdateCommand.Received()
                .Go();
        }
    }
}