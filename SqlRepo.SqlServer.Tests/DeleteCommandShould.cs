using System;
using System.Linq.Expressions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;
using FluentAssertions;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class DeleteCommandShould : SqlCommandTestBase<DeleteCommand<TestEntity>, int>
    {
        protected override DeleteCommand<TestEntity> CreateCommand(ICommandExecutor commandExecutor,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher,
            ISelectClauseBuilder selectClauseBuilder,
            IFromClauseBuilder fromClauseBuilder,
            IWhereClauseBuilder whereClauseBuilder,
            string connectionString)
        {
            var command = new DeleteCommand<TestEntity>(commandExecutor, entityMapper, whereClauseBuilder);
            command.UseConnectionString(connectionString);
            return command;
        }

        private void AssumeGoIsRequested()
        {
            Command.Where(e => e.Id == 1)
                .Go();
        }

        private string ExpectedTableSpecification(string schema, string table)
        {
            return string.Format("DELETE [{0}].[{1}]", schema, table);
        }

        [Test]
        public void BeCleanByDefault()
        {
            Command.IsClean.Should()
                .BeTrue();
        }

        [Test]
        public void BuildCorrectStatementFromEntity()
        {
            AssumeWhereClauseBuilderReportsClean();
            this.AssumeTestEntityIsInitialised();
            var expected = string.Format("DELETE [dbo].[TestEntity]\nWHERE [Id] = {0};", this.Entity.Id);
            Command.For(this.Entity)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void BuildDeleteStatementWithNoWhereClause()
        {
            const string expected = "DELETE [dbo].[TestEntity];";
            Command.Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void DefaultSchemaToDbo()
        {
            Command.TableSchema.Should()
                .Be("dbo");
        }

        [Test]
        public void DefaultTableNameToNameOfType()
        {
            Command.TableName.Should()
                .Be("TestEntity");
        }

        [Test]
        public void EmbedWhereClauseFromBuilderInStatement()
        {
            const string whereClause = "WHERE [Id] = 55";
            const string expected = "DELETE [dbo].[TestEntity]\n" + whereClause + ";";
            WhereClauseBuilder.Sql()
                .Returns(whereClause);
            var result = Command.Where(e => e.Id == 55)
                .Sql();
            result.Should()
                .Be(expected);
        }

        [Test]
        public void ExecuteQueryOnGo()
        {
            AssumeGoIsRequested();
            CommandExecutor.Received()
                .ExecuteNonQuery(ConnectionString, Arg.Any<string>());
        }

        [Test]
        public void ProduceCorrectNonDefaultTableSpecfication()
        {
            //this.Command.UsingTableSchema(OtherValue)
            //    .UsingTableName(OtherValue)
            //    .Sql()
            //    .Should()
            //    .Contain(this.ExpectedTableSpecification(OtherValue, OtherValue));
        }

        [Test]
        public void SupportChainingAfterSettingTableSchema()
        {
            Command.UsingTableSchema(OtherValue)
                .Should()
                .Be(Command);
        }

        [Test]
        public void SupportUsingSpecificSchema()
        {
            Command.UsingTableSchema(OtherValue);
            Command.TableSchema.Should()
                .Be(OtherValue);
        }

        [Test]
        public void SupportUsingSpecificTableName()
        {
            Command.UsingTableName(OtherValue);
            Command.TableName.Should()
                .Be(OtherValue);
        }

        [Test]
        public void ThrowExceptionIfForCalledAfterWhere()
        {
            this.AssumeTestEntityIsInitialised();
            Command.Where(e => e.ByteProperty == 1);
            Command.Invoking(s => s.For(this.Entity))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfWhereCalledAfterFor()
        {
            AssumeWhereClauseBuilderReportsClean();
            this.AssumeTestEntityIsInitialised();
            Command.For(this.Entity);
            Command.Invoking(s => s.Where(e => e.ByteProperty == 1))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void UseBuilderOnSql()
        {
            Command.Where(e => e.Id == 55)
                .Sql();
            WhereClauseBuilder.Received()
                .Sql();
        }

        [Test]
        public void UserBuilderOnWhere()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            Command.Where(expression);
            WhereClauseBuilder.Received()
                .Where(expression);
        }

        [Test]
        public void UserWhereBuilderOnAnd()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            Command.Where(expression)
                .And(expression);
            WhereClauseBuilder.Received()
                .And(expression);
        }

        [Test]
        public void UserWhereBuilderOnNestedAnd()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            Command.Where(expression)
                .NestedAnd(expression);
            WhereClauseBuilder.Received()
                .NestedAnd(expression);
        }

        [Test]
        public void UserWhereBuilderOnNestedOr()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            Command.Where(expression)
                .NestedOr(expression);
            WhereClauseBuilder.Received()
                .NestedOr(expression);
        }

        [Test]
        public void UserWhereBuilderOnOr()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            Command.Where(expression)
                .Or(expression);
            WhereClauseBuilder.Received()
                .Or(expression);
        }
    }
}