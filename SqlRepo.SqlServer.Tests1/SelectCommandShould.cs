using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Extensions;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class SelectCommandShould : SqlCommandTestBase<SelectCommand<TestEntity>, IEnumerable<TestEntity>>
    {
        [Test]
        public void BeCleanByDefault()
        {
            this.Command.IsClean.Should()
                .BeTrue();
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
        public void SupportUsingSpecificSchema()
        {
            this.Command.UsingTableSchema(OtherValue);
            this.Command.TableSchema.Should()
                .Be(OtherValue);
        }

        [Test]
        public void SupportChainingAfterSettingTableSchema()
        {
            this.Command.UsingTableSchema(OtherValue)
                .Should()
                .Be(this.Command);
        }

        [Test]
        public void SupportUsingSpecificTableName()
        {
            this.Command.UsingTableName(OtherValue);
            this.Command.TableName.Should()
                .Be(OtherValue);
        }

        [Test]
        public void SupportChainingAfterSettingTableName()
        {
            this.Command.UsingTableName(OtherValue)
                .Should()
                .Be(this.Command);
        }

        [Test]
        public void UseBuilderOnTop()
        {
            this.Command.Top(1);
            this.SelectClauseBuilder.Received()
                .Top(1);
        }

        [Test]
        public void AlwaysUseBuilderToGetSelectClause()
        {
            this.Command.Sql();
            this.SelectClauseBuilder.Received()
                .Sql();
        }

        [Test]
        public void AlwaysUseBuilderToGetFromClause()
        {
            this.Command.Sql();
            this.FromClauseBuilder.Received()
                .Sql();
        }

        [Test]
        public void GenerateDefaultFromClauseIfNotInitialised()
        {
            this.FromClauseBuilder.IsClean.Returns(true);
            this.Command.Sql();
            this.FromClauseBuilder.Received()
                .From<TestEntity>();
        }

        [Test]
        public void UseBuilderToGetWhereClauseIfInitialised()
        {
            this.WhereClauseBuilder.IsClean.Returns(false);
            this.Command.Sql();
            this.WhereClauseBuilder.Received()
                .Sql();
        }

        [Test]
        public void UseBuilderToGetOrderBClauseIfInitialised()
        {
            this.OrderByClauseBuilder.IsClean.Returns(false);
            this.Command.Sql();
            this.OrderByClauseBuilder.Received()
                .Sql();
        }

        [Test]
        public void ExecuteQueryOnGo()
        {
            this.AssumeGoIsRequested();
            this.CommandExecutor.Received()
                .ExecuteReader(Arg.Any<string>());
        }

        [Test]
        public void MapResultFromExecution()
        {
            this.AssumeGoIsRequested();
            this.EntityMapper.Received()
                .Map<TestEntity>(this.DataReader);
        }

        [Test]
        public void UseBuilderOnWhere()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 1;
            this.Command.Where(expression);
            this.WhereClauseBuilder.Received()
                .Where(expression);
        }

        [Test]
        public void UseBuilderOnWhereIn()
        {
            Expression<Func<TestEntity, int>> expression = e => e.Id;
            var values = new[]
                         {
                             1,
                             2,
                             3
                         };
            this.Command.WhereIn(expression, values);
            this.WhereClauseBuilder.Received()
                .WhereIn(expression, values);
        }

        [Test]
        public void UseBuilderOnAnd()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 1;
            this.Command.And(expression);
            this.WhereClauseBuilder.Received()
                .And(expression);
        }

        [Test]
        public void UseBuilderOnAndIn()
        {
            Expression<Func<TestEntity, int>> expression = e => e.Id;
            var values = new[]
                         {
                             1,
                             2,
                             3
                         };
            this.Command.AndIn(expression, values);
            this.WhereClauseBuilder.Received()
                .AndIn(expression, values);
        }

        [Test]
        public void UseBuilderOnNestedAnd()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 1;
            this.Command.NestedAnd(expression);
            this.WhereClauseBuilder.Received()
                .NestedAnd(expression);
        }

        [Test]
        public void UseBuilderOnOr()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 1;
            this.Command.Or(expression);
            this.WhereClauseBuilder.Received()
                .Or(expression);
        }

        [Test]
        public void UseBuilderOnOrIn()
        {
            Expression<Func<TestEntity, int>> expression = e => e.Id;
            var values = new[]
                         {
                             1,
                             2,
                             3
                         };
            this.Command.OrIn(expression, values);
            this.WhereClauseBuilder.Received()
                .OrIn(expression, values);
        }

        [Test]
        public void UseBuilderOnNestedOr()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 1;
            this.Command.NestedOr(expression);
            this.WhereClauseBuilder.Received()
                .NestedOr(expression);
        }

        [Test]
        public void UseBuilderOnSelectAll()
        {
            this.Command.SelectAll<TestEntity>();
            this.SelectClauseBuilder.Received()
                .SelectAll<TestEntity>();
        }

        [Test]
        public void UseBuilderOnInnerJoin()
        {
            this.Command.InnerJoin<TestEntity, InnerEntity>();
            this.FromClauseBuilder.Received()
                .InnerJoin<TestEntity, InnerEntity>();
        }

        [Test]
        public void InitialiseFromClauseIfNotOnInnerJoin()
        {
            this.FromClauseBuilder.IsClean.Returns(true);
            this.Command.InnerJoin<TestEntity, InnerEntity>();
            this.FromClauseBuilder.Received()
                .From<TestEntity>();
        }

        [Test]
        public void InitialiseFromClauseWithAliasIfNotOnInnerJoin()
        {
            this.FromClauseBuilder.IsClean.Returns(true);
            this.Command.InnerJoin<TestEntity, InnerEntity>("a", "b");
            this.FromClauseBuilder.Received()
                .From<TestEntity>("a");
        }

        [Test]
        public void PassAliasesToBuilderOnInnerJoin()
        {
            this.FromClauseBuilder.IsClean.Returns(true);
            this.Command.InnerJoin<TestEntity, InnerEntity>("a", "b");
            this.FromClauseBuilder.Received()
                .InnerJoin<TestEntity, InnerEntity>("a", "b");
        }

        [Test]
        public void UseBuildOnSettingJoinCondition()
        {
            this.FromClauseBuilder.IsClean.Returns(true);
            this.Command.InnerJoin<TestEntity, InnerEntity>();
            Expression<Func<TestEntity, InnerEntity, bool>> expression = (l, r) => l.Id == r.TestEntityId;
            this.Command.On(expression);
            this.FromClauseBuilder.Received()
                .On(expression);
        }

        [Test]
        public void PassAliasesToBuilderOnSettingJoinCondition()
        {
            this.FromClauseBuilder.IsClean.Returns(true);
            this.Command.InnerJoin<TestEntity, InnerEntity>();
            Expression<Func<TestEntity, InnerEntity, bool>> expression = (l, r) => l.Id == r.TestEntityId;
            this.Command.On(expression, "a", "b");
            this.FromClauseBuilder.Received()
                .On(expression, "a", "b");
        }

        [Test]
        public void UseBuilderOnLeftOuterJoin()
        {
            this.Command.LeftOuterJoin<TestEntity, InnerEntity>();
            this.FromClauseBuilder.Received()
                .LeftOuterJoin<TestEntity, InnerEntity>();
        }

        [Test]
        public void InitialiseFromClauseIfNotOnLeftOuterJoin()
        {
            this.FromClauseBuilder.IsClean.Returns(true);
            this.Command.LeftOuterJoin<TestEntity, InnerEntity>();
            this.FromClauseBuilder.Received()
                .From<TestEntity>();
        }

        [Test]
        public void InitialiseFromClauseWithAliasIfNotOnLeftOuterJoin()
        {
            this.FromClauseBuilder.IsClean.Returns(true);
            this.Command.LeftOuterJoin<TestEntity, InnerEntity>("a", "b");
            this.FromClauseBuilder.Received()
                .From<TestEntity>("a");
        }

        [Test]
        public void PassAliasesToBuilderOnLeftOuterJoin()
        {
            this.FromClauseBuilder.IsClean.Returns(true);
            this.Command.LeftOuterJoin<TestEntity, InnerEntity>("a", "b");
            this.FromClauseBuilder.Received()
                .LeftOuterJoin<TestEntity, InnerEntity>("a", "b");
        }

        [Test]
        public void UseBuilderOnRightOuterJoin()
        {
            this.Command.RightOuterJoin<TestEntity, InnerEntity>();
            this.FromClauseBuilder.Received()
                .RightOuterJoin<TestEntity, InnerEntity>();
        }

        [Test]
        public void InitialiseFromClauseIfNotOnRightOuterJoin()
        {
            this.FromClauseBuilder.IsClean.Returns(true);
            this.Command.RightOuterJoin<TestEntity, InnerEntity>();
            this.FromClauseBuilder.Received()
                .From<TestEntity>();
        }

        [Test]
        public void InitialiseFromClauseWithAliasIfNotOnRightOuterJoin()
        {
            this.FromClauseBuilder.IsClean.Returns(true);
            this.Command.RightOuterJoin<TestEntity, InnerEntity>("a", "b");
            this.FromClauseBuilder.Received()
                .From<TestEntity>("a");
        }

        [Test]
        public void PassAliasesToBuilderOnRightOuterJoin()
        {
            this.FromClauseBuilder.IsClean.Returns(true);
            this.Command.RightOuterJoin<TestEntity, InnerEntity>("a", "b");
            this.FromClauseBuilder.Received()
                .RightOuterJoin<TestEntity, InnerEntity>("a", "b");
        }

        [Test]
        public void UseBuilderOnOrderBy()
        {
            Expression<Func<TestEntity, object>> expression = e => e.IntProperty;
            this.Command.OrderBy(expression);
            this.OrderByClauseBuilder.Received()
                .By(expression);
        }

        [Test]
        public void UseBuilderOnOrderByDescending()
        {
            Expression<Func<TestEntity, object>> expression = e => e.IntProperty;
            this.Command.OrderByDescending(expression);
            this.OrderByClauseBuilder.Received()
                .ByDescending(expression);
        }

        [Test]
        public void CleanClauseBuilders()
        {
            this.Command.FromScratch();
            this.SelectClauseBuilder.Received()
                .FromScratch();
            this.FromClauseBuilder.Received()
                .FromScratch();
            this.WhereClauseBuilder.Received()
                .FromScratch();
            this.OrderByClauseBuilder.Received()
                .FromScratch();
        }

        [Test]
        public void UseBuilderOnEndNesting()
        {
            this.Command.EndNesting();
            this.WhereClauseBuilder.Received()
                .EndNesting();
        }

        protected override SelectCommand<TestEntity> CreateCommand(ICommandExecutor commandExecutor,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher,
            ISelectClauseBuilder selectClauseBuilder,
            IFromClauseBuilder fromClauseBuilder,
            IWhereClauseBuilder whereClauseBuilder,
            string connectionString)
        {
            this.AssumeOrderByClauseBuilderIsInitialised();
            var command = new SelectCommand<TestEntity>(commandExecutor,
                entityMapper,
                selectClauseBuilder,
                fromClauseBuilder,
                whereClauseBuilder,
                this.OrderByClauseBuilder);
            command.UseConnectionString(connectionString);
            return command;
        }

        private void AssumeGoIsRequested()
        {
            this.Command.Go();
        }

        private void AssumeOrderByClauseBuilderIsInitialised()
        {
            this.OrderByClauseBuilder = Substitute.For<IOrderByClauseBuilder>();
            this.OrderByClauseBuilder.ReturnsForAll(this.OrderByClauseBuilder);
        }

        protected IOrderByClauseBuilder OrderByClauseBuilder { get; set; }
    }
}