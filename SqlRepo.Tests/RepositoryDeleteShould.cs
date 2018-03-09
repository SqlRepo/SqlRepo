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
        public void UseFactoryToCreateStatement()
        {
            this.AssumeDeleteIsRequested();
            this.StatementFactory.Received()
                .CreateDelete<TestEntity>();
        }

        [Test]
        public void ReturnTheStatement()
        {
            this.AssumeDeleteIsRequested()
                .Should()
                .Be(this.DeleteStatement);
        }
    }
}