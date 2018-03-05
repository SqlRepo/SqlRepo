using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class StatementFactoryShould
    {
        [SetUp]
        public void SetUp()
        {
            this.AssumeSqlLoggerIsInitialised();
            this.AssumeConnectionProviderIsInitialised();
            this.AssumeEntityMapperIsInitialised();
            this.AssumeWritableProperyMatcherIsInitialised();
            this.statementFactory = new StatementFactory(this.sqlLogger,
                this.connectionProvider,
                this.entityMapper,
                this.writablePropertyMatcher);
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

        [Test]
        public void ReturnExecuteProcedureOnRequest()
        {
            var result = this.statementFactory.CreateExecute<TestEntity>();
            result.Should()
                  .NotBeNull();
            result.Should()
                  .BeAssignableTo<IExecuteProcedureStatement<TestEntity>>();
            result.Should()
                  .BeAssignableTo<ExecuteProcedureStatement<TestEntity>>();
        }

        [Test]
        public void ReturnExecuteNonQueryProcedureOnRequest()
        {
            var result = this.statementFactory.CreateExecuteNonQuery();
            result.Should()
                  .NotBeNull();
            result.Should()
                  .BeAssignableTo<IExecuteNonQueryProcedureStatement>();
            result.Should()
                  .BeAssignableTo<ExecuteNonQueryProcedureStatementBase>();
        }

        private IConnectionProvider connectionProvider;
        private IEntityMapper entityMapper;
        private ISqlLogger sqlLogger;
        private IStatementFactory statementFactory;
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

        private void AssumeWritableProperyMatcherIsInitialised()
        {
            this.writablePropertyMatcher = Substitute.For<IWritablePropertyMatcher>();
        }
    }
}