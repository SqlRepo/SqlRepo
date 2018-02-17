using System;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;
using SqlRepo.SqlServer.Abstractions;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class StatmentFactoryShould
    {
        [SetUp]
        public void SetUp()
        {
            AssumeCommandExecutorIsInitialised();
            AssumeEntityMapperIsInitialised();
            AssumeWritableProperyMatcherIsInitialised();
            this.statementFactory = new StatementFactory(this.statementExecutor,
                entityMapper,
                writablePropertyMatcher);
        }

        private IStatementExecutor statementExecutor;
        private IStatementFactory statementFactory;
        private IEntityMapper entityMapper;
        private IWritablePropertyMatcher writablePropertyMatcher;

        private void AssumeCommandExecutorIsInitialised()
        {
            this.statementExecutor = Substitute.For<IStatementExecutor>();
        }

        private void AssumeEntityMapperIsInitialised()
        {
            entityMapper = Substitute.For<IEntityMapper>();
        }

        private void AssumeWritableProperyMatcherIsInitialised()
        {
            writablePropertyMatcher = Substitute.For<IWritablePropertyMatcher>();
        }

        [Test]
        public void NotGenerateExceptionForMissingDependenciesOnRequestForDelete()
        {
            this.statementFactory.Invoking(e => e.CreateDelete<TestEntity>())
                .ShouldNotThrow<ArgumentNullException>();
        }

        [Test]
        public void NotGenerateExceptionForMissingDependenciesOnRequestForInsert()
        {
            this.statementFactory.Invoking(e => e.CreateInsert<TestEntity>())
                .ShouldNotThrow<ArgumentNullException>();
        }

        [Test]
        public void NotGenerateExceptionForMissingDependenciesOnRequestForSelect()
        {
            this.statementFactory.Invoking(e => e.CreateSelect<TestEntity>())
                .ShouldNotThrow<ArgumentNullException>();
        }

        [Test]
        public void NotGenerateExceptionForMissingDependenciesOnRequestForUpdate()
        {
            this.statementFactory.Invoking(e => e.CreateUpdate<TestEntity>())
                .ShouldNotThrow<ArgumentNullException>();
        }

        [Test]
        public void ReturnDeleteOnRequest()
        {
            var result = this.statementFactory.CreateDelete<TestEntity>();
            result.Should()
                .NotBeNull();
            result.Should()
                .BeAssignableTo<ISqlStatement<int>>();
            result.Should()
                .BeAssignableTo<DeleteStatement<TestEntity>>();
        }

        [Test]
        public void ReturnInsertOnRequest()
        {
            var result = this.statementFactory.CreateInsert<TestEntity>();
            result.Should()
                .NotBeNull();
            result.Should()
                .BeAssignableTo<ISqlStatement<TestEntity>>();
            result.Should()
                .BeAssignableTo<InsertStatement<TestEntity>>();
        }

        [Test]
        public void ReturnSelectOnRequest()
        {
            var result = this.statementFactory.CreateSelect<TestEntity>();
            result.Should()
                .NotBeNull();
            result.Should()
                .BeAssignableTo<IClauseBuilder>();
            result.Should()
                .BeAssignableTo<SelectStatement<TestEntity>>();
        }

        [Test]
        public void ReturnUpdateOnRequest()
        {
            var result = this.statementFactory.CreateUpdate<TestEntity>();
            result.Should()
                .NotBeNull();
            result.Should()
                .BeAssignableTo<ISqlStatement<int>>();
            result.Should()
                .BeAssignableTo<UpdateStatement<TestEntity>>();
        }
    }
}