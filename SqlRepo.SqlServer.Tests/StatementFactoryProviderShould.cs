using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class StatementFactoryProviderShould
    {
        [SetUp]
        public void SetUp()
        {
            this.AssumeEntityMapperIsInitialised();
            this.AssumeWritablePropertyMatcherIsInitialised();
            this.AssumeConnectionProviderIsInitialised();
            this.AssumeSqlLoggerIsInitialised();
            this.target = new StatementFactoryProvider(this.entityMapper,
                this.writablePropertyMatcher,
                this.connectionProvider,
                this.sqlLogger);
        }

        [Test]
        public void ProvideCorrectConcreteType()
        {
            this.target.Provide()
                .Should()
                .BeOfType<StatementFactory>();
        }

        [Test]
        public void ProvideNewInstanceOnEachRequest()
        {
            var factory1 = this.target.Provide();
            var factory2 = this.target.Provide();
            factory1.Should()
                    .NotBeSameAs(factory2);
        }

        [Test]
        public void ProvideTypeWithCorrectInterface()
        {
            this.target.Provide()
                .Should()
                .BeAssignableTo<IStatementFactory>();
        }

        private IConnectionProvider connectionProvider;
        private IEntityMapper entityMapper;
        private ISqlLogger sqlLogger;

        private IStatementFactoryProvider target;
        private IWritablePropertyMatcher writablePropertyMatcher;

        private void AssumeConnectionProviderIsInitialised()
        {
            this.connectionProvider = Substitute.For<IConnectionProvider>();
        }

        private void AssumeEntityMapperIsInitialised()
        {
            this.entityMapper = Substitute.For<IEntityMapper>();
        }

        private void AssumeSqlLoggerIsInitialised()
        {
            this.sqlLogger = Substitute.For<ISqlLogger>();
        }

        private void AssumeWritablePropertyMatcherIsInitialised()
        {
            this.writablePropertyMatcher = Substitute.For<IWritablePropertyMatcher>();
        }
    }
}