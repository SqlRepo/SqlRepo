using System;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityActivatorFactoryShould
    {
        [SetUp]
        public void SetUp()
        {
            this.target = new EntityActivatorFactory();
        }

        private IEntityActivatorFactory target;

        [Test]
        public void CreateInstanceOfCorrectType()
        {
            var actual = this.target.Create<TestEntity>();
            actual.Should()
                  .NotBeNull("Actual was null");
            actual.Should()
                  .BeOfType<EntityActivator<TestEntity>>("Actual was not of correct type");
        }
    }
}