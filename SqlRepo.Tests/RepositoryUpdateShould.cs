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
        public void UseFactoryToCreateStatement()
        {
            this.AssumeUpdateIsRequested();
            this.StatementFactory.Received()
                .CreateUpdate<TestEntity>();
        }

        [Test]
        public void ReturnTheStatement()
        {
            this.AssumeUpdateIsRequested()
                .Should()
                .Be(this.UpdateStatement);
        }
    }
}