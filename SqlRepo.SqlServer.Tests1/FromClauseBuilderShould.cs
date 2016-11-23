using System;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class FromClauseBuilderShould
    {
        [SetUp]
        public void SetUp()
        {
            this.builder = new FromClauseBuilder();
        }

        [Test]
        public void BeCleanByDefault()
        {
            this.builder.IsClean.Should()
                .BeTrue();
        }

        [Test]
        public void BeDirtyAfterInitialised()
        {
            this.builder.From<TestEntity>();
            this.builder.IsClean.Should()
                .BeFalse();
        }

        [Test]
        public void ReturnToCleanAfterFromScatch()
        {
            this.builder.From<TestEntity>()
                .FromScratch();
            this.builder.IsClean.Should()
                .BeTrue();
        }

        [Test]
        public void ReturnCorrectSqlForFromWithNoOverridesOrAlias()
        {
            const string expected = "FROM [dbo].[TestEntity]";
            this.builder.From<TestEntity>()
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForFromWithOverridesAndAlias()
        {
            const string expected = "FROM [Schema].[Table] AS [a]";
            this.builder.From<TestEntity>("a", "Table", "Schema")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectTablesSqlForInnerJoinWithNoOverridesOrAlias()
        {
            const string expected = "FROM [dbo].[TestEntity]\nINNER JOIN [dbo].[InnerEntity]";
            this.builder.From<TestEntity>()
                .InnerJoin<TestEntity, InnerEntity>()
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ReturnCorrectTablesSqlForInnerJoinWithOverridesAndAlias()
        {
            const string expected = "FROM [Schema].[Left] AS [a]\nINNER JOIN [Schema].[Right] AS [b]";
            this.builder.From<TestEntity>("a", "Left", "Schema")
                .InnerJoin<TestEntity, InnerEntity>("a", "b", "Right", "Schema")
                .Sql()
                .Should()
                .StartWith(expected);
        }
        
        [Test]
        public void ReturnCorrectSqlForInnerJoinWithOverridesAndAlias()
        {
            const string expected = "FROM [dbo].[TestEntity]\nINNER JOIN [dbo].[InnerEntity]\nON [dbo].[TestEntity].[Id] = [dbo].[InnerEntity].[TestEntityId]";
            this.builder.From<TestEntity>()
                .InnerJoin<TestEntity, InnerEntity>()
                .On<TestEntity, InnerEntity>((l, r) => l.Id == r.TestEntityId)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ReturnCorrectTablesSqlForLeftOuterJoinWithNoOverridesOrAlias()
        {
            const string expected = "FROM [dbo].[TestEntity]\nLEFT OUTER JOIN [dbo].[InnerEntity]";
            this.builder.From<TestEntity>()
                .LeftOuterJoin<TestEntity, InnerEntity>()
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ReturnCorrectTablesSqlForLeftOuterJoinWithOverridesAndAlias()
        {
            const string expected = "FROM [Schema].[Left] AS [a]\nLEFT OUTER JOIN [Schema].[Right] AS [b]";
            this.builder.From<TestEntity>("a", "Left", "Schema")
                .LeftOuterJoin<TestEntity, InnerEntity>("a", "b", "Right", "Schema")
                .Sql()
                .Should()
                .StartWith(expected);
        }
        
        [Test]
        public void ReturnCorrectSqlForLeftOuterJoinWithOverridesAndAlias()
        {
            const string expected = "FROM [dbo].[TestEntity]\nLEFT OUTER JOIN [dbo].[InnerEntity]\nON [dbo].[TestEntity].[Id] = [dbo].[InnerEntity].[TestEntityId]";
            this.builder.From<TestEntity>()
                .LeftOuterJoin<TestEntity, InnerEntity>()
                .On<TestEntity, InnerEntity>((l, r) => l.Id == r.TestEntityId)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ReturnCorrectTablesSqlForRightOuterJoinWithNoOverridesOrAlias()
        {
            const string expected = "FROM [dbo].[TestEntity]\nRIGHT OUTER JOIN [dbo].[InnerEntity]";
            this.builder.From<TestEntity>()
                .RightOuterJoin<TestEntity, InnerEntity>()
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ReturnCorrectTablesSqlForRightOuterJoinWithOverridesAndAlias()
        {
            const string expected = "FROM [Schema].[Right] AS [a]\nRIGHT OUTER JOIN [Schema].[Right] AS [b]";
            this.builder.From<TestEntity>("a", "Right", "Schema")
                .RightOuterJoin<TestEntity, InnerEntity>("a", "b", "Right", "Schema")
                .Sql()
                .Should()
                .StartWith(expected);
        }
        
        [Test]
        public void ReturnCorrectSqlForRightOuterJoinWithOverridesAndAlias()
        {
            const string expected = "FROM [dbo].[TestEntity]\nRIGHT OUTER JOIN [dbo].[InnerEntity]\nON [dbo].[TestEntity].[Id] = [dbo].[InnerEntity].[TestEntityId]";
            this.builder.From<TestEntity>()
                .RightOuterJoin<TestEntity, InnerEntity>()
                .On<TestEntity, InnerEntity>((l, r) => l.Id == r.TestEntityId)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        private IFromClauseBuilder builder;
    }
}