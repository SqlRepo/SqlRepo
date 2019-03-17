using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMapperDefinitionProviderShould
    {
        [SetUp]
        public void SetUp()
        {
            this.AssumeEntityActivatorFactoryIsInitialised();
            this.target = new EntityMapperDefinitionProvider(this.entityActivatorFactory);
        }

        [Test]
        public void ReturnInstanceOfCorrectType()
        {
            var actual = this.target.Get<TestEntity>();

            actual.Should()
                  .NotBeNull("Actual was null");
            actual.Should().BeOfType<EntityMapperDefinition<TestEntity>>("Actual was not of correct type");
        }

        [Test]
        public void UseFactoryToCreateActivatorForNewDefinition()
        {
            var actual = this.target.Get<TestEntity>();

            this.entityActivatorFactory.Received()
                .Create<TestEntity>();
        }

        [Test]
        public void CacheDefinitions()
        {
            var expected = this.target.Get<TestEntity>();

            var actual = this.target.Get<TestEntity>();

            actual.Should()
                  .Be(expected);
        }

        [Test]
        public void NotUseFactoryToCreateActivatorForCachedDefinition()
        {
            this.target.Get<TestEntity>();
            this.entityActivatorFactory.ClearReceivedCalls();

            this.target.Get<TestEntity>();
            
            this.entityActivatorFactory.DidNotReceive()
                .Create<TestEntity>();
        }

        [Test]
        public void PopulateMembersOfDefinition()
        {
            var propertyCount = typeof(TestEntity).GetProperties()
                                                  .Length;
            var actual = this.target.Get<TestEntity>();

            actual.Activator.Should()
                  .Be(this.activator, "Actual did not have expected activator");

            actual.ColumnTypeMappings.Should()
                  .NotBeEmpty("Column type mappings were not populated");
            actual.ColumnTypeMappings.Count.Should()
                  .Be(propertyCount, "Column type mappings were fewer than expected");

            actual.PropertySetters.Should()
                  .NotBeEmpty("Property setters were not populated");
            actual.PropertySetters.Count.Should()
                  .Be(propertyCount, "Property setters were fewer than expected");
        }

        private void AssumeEntityActivatorFactoryIsInitialised()
        {
            this.entityActivatorFactory = Substitute.For<IEntityActivatorFactory>();
            this.activator = () => new TestEntity();
            this.entityActivatorFactory.Create<TestEntity>()
                .Returns(this.activator);
        }

        private IEntityMapperDefinitionProvider target;
        private IEntityActivatorFactory entityActivatorFactory;
        private EntityActivator<TestEntity> activator;
    }
}