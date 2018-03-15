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
            const string Expected = "SELECT [dbo].[TestEntity].[IntProperty]";
            this.builder.Select<TestEntity>(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleSelectionsWithoutAlias()
        {
            const string Expected =
                "SELECT [dbo].[TestEntity].[IntProperty], [dbo].[TestEntity].[IntProperty2]";
            this.builder.Select<TestEntity>(e => e.IntProperty, additionalSelectors: e => e.IntProperty2)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleSelectionsWithDifferentLocalAliases()
        {
            const string Expected = "SELECT [a].[IntProperty], [b].[StringProperty]";
            this.builder.Select<TestEntity>(e => e.IntProperty, "a")
                .Select<TestEntity>(e => e.StringProperty, "b")
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForExplicitSelectionOfAllColumnsForSingleEntity()
        {
            const string Expected =
                "SELECT [dbo].[TestEntity].[BooleanProperty], [dbo].[TestEntity].[BooleanProperty2], [dbo].[TestEntity].[ByteProperty], [dbo].[TestEntity].[DateTimeOffsetProperty], [dbo].[TestEntity].[DateTimeProperty], [dbo].[TestEntity].[DecimalProperty], [dbo].[TestEntity].[DoubleProperty], [dbo].[TestEntity].[GuidProperty], [dbo].[TestEntity].[IntProperty], [dbo].[TestEntity].[IntProperty2], [dbo].[TestEntity].[NullableDateTimeOffsetProperty], [dbo].[TestEntity].[NullableDateTimeProperty], [dbo].[TestEntity].[ShortProperty], [dbo].[TestEntity].[SingleProperty], [dbo].[TestEntity].[StringProperty], [dbo].[TestEntity].[TestEnumProperty], [dbo].[TestEntity].[Id]";
            this.AssumeTestEntityIsInitialised();
            this.builder.For(this.Entity)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForImplicitSelectionOfAllColumnsWithoutAlias()
        {
            const string Expected = "SELECT [dbo].[TestEntity].*";
            this.AssumeTestEntityIsInitialised();
            this.builder.SelectAll<TestEntity>()
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForImplicitSelectionOfAllColumnsWithAlias()
        {
            const string Expected = "SELECT [a].*";
            this.AssumeTestEntityIsInitialised();
            this.builder.SelectAll<TestEntity>("a")
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForImplicitSelectionOfAllColumnsOfDefaultEntityWithAlias()
        {
            const string Expected = "SELECT [a].*";
            this.AssumeTestEntityIsInitialised();
            this.builder.SelectAll<TestEntity>("a")
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForTopRequest()
        {
            const string Expected = "SELECT TOP 1 *";
            this.AssumeTestEntityIsInitialised();
            this.builder.Top(1)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForAvgWithoutAlias()
        {
            const string Expected = "SELECT AVG([dbo].[TestEntity].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Avg<TestEntity>(e => e.DoubleProperty)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForAvgWithAlias()
        {
            const string Expected = "SELECT AVG([a].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Avg<TestEntity>(e => e.DoubleProperty, "a")
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForAvgWithTableAndSchema()
        {
            const string Expected = "SELECT AVG([schema].[table].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Avg<TestEntity>(e => e.DoubleProperty, tableName: "table", tableSchema: "schema")
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForCountWithoutAlias()
        {
            const string Expected = "SELECT COUNT([dbo].[TestEntity].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Count<TestEntity>(e => e.DoubleProperty)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForCountWithAlias()
        {
            const string Expected = "SELECT COUNT([a].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Count<TestEntity>(e => e.DoubleProperty, "a")
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForCountWithTableAndSchema()
        {
            const string Expected = "SELECT COUNT([schema].[table].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Count<TestEntity>(e => e.DoubleProperty, tableName: "table", tableSchema: "schema")
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForCountAll()
        {
            const string Expected = "SELECT COUNT(*)";
            this.AssumeTestEntityIsInitialised();
            this.builder.CountAll()
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForMaxWithoutAlias()
        {
            const string Expected = "SELECT MAX([dbo].[TestEntity].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Max<TestEntity>(e => e.DoubleProperty)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForMaxWithAlias()
        {
            const string Expected = "SELECT MAX([a].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Max<TestEntity>(e => e.DoubleProperty, "a")
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForMaxWithTableAndSchema()
        {
            const string Expected = "SELECT MAX([schema].[table].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Max<TestEntity>(e => e.DoubleProperty, tableName: "table", tableSchema: "schema")
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForMinWithoutAlias()
        {
            const string Expected = "SELECT MIN([dbo].[TestEntity].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Min<TestEntity>(e => e.DoubleProperty)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForMinWithAlias()
        {
            const string Expected = "SELECT MIN([a].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Min<TestEntity>(e => e.DoubleProperty, "a")
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForMinWithTableAndSchema()
        {
            const string Expected = "SELECT MIN([schema].[table].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Min<TestEntity>(e => e.DoubleProperty, tableName: "table", tableSchema: "schema")
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForSumWithoutAlias()
        {
            const string Expected = "SELECT SUM([dbo].[TestEntity].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Sum<TestEntity>(e => e.DoubleProperty)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForSumWithAlias()
        {
            const string Expected = "SELECT SUM([a].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Sum<TestEntity>(e => e.DoubleProperty, "a")
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForSumWithTableAndSchema()
        {
            const string Expected = "SELECT SUM([schema].[table].[DoubleProperty]) AS [DoubleProperty]";
            this.AssumeTestEntityIsInitialised();
            this.builder.Sum<TestEntity>(e => e.DoubleProperty, tableName: "table", tableSchema: "schema")
                .Sql()
                .Should()
                .Be(Expected);
        }

        private ISelectClauseBuilder builder;
    }
}