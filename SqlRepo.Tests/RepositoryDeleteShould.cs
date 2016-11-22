using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class RepositoryDeleteShould : RepositoryTestBase
    {
        [Test]
        public void UseFactoryToCreateCommandtOnFirstCall()
        {
            this.AssumeDeleteIsRequested();
            this.CommandFactory.Received()
                .CreateDelete<TestEntity>();
        }

        [Test]
        public void ReturnTheCommand()
        {
            this.AssumeDeleteIsRequested()
                .Should()
                .Be(this.DeleteCommand);
        }
    }
}