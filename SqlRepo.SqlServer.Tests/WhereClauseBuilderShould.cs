using System;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.SqlServer.Abstractions;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class WhereClauseBuilderShould
    {
        [SetUp]
        public void SetUp()
        {
            this.builder = new WhereClauseBuilder();
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
            this.builder.Where<TestEntity>(e => e.IntProperty == 1);
            this.builder.IsClean.Should()
                .BeFalse();
        }

        [Test]
        public void NotThrowExceptionIfAndIsUsedAfterWhere()
        {
            this.builder.Where<TestEntity>(e => e.IntProperty == 2)
                .Invoking(b => b.And<TestEntity>(e => e.IntProperty == 1))
                .Should()
                .NotThrow<InvalidOperationException>();
        }

        [Test]
        public void NotThrowExceptionIfNestedAndIsUsedAfterWhere()
        {
            this.builder.Where<TestEntity>(e => e.IntProperty == 2)
                .Invoking(b => b.NestedAnd<TestEntity>(e => e.IntProperty == 1))
                .Should()
                .NotThrow<InvalidOperationException>();
        }

        [Test]
        public void NotThrowExceptionIfNestedOrIsUsedAfterWhere()
        {
            this.builder.Where<TestEntity>(e => e.IntProperty == 2)
                .Invoking(b => b.NestedOr<TestEntity>(e => e.IntProperty == 1))
                .Should()
                .NotThrow<InvalidOperationException>();
        }

        [Test]
        public void NotThrowExceptionIfOrIsUsedAfterWhere()
        {
            this.builder.Where<TestEntity>(e => e.IntProperty == 2)
                .Invoking(b => b.Or<TestEntity>(e => e.IntProperty == 1))
                .Should()
                .NotThrow<InvalidOperationException>();
        }

        [Test]
        public void NotThrowForAndedConditionsOnDateTimeProperty()
        {
            this.builder.Where<TestEntity>(e => e.DateTimeProperty > DateTime.UtcNow)
                .And<TestEntity>(e => e.DateTimeProperty < DateTime.UtcNow)
                .Invoking(e => e.Sql())
                .Should()
                .NotThrow();
        }

        [Test]
        public void NotThrowForAndedConditionsOnIntProperty()
        {
            const int IntProperty = 10;
            this.builder.Where<TestEntity>(e => e.IntProperty > 2)
                .And<TestEntity>(e => e.IntProperty < IntProperty)
                .Invoking(e => e.Sql())
                .Should()
                .NotThrow();
        }

        [Test]
        public void NotThrowForAndedConditionsOnStringProperty()
        {
            const string StringProperty = "TestString";
            this.builder.Where<TestEntity>(e => e.StringProperty == "TestString")
                .Or<TestEntity>(e => e.StringProperty == StringProperty)
                .Invoking(e => e.Sql())
                .Should()
                .NotThrow();
        }

        [Test]
        public void ReturnCorrectSqlForAndBetween()
        {
            const string Expected =
                "WHERE ([dbo].[TestEntity].[StringProperty] = 'Something' AND [dbo].[TestEntity].[IntProperty] >= 1 AND [dbo].[TestEntity].[IntProperty] <= 10)";
            this.builder.Where<TestEntity>(e => e.StringProperty == "Something")
                .AndBetween<TestEntity, int>(e => e.IntProperty, 1, 10)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForAndedConditionsOnIntProperty()
        {
            const string Expected =
                "WHERE ([dbo].[TestEntity].[IntProperty] >= 1 AND [dbo].[TestEntity].[IntProperty] <= 5)";
            this.builder.Where<TestEntity>(e => e.IntProperty >= 1)
                .And<TestEntity>(e => e.IntProperty <= 5)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForAndIn()
        {
            const string Expected =
                "WHERE ([dbo].[TestEntity].[StringProperty] = 'Something' AND [dbo].[TestEntity].[StringProperty] IN ('My String', 'My Name'))";
            this.builder.Where<TestEntity>(e => e.StringProperty == "Something")
                .AndIn<TestEntity, string>(e => e.StringProperty, new[] {"My String", "My Name"})
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForBetween()
        {
            const string Expected =
                "WHERE ([dbo].[TestEntity].[IntProperty] >= 1 AND [dbo].[TestEntity].[IntProperty] <= 10)";
            this.builder.WhereBetween<TestEntity, int>(e => e.IntProperty, 1, 10)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultiLevelNesting()
        {
            const string Expected =
                "WHERE ([dbo].[TestEntity].[IntProperty] < 100 AND ([dbo].[TestEntity].[DecimalProperty] >= 5 OR [dbo].[TestEntity].[DecimalProperty] <= 50 AND ([dbo].[TestEntity].[SingleProperty] >= 2 AND [dbo].[TestEntity].[SingleProperty] <= 10)))";
            this.builder.Where<TestEntity>(e => e.IntProperty < 100)
                .NestedAnd<TestEntity>(e => e.DecimalProperty >= 5)
                .Or<TestEntity>(e => e.DecimalProperty <= 50)
                .NestedAnd<TestEntity>(e => e.SingleProperty >= 2)
                .And<TestEntity>(e => e.SingleProperty <= 10)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForMultiLevelNestingWithAliases()
        {
            const string Expected =
                "WHERE ([a].[IntProperty] < 100 AND ([a].[DecimalProperty] >= 5 OR [a].[DecimalProperty] <= 50 AND ([b].[SingleProperty] >= 2 AND [b].[SingleProperty] <= 10)))";
            this.builder.Where<TestEntity>(e => e.IntProperty < 100, "a")
                .NestedAnd<TestEntity>(e => e.DecimalProperty >= 5, "a")
                .Or<TestEntity>(e => e.DecimalProperty <= 50, "a")
                .NestedAnd<TestEntity>(e => e.SingleProperty >= 2, "b")
                .And<TestEntity>(e => e.SingleProperty <= 10, "b")
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForNestedAndWithSingleConditionOnIntProperty()
        {
            const string Expected =
                "WHERE ([dbo].[TestEntity].[IntProperty] < 100 AND ([dbo].[TestEntity].[DecimalProperty] >= 5 OR [dbo].[TestEntity].[DecimalProperty] <= 50))";
            this.builder.Where<TestEntity>(e => e.IntProperty < 100)
                .NestedAnd<TestEntity>(e => e.DecimalProperty >= 5)
                .Or<TestEntity>(e => e.DecimalProperty <= 50)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForNestedOrWithSingleConditionOnIntProperty()
        {
            const string Expected =
                "WHERE ([dbo].[TestEntity].[IntProperty] < 100 OR ([dbo].[TestEntity].[DecimalProperty] >= 5 AND [dbo].[TestEntity].[DecimalProperty] <= 50))";
            this.builder.Where<TestEntity>(e => e.IntProperty < 100)
                .NestedOr<TestEntity>(e => e.DecimalProperty >= 5)
                .And<TestEntity>(e => e.DecimalProperty <= 50)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForOrBetween()
        {
            const string Expected =
                "WHERE ([dbo].[TestEntity].[StringProperty] = 'Something' OR [dbo].[TestEntity].[IntProperty] >= 1 AND [dbo].[TestEntity].[IntProperty] <= 10)";
            this.builder.Where<TestEntity>(e => e.StringProperty == "Something")
                .OrBetween<TestEntity, int>(e => e.IntProperty, 1, 10)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForOredConditionsOnIntProperty()
        {
            const string Expected =
                "WHERE ([dbo].[TestEntity].[IntProperty] <= 1 OR [dbo].[TestEntity].[IntProperty] >= 5)";
            this.builder.Where<TestEntity>(e => e.IntProperty <= 1)
                .Or<TestEntity>(e => e.IntProperty >= 5)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForOrIn()
        {
            const string Expected =
                "WHERE ([dbo].[TestEntity].[StringProperty] = 'Something' OR [dbo].[TestEntity].[StringProperty] IN ('My String', 'My Name'))";
            this.builder.Where<TestEntity>(e => e.StringProperty == "Something")
                .OrIn<TestEntity, string>(e => e.StringProperty, new[] {"My String", "My Name"})
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForSingleEqualityConditionOnStringProperty()
        {
            const string Expected = "WHERE ([dbo].[TestEntity].[StringProperty] = 'My String')";
            this.builder.Where<TestEntity>(e => e.StringProperty == "My String")
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForSingleEqualityConditionOnTestEnumProperty()
        {
            const string Expected = "WHERE ([dbo].[TestEntity].[TestEnumProperty] = 1)";
            this.builder.Where<TestEntity>(e => e.TestEnumProperty == TestEnum.One)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForSingleGreaterThanConditionOnIntProperty()
        {
            const string Expected = "WHERE ([dbo].[TestEntity].[IntProperty] > 1)";
            this.builder.Where<TestEntity>(e => e.IntProperty > 1)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForSingleGreaterThanOrEqualConditionOnDoubleProperty()
        {
            const string Expected = "WHERE ([dbo].[TestEntity].[DoubleProperty] >= 1.01)";
            this.builder.Where<TestEntity>(e => e.DoubleProperty >= 1.01)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForSingleLessThanConditionOnDecimalProperty()
        {
            const string Expected = "WHERE ([dbo].[TestEntity].[DecimalProperty] < 2.02)";
            this.builder.Where<TestEntity>(e => e.DecimalProperty < 2.02M)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForSingleLessThanOrEqualConditionOnSingleProperty()
        {
            const string Expected = "WHERE ([dbo].[TestEntity].[SingleProperty] <= 2.02)";
            this.builder.Where<TestEntity>(e => e.SingleProperty <= 2.02)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForSingleNoneEqualityConditionOnGuidProperty()
        {
            const string Expected = "WHERE ([dbo].[TestEntity].[GuidProperty] <> '{0}')";
            var guid = Guid.NewGuid();
            this.builder.Where<TestEntity>(e => e.GuidProperty != guid)
                .Sql()
                .Should()
                .Be(string.Format(Expected, guid));
        }

        [Test]
        public void ReturnCorrectSqlForStringContains()
        {
            const string Expected = "WHERE ([dbo].[TestEntity].[StringProperty] LIKE '%My%')";
            this.builder.Where<TestEntity>(e => e.StringProperty.Contains("My"))
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForTwoNestedAndsAtRoot()
        {
            const string Expected =
                "WHERE ([dbo].[TestEntity].[IntProperty] < 100 AND ([dbo].[TestEntity].[DecimalProperty] >= 5 OR [dbo].[TestEntity].[DecimalProperty] <= 50) AND ([dbo].[TestEntity].[SingleProperty] >= 2 AND [dbo].[TestEntity].[SingleProperty] <= 10))";
            this.builder.Where<TestEntity>(e => e.IntProperty < 100)
                .NestedAnd<TestEntity>(e => e.DecimalProperty >= 5)
                .Or<TestEntity>(e => e.DecimalProperty <= 50)
                .EndNesting()
                .NestedAnd<TestEntity>(e => e.SingleProperty >= 2)
                .And<TestEntity>(e => e.SingleProperty <= 10)
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnCorrectSqlForWhereIn()
        {
            const string Expected = "WHERE ([dbo].[TestEntity].[StringProperty] IN ('My String', 'My Name'))";
            this.builder.WhereIn<TestEntity, string>(e => e.StringProperty, new[] {"My String", "My Name"})
                .Sql()
                .Should()
                .Be(Expected);
        }

        [Test]
        public void ReturnEmptyStringIfNotInitialised()
        {
            this.builder.Sql()
                .Should()
                .BeEmpty();
        }

        [Test]
        public void StartNewClauseCorrectly()
        {
            this.builder.Where<TestEntity>(e => e.StringProperty == "My String")
                .Sql()
                .Should()
                .StartWith("WHERE (");
        }

        [Test]
        public void TerminateNewClauseCorrectly()
        {
            this.builder.Where<TestEntity>(e => e.StringProperty == "My String")
                .Sql()
                .Should()
                .EndWith(")");
        }

        [Test]
        public void ThrowExceptionIfAndIsUsedBeforeWhere()
        {
            this.builder.Invoking(b => b.And<TestEntity>(e => e.IntProperty == 1))
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfNestedAndIsUsedBeforeWhere()
        {
            this.builder.Invoking(b => b.NestedAnd<TestEntity>(e => e.IntProperty == 1))
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfNestedOrIsUsedBeforeWhere()
        {
            this.builder.Invoking(b => b.NestedOr<TestEntity>(e => e.IntProperty == 1))
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfOrIsUsedBeforeWhere()
        {
            this.builder.Invoking(b => b.Or<TestEntity>(e => e.IntProperty == 1))
                .Should()
                .Throw<InvalidOperationException>();
        }

        private IWhereClauseBuilder builder;
    }
}