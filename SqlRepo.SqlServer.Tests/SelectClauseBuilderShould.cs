using System;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.SqlServer.Abstractions;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class SelectClauseBuilderShould : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            this.builder = new SelectClauseBuilder();
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
            this.builder.Select<TestEntity>(e => e.IntProperty);
            this.builder.IsClean.Should()
                .BeFalse();
        }

        [Test]
        public void DefaultToSelectAllIfNoSelectionsOrAlias()
        {
            this.builder.Sql()
                .Should()
                .Be("SELECT *");
        }

        [Test]
        public void ReturnCorrectSqlForSingleSelectionWithoutAlias()
        {
            const string expected = "SELECT [dbo].[TestEntity].[IntProperty]";
            this.builder.Select<TestEntity>(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleSelectionsWithoutAlias()
        {
            const string expected =
                "SELECT [dbo].[TestEntity].[IntProperty], [dbo].[TestEntity].[IntProperty2]";
            this.builder.Select<TestEntity>(e => e.IntProperty, additionalSelectors: e => e.IntProperty2)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleSelectionsWithDifferentLocalAliases()
        {
            const string expected = "SELECT [a].[IntProperty], [b].[StringProperty]";
            this.builder.Select<TestEntity>(e => e.IntProperty, "a")
                .Select<TestEntity>(e => e.StringProperty, "b")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        [Ignore("Obsolete")]
        public void ReturnCorrectSqlForExplicitSelectionOfAllColumnsForSingleEntity()
        {
            const string expected =
                "SELECT [dbo].[TestEntity].[DateTimeOffsetProperty], [dbo].[TestEntity].[NullableDateTimeOffsetProperty], [dbo].[TestEntity].[DateTimeProperty], [dbo].[TestEntity].[NullableDateTimeProperty], [dbo].[TestEntity].[DoubleProperty], [dbo].[TestEntity].[IntProperty], [dbo].[TestEntity].[IntProperty2], [dbo].[TestEntity].[StringProperty], [dbo].[TestEntity].[TestEnumProperty], [dbo].[TestEntity].[DecimalProperty], [dbo].[TestEntity].[ByteProperty], [dbo].[TestEntity].[ShortProperty], [dbo].[TestEntity].[SingleProperty], [dbo].[TestEntity].[GuidProperty], [dbo].[TestEntity].[Id]";
            this.AssumeTestEntityIsInitialised();
            this.builder.For(this.Entity)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForImplicitSelectionOfAllColumnsWithoutAlias()
        {
            const string expected = "SELECT [dbo].[TestEntity].*";
            this.AssumeTestEntityIsInitialised();
            this.builder.SelectAll<TestEntity>()
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForImplicitSelectionOfAllColumnsWithAlias()
        {
            const string expected = "SELECT [a].*";
            this.AssumeTestEntityIsInitialised();
            this.builder.SelectAll<TestEntity>("a")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForImplicitSelectionOfAllColumnsOfDefaultEntityWithAlias()
        {
            const string expected = "SELECT [a].*";
            this.AssumeTestEntityIsInitialised();
            this.builder.SelectAll<TestEntity>("a")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForTopRequest()
        {
            const string expected = "SELECT TOP 1 *";
            this.AssumeTestEntityIsInitialised();
            this.builder.Top(1)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForAvgWithoutAlias()
        {
            const string expected = "SELECT AVG([dbo].[TestEntity].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Avg<TestEntity>(e => e.DoubleProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForAvgWithAlias()
        {
            const string expected = "SELECT AVG([a].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Avg<TestEntity>(e => e.DoubleProperty, "a")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForAvgWithTableAndSchema()
        {
            const string expected = "SELECT AVG([schema].[table].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Avg<TestEntity>(e => e.DoubleProperty, tableName: "table", tableSchema: "schema")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForCountWithoutAlias()
        {
            const string expected = "SELECT COUNT([dbo].[TestEntity].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Count<TestEntity>(e => e.DoubleProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForCountWithAlias()
        {
            const string expected = "SELECT COUNT([a].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Count<TestEntity>(e => e.DoubleProperty, "a")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForCountWithTableAndSchema()
        {
            const string expected = "SELECT COUNT([schema].[table].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Count<TestEntity>(e => e.DoubleProperty, tableName: "table", tableSchema: "schema")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForCountAll()
        {
            const string expected = "SELECT COUNT(*)";
            this.AssumeTestEntityIsInitialised();
            this.builder.CountAll()
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMaxWithoutAlias()
        {
            const string expected = "SELECT MAX([dbo].[TestEntity].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Max<TestEntity>(e => e.DoubleProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMaxWithAlias()
        {
            const string expected = "SELECT MAX([a].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Max<TestEntity>(e => e.DoubleProperty, "a")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMaxWithTableAndSchema()
        {
            const string expected = "SELECT MAX([schema].[table].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Max<TestEntity>(e => e.DoubleProperty, tableName: "table", tableSchema: "schema")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMinWithoutAlias()
        {
            const string expected = "SELECT MIN([dbo].[TestEntity].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Min<TestEntity>(e => e.DoubleProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMinWithAlias()
        {
            const string expected = "SELECT MIN([a].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Min<TestEntity>(e => e.DoubleProperty, "a")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMinWithTableAndSchema()
        {
            const string expected = "SELECT MIN([schema].[table].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Min<TestEntity>(e => e.DoubleProperty, tableName: "table", tableSchema: "schema")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForSumWithoutAlias()
        {
            const string expected = "SELECT SUM([dbo].[TestEntity].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Sum<TestEntity>(e => e.DoubleProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForSumWithAlias()
        {
            const string expected = "SELECT SUM([a].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Sum<TestEntity>(e => e.DoubleProperty, "a")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForSumWithTableAndSchema()
        {
            const string expected = "SELECT SUM([schema].[table].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Sum<TestEntity>(e => e.DoubleProperty, tableName: "table", tableSchema: "schema")
                .Sql()
                .Should()
                .Be(expected);
        }

        private ISelectClauseBuilder builder;
    }
}