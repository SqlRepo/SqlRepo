using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class DeleteStatementShould : SqlStatementTestBase<DeleteStatement<TestEntity>, int>
    {
        [Test]
        public void BeCleanByDefault()
        {
            this.Statement.IsClean.Should()
                .BeTrue();
        }

        [Test]
        public void BuildCorrectStatementFromEntity()
        {
            this.AssumeWhereClauseBuilderReportsClean();
            this.AssumeTestEntityIsInitialised();
            var expected = $"DELETE [dbo].[TestEntity]\nWHERE [Id] = {this.Entity.Id};";
            this.Statement.For(this.Entity)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void BuildDeleteStatementWithNoWhereClause()
        {
            const string expected = "DELETE [dbo].[TestEntity];";
            this.Statement.Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void DefaultSchemaToDbo()
        {
            this.Statement.TableSchema.Should()
                .Be("dbo");
        }

        [Test]
        public void DefaultTableNameToNameOfType()
        {
            this.Statement.TableName.Should()
                .Be("TestEntity");
        }

        [Test]
        public void EmbedWhereClauseFromBuilderInStatement()
        {
            const string whereClause = "WHERE [Id] = 55";
            const string expected = "DELETE [dbo].[TestEntity]\n" + whereClause + ";";
            this.WhereClauseBuilder.Sql()
                .Returns(whereClause);
            var result = this.Statement.Where(e => e.Id == 55)
                             .Sql();
            result.Should()
                  .Be(expected);
        }

        [Test]
        public void ExecuteQueryOnGo()
        {
            this.AssumeGoIsRequested();
            this.StatementExecutor.Received()
                .ExecuteNonQuery(Arg.Any<string>());
        }

        [Test]
        public async Task ExecuteQueryOnGoAsync()
        {
            await this.AssumeGoAsyncIsRequested();
            await this.StatementExecutor.Received()
                      .ExecuteNonQueryAsync(Arg.Any<string>());
        }

        [Test]
        public void ProduceCorrectNonDefaultTableSpecfication()
        {
            //this.Statement.UsingTableSchema(OtherValue)
            //    .UsingTableName(OtherValue)
            //    .Sql()
            //    .Should()
            //    .Contain(this.ExpectedTableSpecification(OtherValue, OtherValue));
        }

        [Test]
        public void SupportChainingAfterSettingTableSchema()
        {
            this.Statement.UsingTableSchema(OtherValue)
                .Should()
                .Be(this.Statement);
        }

        [Test]
        public void SupportUsingSpecificSchema()
        {
            this.Statement.UsingTableSchema(OtherValue);
            this.Statement.TableSchema.Should()
                .Be(OtherValue);
        }

        [Test]
        public void SupportUsingSpecificTableName()
        {
            this.Statement.UsingTableName(OtherValue);
            this.Statement.TableName.Should()
                .Be(OtherValue);
        }

        [Test]
        public void ThrowExceptionIfForCalledAfterWhere()
        {
            this.AssumeTestEntityIsInitialised();
            this.Statement.Where(e => e.ByteProperty == 1);
            this.Statement.Invoking(s => s.For(this.Entity))
                .Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfWhereCalledAfterFor()
        {
            this.AssumeWhereClauseBuilderReportsClean();
            this.AssumeTestEntityIsInitialised();
            this.Statement.For(this.Entity);
            this.Statement.Invoking(s => s.Where(e => e.ByteProperty == 1))
                .Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void UseBuilderOnSql()
        {
            this.Statement.Where(e => e.Id == 55)
                .Sql();
            this.WhereClauseBuilder.Received()
                .Sql();
        }

        [Test]
        public void UserBuilderOnWhere()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Statement.Where(expression);
            this.WhereClauseBuilder.Received()
                .Where(expression, null, "TestEntity", "dbo");
        }

        [Test]
        public void UserWhereBuilderOnAnd()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Statement.Where(expression)
                .And(expression);
            this.WhereClauseBuilder.Received()
                .And(expression);
        }

        [Test]
        public void UserWhereBuilderOnNestedAnd()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Statement.Where(expression)
                .NestedAnd(expression);
            this.WhereClauseBuilder.Received()
                .NestedAnd(expression);
        }

        [Test]
        public void UserWhereBuilderOnNestedOr()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Statement.Where(expression)
                .NestedOr(expression);
            this.WhereClauseBuilder.Received()
                .NestedOr(expression);
        }

        [Test]
        public void UserWhereBuilderOnOr()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Statement.Where(expression)
                .Or(expression);
            this.WhereClauseBuilder.Received()
                .Or(expression);
        }

        protected override DeleteStatement<TestEntity> CreateStatement(IStatementExecutor statementExecutor,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher,
            ISelectClauseBuilder selectClauseBuilder,
            IFromClauseBuilder fromClauseBuilder,
            IWhereClauseBuilder whereClauseBuilder,
            ISqlConnectionProvider connectionProvider)
        {
            var statement = new DeleteStatement<TestEntity>(statementExecutor, entityMapper, whereClauseBuilder);
            statement.UseConnectionProvider(connectionProvider);
            return statement;
        }

        private async Task AssumeGoAsyncIsRequested()
        {
            await this.Statement.Where(e => e.Id == 1)
                      .GoAsync();
        }

        private void AssumeGoIsRequested()
        {
            this.Statement.Where(e => e.Id == 1)
                .Go();
        }

        private string ExpectedTableSpecification(string schema, string table)
        {
            return $"DELETE [{schema}].[{table}]";
        }
    }
}