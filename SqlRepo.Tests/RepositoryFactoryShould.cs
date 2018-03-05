using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class RepositoryFactoryShould
    {
        [SetUp]
        public void SetUp()
        {
            this.AssumeStatementFactoryProviderIsInitialised();
            this.factory = new RepositoryFactory(this.statementFactoryProvider);
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

        private IStatementFactoryProvider statementFactoryProvider;
        private IRepositoryFactory factory;
        private IStatementFactory statementFactory;

        private void AssumeStatementFactoryProviderIsInitialised()
        {
            this.statementFactoryProvider = Substitute.For<IStatementFactoryProvider>();
            this.statementFactory = Substitute.For<IStatementFactory>();
            this.statementFactoryProvider.Provide()
                .Returns(this.statementFactory);
        }

        private IRepository<TestEntity> AssumeCreateIsRequested()
        {
            return this.factory.Create<TestEntity>();
        }
    }
}