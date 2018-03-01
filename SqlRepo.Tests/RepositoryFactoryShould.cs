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
            this.AssumeStatementFactoryIsInitialised();
            this.factory = new RepositoryFactory(this.statementFactory);
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

        private IStatementFactory statementFactory;
        private IRepositoryFactory factory;

        private void AssumeStatementFactoryIsInitialised()
        {
            this.statementFactory = Substitute.For<IStatementFactory>();
        }

        private IRepository<TestEntity> AssumeCreateIsRequested()
        {
            return this.factory.Create<TestEntity>();
        }
    }
}