using System;
using FluentAssertions;
using NUnit.Framework;
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
        public void ReturnToCleanAfterFromScatch()
        {
            this.builder.Select<TestEntity>(e => e.IntProperty)
                .FromScratch();
            this.builder.IsClean.Should()
                .BeTrue();
        }

        [Test]
        public void DefaultToSelectAllIfNoSelectionsOrAlias()
        {
            this.builder.Sql()
                .Should()
                .Be("SELECT *");
        }

        [Test]
        public void SupportSettingTheActiveAlias()
        {
            this.builder.UsingAlias("a")
                .ActiveAlias.Should()
                .Be("a");
        }

        [Test]
        public void IncludeAliasInDefaultSelectionIfSet()
        {
            this.builder.UsingAlias("a")
                .Sql()
                .Should()
                .Be("SELECT [a].*");
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
            this.builder.Select<TestEntity>(e => e.IntProperty, e => e.IntProperty2)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForSingleSelectionWithSingleAlias()
        {
            const string expected = "SELECT [a].[IntProperty]";
            this.builder.UsingAlias("a")
                .Select<TestEntity>(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleSelectionsWithSingleAlias()
        {
            const string expected = "SELECT [a].[IntProperty], [a].[IntProperty2]";
            this.builder.UsingAlias("a")
                .Select<TestEntity>(e => e.IntProperty, e => e.IntProperty2)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleSelectionsWithDifferentAliases()
        {
            const string expected = "SELECT [a].[IntProperty], [b].[StringProperty]";
            this.builder.UsingAlias("a")
                .Select<TestEntity>(e => e.IntProperty)
                .UsingAlias("b")
                .Select<TestEntity>(e => e.StringProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleSelectionsWithLocalAlias()
        {
            const string expected = "SELECT [a].[IntProperty], [a].[StringProperty]";
            this.builder.Select<TestEntity>("a", e => e.IntProperty, e => e.StringProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultipleSelectionsWithDifferentLocalAliases()
        {
            const string expected = "SELECT [a].[IntProperty], [b].[StringProperty]";
            this.builder
                .Select<TestEntity>("a", e => e.IntProperty)
                .Select<TestEntity>("b", e => e.StringProperty)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
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
        public void ReturnCorrectSqlForExplicitSelectionOfAllColumnsForSingleEntityWithAlias()
        {
            const string expected =
                "SELECT [a].[DateTimeOffsetProperty], [a].[NullableDateTimeOffsetProperty], [a].[DateTimeProperty], [a].[NullableDateTimeProperty], [a].[DoubleProperty], [a].[IntProperty], [a].[IntProperty2], [a].[StringProperty], [a].[TestEnumProperty], [a].[DecimalProperty], [a].[ByteProperty], [a].[ShortProperty], [a].[SingleProperty], [a].[GuidProperty], [a].[Id]";
            this.AssumeTestEntityIsInitialised();
            this.builder.UsingAlias("a")
                .For(this.Entity)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForExplicitSelectionOfAllColumnsForMultipleEntitiesWithAliases()
        {
            const string expected =
                "SELECT [a].[DateTimeOffsetProperty], [a].[NullableDateTimeOffsetProperty], [a].[DateTimeProperty], [a].[NullableDateTimeProperty], [a].[DoubleProperty], [a].[IntProperty], [a].[IntProperty2], [a].[StringProperty], [a].[TestEnumProperty], [a].[DecimalProperty], [a].[ByteProperty], [a].[ShortProperty], [a].[SingleProperty], [a].[GuidProperty], [a].[Id], [b].[DateTimeOffsetProperty], [b].[NullableDateTimeOffsetProperty], [b].[DateTimeProperty], [b].[NullableDateTimeProperty], [b].[DoubleProperty], [b].[IntProperty], [b].[IntProperty2], [b].[StringProperty], [b].[TestEnumProperty], [b].[DecimalProperty], [b].[ByteProperty], [b].[ShortProperty], [b].[SingleProperty], [b].[GuidProperty], [b].[Id]";
            this.AssumeTestEntityIsInitialised();
            this.builder.UsingAlias("a")
                .For(this.Entity)
                .UsingAlias("b")
                .For(this.Entity)
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForImplicitSelectionOfAllColumnsWithoutAlias()
        {
            const string expected =
                "SELECT [dbo].[TestEntity].*";
            this.AssumeTestEntityIsInitialised();
            this.builder.SelectAll<TestEntity>()
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForImplicitSelectionOfAllColumnsWithAlias()
        {
            const string expected =
                "SELECT [a].*";
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

        private ISelectClauseBuilder builder;
    }
}