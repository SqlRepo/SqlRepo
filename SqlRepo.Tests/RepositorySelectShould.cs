using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class RepositorySelectShould : RepositoryTestBase
    {
        [Test]
        public void UseFactoryToCreateCommandtOnFirstCall()
        {
            this.AssumeSelectIsRequested();
            this.CommandFactory.Received()
                .CreateSelect<TestEntity>();
        }

        [Test]
        public void ReturnTheCommand()
        {
            this.AssumeSelectIsRequested()
                .Should()
                .Be(this.SelectCommand);
        }
    }
}