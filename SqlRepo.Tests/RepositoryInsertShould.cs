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
        public void UseFactoryToCreateStatement()
        {
            this.AssumeInsertIsRequested();
            this.StatementFactory.Received()
                .CreateInsert<TestEntity>();
        }

        [Test]
        public void ReturnTheStatement()
        {
            this.AssumeInsertIsRequested()
                .Should()
                .Be(this.InsertStatement);
        }
    }
}