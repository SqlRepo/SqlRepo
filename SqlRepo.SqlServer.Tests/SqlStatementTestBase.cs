using System;
using System.Data;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.SqlServer.Abstractions;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public abstract class SqlStatementTestBase<TCommand, TResult> : TestBase
        where TCommand: SqlStatement<TestEntity, TResult>
    {
        [SetUp]
        public void SetUp()
        {
            this.AssumeTestEntityIsInitialised();
            this.AssumeCommandExecutorIsInitialised();
            this.AssumeEntityMapperIsInitialised();
            this.AssumeSelectClauseBuilderIsInitialised();
            this.AssumeFromClauseBuilderIsInitialised();
            this.AssumeWhereClauseBuilderIsInitialised();
            this.AssumeWritablePropertyMathcerIsInitialised();
            this.Statement = this.CreateStatement(this.StatementExecutor,
                this.EntityMapper,
                this.WritablePropertyMatcher,
                this.SelectClauseBuilder,
                this.FromClauseBuilder,
                this.WhereClauseBuilder,
                ConnectionString);
        }

        [Test]
        public void ThrowErrorIfNoCommandExecutorProvided()
        {
            this.Invoking(
                    e =>
                        e.CreateStatement(null,
                            this.EntityMapper,
                            this.WritablePropertyMatcher,
                            this.SelectClauseBuilder,
                            this.FromClauseBuilder,
                            this.WhereClauseBuilder,
                            ConnectionString))
                .ShouldThrow<ArgumentException>();
        }

        [Test]
        public void ThrowErrorIfNoEntityMapperProvided()
        {
            this.Invoking(
                    e =>
                        e.CreateStatement(this.StatementExecutor,
                            null,
                            this.WritablePropertyMatcher,
                            this.SelectClauseBuilder,
                            this.FromClauseBuilder,
                            this.WhereClauseBuilder,
                            ConnectionString))
                .ShouldThrow<ArgumentException>();
        }

        protected const string ConnectionString = "MyConnection";

        protected void AssumeFromClauseBuilderIsInitialised()
        {
            this.TableDefinition = new TableDefinition
            {
                TableType = typeof(TestEntity),
                Name = "TestEntity",
                Schema = "dbo"
            };
            this.FromClauseBuilder = Substitute.For<IFromClauseBuilder>();
            this.FromClauseBuilder.From<TestEntity>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .ReturnsForAnyArgs(this.FromClauseBuilder);
            this.FromClauseBuilder.TableDefinition<TestEntity>()
                .Returns(this.TableDefinition);
        }

        protected void AssumeSelectClauseBuilderIsInitialised()
        {
            this.SelectClauseBuilder = Substitute.For<ISelectClauseBuilder>();
            this.SelectClauseBuilder.Select(Arg.Any<Expression<Func<TestEntity, object>>>())
                .Returns(this.SelectClauseBuilder);
            this.SelectClauseBuilder.For(Arg.Any<TestEntity>())
                .Returns(this.SelectClauseBuilder);
        }

        protected void AssumeWhereClauseBuilderIsInitialised()
        {
            this.WhereClauseBuilder = Substitute.For<IWhereClauseBuilder>();
            this.WhereClauseBuilder.Where(Arg.Any<Expression<Func<TestEntity, bool>>>())
                .Returns(this.WhereClauseBuilder);
            this.WhereClauseBuilder.And(Arg.Any<Expression<Func<TestEntity, bool>>>())
                .Returns(this.WhereClauseBuilder);
            this.WhereClauseBuilder.Or(Arg.Any<Expression<Func<TestEntity, bool>>>())
                .Returns(this.WhereClauseBuilder);
            this.WhereClauseBuilder.NestedAnd(Arg.Any<Expression<Func<TestEntity, bool>>>())
                .Returns(this.WhereClauseBuilder);
            this.WhereClauseBuilder.NestedOr(Arg.Any<Expression<Func<TestEntity, bool>>>())
                .Returns(this.WhereClauseBuilder);
            this.WhereClauseBuilder.EndNesting()
                .Returns(this.WhereClauseBuilder);
        }

        protected void AssumeWhereClauseBuilderReportsClean()
        {
            this.WhereClauseBuilder.IsClean.Returns(true);
        }

        protected abstract TCommand CreateStatement(IStatementExecutor statementExecutor,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher,
            ISelectClauseBuilder selectClauseBuilder,
            IFromClauseBuilder fromClauseBuilder,
            IWhereClauseBuilder whereClauseBuilder,
            string connectionString);

        private void AssumeCommandExecutorIsInitialised()
        {
            this.StatementExecutor = Substitute.For<IStatementExecutor>();
            this.DataReader = Substitute.For<IDataReader>();
            this.StatementExecutor.ExecuteReader(Arg.Any<string>())
                .Returns(this.DataReader);
        }

        private void AssumeEntityMapperIsInitialised()
        {
            this.EntityMapper = Substitute.For<IEntityMapper>();
            this.EntityMapper.Map<TestEntity>(Arg.Any<IDataReader>())
                .Returns(new[] { this.Entity });
        }

        private void AssumeWritablePropertyMathcerIsInitialised()
        {
            this.WritablePropertyMatcher = Substitute.For<IWritablePropertyMatcher>();
            this.WritablePropertyMatcher.Test(Arg.Any<Type>())
                .Returns(true);
        }

        public TableDefinition TableDefinition { get; private set; }

        protected TCommand Statement { get; private set; }
        protected IStatementExecutor StatementExecutor { get; private set; }
        protected IDataReader DataReader { get; private set; }
        protected IEntityMapper EntityMapper { get; private set; }
        protected IFromClauseBuilder FromClauseBuilder { get; private set; }
        protected ISelectClauseBuilder SelectClauseBuilder { get; set; }
        protected IWhereClauseBuilder WhereClauseBuilder { get; set; }
        protected IWritablePropertyMatcher WritablePropertyMatcher { get; set; }
    }
}