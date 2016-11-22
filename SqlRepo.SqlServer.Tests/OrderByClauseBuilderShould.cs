using System;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class OrderByClauseBuilderShould: TestBase
    {
        [SetUp]
        public void SetUp()
        {
            this.builder = new OrderByClauseBuilder();
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
        public void ReturnToCleanAfterFromScatch()
        {
            this.builder.By<TestEntity>(e => e.IntProperty)
                .FromScratch();
            this.builder.IsClean.Should()
                .BeTrue();
        }

        [Test]
        public void DefaultToEmptyStringIfNotUsed()
        {
            this.builder.Sql()
                .Should()
                .Be(string.Empty);
        }

        [Test]
        public void SupportSettingTheActiveAlias()
        {
            this.builder.UsingAlias("a")
                .ActiveAlias.Should()
                .Be("a");
        }

        [Test]
        public void ReturnCorrectSqlForSingleOrderByWithoutAlias()
        {
            const string expected = "ORDER BY [dbo].[TestEntity].[IntProperty] ASC";
            this.builder.By<TestEntity>(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithoutAlias()
        {
            const string expected = "ORDER BY [dbo].[TestEntity].[IntProperty] ASC, " +
                                    "[dbo].[TestEntity].[ByteProperty] ASC, " +
                                    "[dbo].[TestEntity].[DecimalProperty] ASC";
            this.builder.By<TestEntity>(e => e.IntProperty, e => e.ByteProperty, e => e.DecimalProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForSingleOrderByWithDefaultAlias()
        {
            const string expected = "ORDER BY [a].[IntProperty] ASC";
            this.builder.UsingAlias("a")
                .By<TestEntity>(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithDefaultAlias()
        {
            const string expected = "ORDER BY [a].[IntProperty] ASC, [a].[ByteProperty] ASC";
            this.builder.UsingAlias("a")
                .By<TestEntity>(e => e.IntProperty, e => e.ByteProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithDifferentAliases()
        {
            const string expected = "ORDER BY [a].[IntProperty] ASC, [b].[StringProperty] ASC";
            this.builder.UsingAlias("a")
                .By<TestEntity>(e => e.IntProperty)
                .UsingAlias("b")
                .By<TestEntity>(e => e.StringProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithLocalAliases()
        {
            const string expected = "ORDER BY [a].[IntProperty] ASC, [a].[StringProperty] ASC";
            this.builder
                .By<TestEntity>("a", e => e.IntProperty, e => e.StringProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithDifferentLocalAliases()
        {
            const string expected = "ORDER BY [a].[IntProperty] ASC, [b].[StringProperty] ASC";
            this.builder
                .By<TestEntity>("a", e => e.IntProperty)
                .By<TestEntity>("b", e => e.StringProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithDifferentDirections()
        {
            const string expected = "ORDER BY [a].[IntProperty] ASC, [b].[StringProperty] DESC";
            this.builder
                .By<TestEntity>("a", e => e.IntProperty)
                .ByDescending<TestEntity>("b", e => e.StringProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithDirection()
        {
            const string expected = "ORDER BY [dbo].[TestEntity].[IntProperty] DESC, " +
                                    "[dbo].[TestEntity].[ByteProperty] DESC, " +
                                    "[dbo].[TestEntity].[DecimalProperty] DESC";
            this.builder.ByDescending<TestEntity>(e => e.IntProperty, e => e.ByteProperty, e => e.DecimalProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithDifferentDirectionsAndNoAlias()
        {
            const string expected = "ORDER BY [dbo].[TestEntity].[IntProperty] ASC, " +
                                    "[dbo].[TestEntity].[ByteProperty] DESC, " +
                                    "[dbo].[TestEntity].[DecimalProperty] ASC";
            this.builder.By<TestEntity>(e => e.IntProperty)
                .ByDescending<TestEntity>(e => e.ByteProperty)
                .By<TestEntity>( e => e.DecimalProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        private IOrderByClauseBuilder builder;
    }
}