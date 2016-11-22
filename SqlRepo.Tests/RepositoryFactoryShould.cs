using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class RepositoryFactoryShould
    {
        [SetUp]
        public void SetUp()
        {
            this.AssumeCommandFactoryIsInitialised();
            this.factory = new RepositoryFactory(this.commandFactory);
        }

        [Test]
        public void ReturnRepositoryWithCorrectInterface()
        {
            var result = this.AssumeCreateIsRequested();

            result.Should()
                  .BeAssignableTo<IRepository<TestEntity>>();
        }

        [Test]
        public void ReturnRepositoryOfCorrectConcreteType()
        {
            var result = this.AssumeCreateIsRequested();

            result.Should()
                  .BeAssignableTo<Repository<TestEntity>>();
        }

        private ICommandFactory commandFactory;
        private IRepositoryFactory factory;

        private void AssumeCommandFactoryIsInitialised()
        {
            this.commandFactory = Substitute.For<ICommandFactory>();
        }

        private IRepository<TestEntity> AssumeCreateIsRequested()
        {
            return this.factory.Create<TestEntity>();
        }
    }
}