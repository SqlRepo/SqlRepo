using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class RepositoryInsertShould : RepositoryTestBase
    {
        [Test]
        public void UseFactoryToCreateCommandtOnFirstCall()
        {
            this.AssumeInsertIsRequested();
            this.CommandFactory.Received()
                .CreateInsert<TestEntity>();
        }

        [Test]
        public void ReturnTheCommand()
        {
            this.AssumeInsertIsRequested()
                .Should()
                .Be(this.InsertCommand);
        }
    }
}