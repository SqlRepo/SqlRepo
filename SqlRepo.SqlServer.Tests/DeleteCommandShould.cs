using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class DeleteCommandShould : SqlCommandTestBase<DeleteCommand<TestEntity>, int>
    {
        [Test]
        public void BeCleanByDefault()
        {
            this.Command.IsClean.Should()
                .BeTrue();
        }

        [Test]
        public void BuildCorrectStatementFromEntity()
        {
            this.AssumeWhereClauseBuilderReportsClean();
            this.AssumeTestEntityIsInitialised();
            var expected = $"DELETE [dbo].[TestEntity]\nWHERE [Id] = {this.Entity.Id};";
            this.Command.For(this.Entity)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void BuildDeleteStatementWithNoWhereClause()
        {
            const string expected = "DELETE [dbo].[TestEntity];";
            this.Command.Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void DefaultSchemaToDbo()
        {
            this.Command.TableSchema.Should()
                .Be("dbo");
        }

        [Test]
        public void DefaultTableNameToNameOfType()
        {
            this.Command.TableName.Should()
                .Be("TestEntity");
        }

        [Test]
        public void EmbedWhereClauseFromBuilderInStatement()
        {
            const string whereClause = "WHERE [Id] = 55";
            const string expected = "DELETE [dbo].[TestEntity]\n" + whereClause + ";";
            this.WhereClauseBuilder.Sql()
                .Returns(whereClause);
            var result = this.Command.Where(e => e.Id == 55)
                             .Sql();
            result.Should()
                  .Be(expected);
        }

        [Test]
        public void ExecuteQueryOnGo()
        {
            this.AssumeGoIsRequested();
            this.CommandExecutor.Received()
                .ExecuteNonQuery(ConnectionString, Arg.Any<string>());
        }

        [Test]
        public async Task ExecuteQueryOnGoAsync()
        {
            await this.AssumeGoAsyncIsRequested();
            await this.CommandExecutor.Received()
                      .ExecuteNonQueryAsync(ConnectionString, Arg.Any<string>());
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
            this.Command.UsingTableSchema(OtherValue)
                .Should()
                .Be(this.Command);
        }

        [Test]
        public void SupportUsingSpecificSchema()
        {
            this.Command.UsingTableSchema(OtherValue);
            this.Command.TableSchema.Should()
                .Be(OtherValue);
        }

        [Test]
        public void SupportUsingSpecificTableName()
        {
            this.Command.UsingTableName(OtherValue);
            this.Command.TableName.Should()
                .Be(OtherValue);
        }

        [Test]
        public void ThrowExceptionIfForCalledAfterWhere()
        {
            this.AssumeTestEntityIsInitialised();
            this.Command.Where(e => e.ByteProperty == 1);
            this.Command.Invoking(s => s.For(this.Entity))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfWhereCalledAfterFor()
        {
            this.AssumeWhereClauseBuilderReportsClean();
            this.AssumeTestEntityIsInitialised();
            this.Command.For(this.Entity);
            this.Command.Invoking(s => s.Where(e => e.ByteProperty == 1))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void UseBuilderOnSql()
        {
            this.Command.Where(e => e.Id == 55)
                .Sql();
            this.WhereClauseBuilder.Received()
                .Sql();
        }

        [Test]
        public void UserBuilderOnWhere()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Command.Where(expression);
            this.WhereClauseBuilder.Received()
                .Where(expression);
        }

        [Test]
        public void UserWhereBuilderOnAnd()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Command.Where(expression)
                .And(expression);
            this.WhereClauseBuilder.Received()
                .And(expression);
        }

        [Test]
        public void UserWhereBuilderOnNestedAnd()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Command.Where(expression)
                .NestedAnd(expression);
            this.WhereClauseBuilder.Received()
                .NestedAnd(expression);
        }

        [Test]
        public void UserWhereBuilderOnNestedOr()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Command.Where(expression)
                .NestedOr(expression);
            this.WhereClauseBuilder.Received()
                .NestedOr(expression);
        }

        [Test]
        public void UserWhereBuilderOnOr()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Command.Where(expression)
                .Or(expression);
            this.WhereClauseBuilder.Received()
                .Or(expression);
        }

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

        private async Task AssumeGoAsyncIsRequested()
        {
            await this.Command.Where(e => e.Id == 1)
                      .GoAsync();
        }

        private void AssumeGoIsRequested()
        {
            this.Command.Where(e => e.Id == 1)
                .Go();
        }

        private string ExpectedTableSpecification(string schema, string table)
        {
            return $"DELETE [{schema}].[{table}]";
        }
    }
}