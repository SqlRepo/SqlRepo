using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class OrderByClauseBuilderShould : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            builder = new OrderByClauseBuilder();
        }

        private IOrderByClauseBuilder builder;

        [Test]
        public void BeCleanByDefault()
        {
            builder.IsClean.Should()
                .BeTrue();
        }

        [Test]
        public void BeDirtyAfterInitialised()
        {
            builder.By<TestEntity>(e => e.IntProperty);
            builder.IsClean.Should()
                .BeFalse();
        }

        [Test]
        public void DefaultToEmptyStringIfNotUsed()
        {
            builder.Sql()
                .Should()
                .Be(string.Empty);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithDefaultAlias()
        {
            const string expected = "ORDER BY [a].[IntProperty] ASC, [a].[ByteProperty] ASC";
            builder.UsingAlias("a")
                .By<TestEntity>(e => e.IntProperty, e => e.ByteProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithDifferentAliases()
        {
            const string expected = "ORDER BY [a].[IntProperty] ASC, [b].[StringProperty] ASC";
            builder.UsingAlias("a")
                .By<TestEntity>(e => e.IntProperty)
                .UsingAlias("b")
                .By<TestEntity>(e => e.StringProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithDifferentDirections()
        {
            const string expected = "ORDER BY [a].[IntProperty] ASC, [b].[StringProperty] DESC";
            builder.By<TestEntity>("a", "table_name", e => e.IntProperty)
                .ByDescending<TestEntity>("b", e => e.StringProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithDifferentDirectionsAndNoAlias()
        {
            const string expected =
                "ORDER BY [dbo].[TestEntity].[IntProperty] ASC, " + "[dbo].[TestEntity].[ByteProperty] DESC, "
                + "[dbo].[TestEntity].[DecimalProperty] ASC";
            builder.By<TestEntity>(e => e.IntProperty)
                .ByDescending<TestEntity>(e => e.ByteProperty)
                .By<TestEntity>(e => e.DecimalProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithDifferentLocalAliases()
        {
            const string expected = "ORDER BY [a].[IntProperty] ASC, [b].[StringProperty] ASC";
            builder.By<TestEntity>("a", "table_name", e => e.IntProperty)
                .By<TestEntity>("b", "table_name", e => e.StringProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithDirection()
        {
            const string expected =
                "ORDER BY [dbo].[TestEntity].[IntProperty] DESC, "
                + "[dbo].[TestEntity].[ByteProperty] DESC, " + "[dbo].[TestEntity].[DecimalProperty] DESC";
            builder.ByDescending<TestEntity>(e => e.IntProperty,
                e => e.ByteProperty,
                e => e.DecimalProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithLocalAliases()
        {
            const string expected = "ORDER BY [a].[IntProperty] ASC, [a].[StringProperty] ASC";
            builder.By<TestEntity>("a", "table_name", e => e.IntProperty, e => e.StringProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleOrderByWithoutAlias()
        {
            const string expected =
                "ORDER BY [dbo].[TestEntity].[IntProperty] ASC, " + "[dbo].[TestEntity].[ByteProperty] ASC, "
                + "[dbo].[TestEntity].[DecimalProperty] ASC";
            builder.By<TestEntity>(e => e.IntProperty, e => e.ByteProperty, e => e.DecimalProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForSingleOrderByWithDefaultAlias()
        {
            const string expected = "ORDER BY [a].[IntProperty] ASC";
            builder.UsingAlias("a")
                .By<TestEntity>(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForSingleOrderByWithoutAlias()
        {
            const string expected = "ORDER BY [dbo].[TestEntity].[IntProperty] ASC";
            builder.By<TestEntity>(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnToCleanAfterFromScatch()
        {
            builder.By<TestEntity>(e => e.IntProperty)
                .FromScratch();
            builder.IsClean.Should()
                .BeTrue();
        }

        [Test]
        public void SupportSettingTheActiveAlias()
        {
            builder.UsingAlias("a")
                .ActiveAlias.Should()
                .Be("a");
        }
    }
}