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
        public void UseFactoryToCreateCommandtOnFirstCall()
        {
            this.AssumeInsertEntityIsRequested();
            this.CommandFactory.Received()
                .CreateInsert<TestEntity>();
        }

        [Test]
        public void UseForOnCommandToGeneratedSql()
        {
            this.AssumeInsertEntityIsRequested();
            this.InsertCommand.Received()
                .For(this.Entity);
        }

        [Test]
        public void ExecuteCommandImmediately()
        {
            this.AssumeInsertEntityIsRequested();
            this.InsertCommand.Received()
                .Go();
        }
    }
}