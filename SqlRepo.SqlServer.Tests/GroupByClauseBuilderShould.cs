using System;
using NUnit.Framework;
using SqlRepo.Testing;
using FluentAssertions;
using System.Linq.Expressions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class GroupByClauseBuilderShould
    {
        [SetUp]
        public void SetUp()
        {
            this.builder = new GroupByClauseBuilder();
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
            this.builder.By<TestEntity>(e => e.IntProperty);
            this.builder.IsClean.Should()
                .BeFalse();
        }

        [Test]
        public void DefaultToEmptyStringIfNotUsed()
        {
            this.builder.Sql()
                .Should()
                .Be(string.Empty);
        }

        [Test]
        public void ReturnCorrectSqlForSingleGroupByWithoutAlias()
        {
            const string expected = "GROUP BY [dbo].[TestEntity].[IntProperty]";
            this.builder.By<TestEntity>(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleGroupByWithoutAlias()
        {
            const string expected = "GROUP BY [dbo].[TestEntity].[IntProperty], " +
                                    "[dbo].[TestEntity].[ByteProperty], " +
                                    "[dbo].[TestEntity].[DecimalProperty]";
            var additionalSelectors = new Expression<Func<TestEntity, object>>[]
                                      {
                                          e => e.ByteProperty,
                                          e => e.DecimalProperty
                                      };
            this.builder.By(e => e.IntProperty, additionalSelectors: additionalSelectors)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForSingleGroupByWithDefaultAlias()
        {
            const string expected = "GROUP BY [dbo].[TestEntity].[IntProperty]";
            this.builder
                .By<TestEntity>(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleGroupByWithDefaultAlias()
        {
            const string expected = "GROUP BY [a].[IntProperty], [a].[ByteProperty]";
            this.builder.By<TestEntity>(e => e.IntProperty, alias: "a", additionalSelectors: e => e.ByteProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleGroupByWithDifferentAliases()
        {
            const string expected = "GROUP BY [a].[IntProperty], [b].[StringProperty]";
            this.builder
                .By<TestEntity>(e => e.IntProperty, "a")
                .By<TestEntity>(e => e.StringProperty, "b")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleGroupByWithLocalAliases()
        {
            const string expected = "GROUP BY [a].[IntProperty], [a].[StringProperty]";
            this.builder
                .By<TestEntity>(e => e.IntProperty, "a", additionalSelectors: e => e.StringProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForGroupByWithHavingCountAll()
        {
            const string expected = "GROUP BY [dbo].[TestEntity].[IntProperty]\nHAVING COUNT(*) > 1";
            this.builder.By<TestEntity>(e => e.IntProperty)
                .HavingCountAll<TestEntity>(Comparison.GreaterThan, 1)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForGroupByWithHavingAvg()
        {
            const string expected = "GROUP BY [dbo].[TestEntity].[IntProperty]\nHAVING AVG([dbo].[TestEntity].[IntProperty]) > 1";
            this.builder.By<TestEntity>(e => e.IntProperty)
                .HavingAvg<TestEntity>(e => e.IntProperty, Comparison.GreaterThan, 1)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForGroupByWithHavingAvgWithAlias()
        {
            const string expected = "GROUP BY [a].[IntProperty]\nHAVING AVG([a].[IntProperty]) > 1";
            this.builder.By<TestEntity>(e => e.IntProperty, "a")
                .HavingAvg<TestEntity>(e => e.IntProperty, Comparison.GreaterThan, 1, "a")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForGroupByWithHavingCount()
        {
            const string expected = "GROUP BY [dbo].[TestEntity].[IntProperty]\nHAVING COUNT([dbo].[TestEntity].[IntProperty]) > 1";
            this.builder.By<TestEntity>(e => e.IntProperty)
                .HavingCount<TestEntity>(e => e.IntProperty, Comparison.GreaterThan, 1)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForGroupByWithHavingCountWithAlias()
        {
            const string expected = "GROUP BY [a].[IntProperty]\nHAVING COUNT([a].[IntProperty]) > 1";
            this.builder.By<TestEntity>(e => e.IntProperty, "a")
                .HavingCount<TestEntity>(e => e.IntProperty, Comparison.GreaterThan, 1, "a")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForGroupByWithHavingMax()
        {
            const string expected = "GROUP BY [dbo].[TestEntity].[IntProperty]\nHAVING MAX([dbo].[TestEntity].[IntProperty]) > 1";
            this.builder.By<TestEntity>(e => e.IntProperty)
                .HavingMax<TestEntity>(e => e.IntProperty, Comparison.GreaterThan, 1)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForGroupByWithHavingMaxWithAlias()
        {
            const string expected = "GROUP BY [a].[IntProperty]\nHAVING MAX([a].[IntProperty]) > 1";
            this.builder.By<TestEntity>(e => e.IntProperty, "a")
                .HavingMax<TestEntity>(e => e.IntProperty, Comparison.GreaterThan, 1, "a")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForGroupByWithHavingMin()
        {
            const string expected = "GROUP BY [dbo].[TestEntity].[IntProperty]\nHAVING MIN([dbo].[TestEntity].[IntProperty]) > 1";
            this.builder.By<TestEntity>(e => e.IntProperty)
                .HavingMin<TestEntity>(e => e.IntProperty, Comparison.GreaterThan, 1)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForGroupByWithHavingMinWithAlias()
        {
            const string expected = "GROUP BY [a].[IntProperty]\nHAVING MIN([a].[IntProperty]) > 1";
            this.builder.By<TestEntity>(e => e.IntProperty, "a")
                .HavingMin<TestEntity>(e => e.IntProperty, Comparison.GreaterThan, 1, "a")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForGroupByWithHavingSum()
        {
            const string expected = "GROUP BY [dbo].[TestEntity].[IntProperty]\nHAVING SUM([dbo].[TestEntity].[IntProperty]) > 1";
            this.builder.By<TestEntity>(e => e.IntProperty)
                .HavingSum<TestEntity>(e => e.IntProperty, Comparison.GreaterThan, 1)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForGroupByWithHavingSumWithAlias()
        {
            const string expected = "GROUP BY [a].[IntProperty]\nHAVING SUM([a].[IntProperty]) > 1";
            this.builder.By<TestEntity>(e => e.IntProperty, "a")
                .HavingSum<TestEntity>(e => e.IntProperty, Comparison.GreaterThan, 1, "a")
                .Sql()
                .Should()
                .Be(expected);
        }

        private IGroupByClauseBuilder builder;
    }
}