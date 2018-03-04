using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class RepositoryUpdateShould : RepositoryTestBase
    {
        [Test]
        public void UseFactoryToCreateCommandtOnFirstCall()
        {
            this.AssumeUpdateIsRequested();
            this.StatementFactory.Received()
                .CreateUpdate<TestEntity>();
        }

        [Test]
        public void ReturnTheCommand()
        {
            this.AssumeUpdateIsRequested()
                .Should()
                .Be(this.UpdateStatement);
        }
    }
}