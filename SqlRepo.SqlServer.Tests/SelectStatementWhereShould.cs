using System;
using NUnit.Framework;
using FluentAssertions;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class SelectStatementWhereShould : SelectStatementTestBase
    {
        [Test]
        public void ThrowErrorIfTableNotSpecifiedOnWhere()
        {
            const string ExpectedMessage =
                "A table specification for the entity type and alias must be set using From or one of the Join methods before filtering, sorting or grouping can be applied.";
            this.Statement.Invoking(c => c.Where(e => e.Id == 1, "a"))
                .Should().Throw<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
            this.Statement.Invoking(c => c.Where<InnerEntity>(e => e.Id == 1, "a"))
                .Should().Throw<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
        }

        [Test]
        public void AddSpecificationOnWhere()
        {
            this.Statement.Where(e => e.Id == 1);
            this.Statement.Specification.Filters.Should()
                .NotBeNullOrEmpty();
            this.AssertFilterGroupIsValid(0, FilterGroupType.Where);
            this.AssertConditionIsValid<TestEntity>(0, 0, IdPropertyName, "=", "1");
        }

        [Test]
        public void ThrowErrorIfAndIsCalledBeforeWhere()
        {
            this.Statement.Invoking(c => c.And(e => e.IntProperty == 1))
                .Should().Throw<InvalidOperationException>()
                .WithMessage(
                    "Filtering has not been initialised, please use a Where method before any And or Or method.");
        }

        [Test]
        public void AddSpecificationOnAnd()
        {
            this.Statement.Where(e => e.ByteProperty == 1)
                .And(e => e.Id == 1);
            this.AssertConditionIsValid<TestEntity>(0,
                1,
                IdPropertyName,
                "=",
                "1",
                expectedLogicalOperator: LogicalOperator.And);
        }

        [Test]
        public void AddSpecificationOnOr()
        {
            this.Statement.Where(e => e.ByteProperty == 1)
                .Or(e => e.Id == 1);
            this.AssertConditionIsValid<TestEntity>(0,
                1,
                IdPropertyName,
                "=",
                "1",
                expectedLogicalOperator: LogicalOperator.Or);
        }

        [Test]
        public void AddSpecificationOnWhereIn()
        {
            var values = new[] {1, 2, 3};
            const string ExpectedValue = "(1, 2, 3)";
            this.Statement.WhereIn(e => e.Id, values);
            this.AssertFilterGroupIsValid(0, FilterGroupType.Where);
            this.AssertConditionIsValid<TestEntity>(0, 0, IdPropertyName, "IN", ExpectedValue);
        }

        [Test]
        public void AddSpecifciationOnAndIn()
        {
            var values = new[] {1, 2, 3};
            const string ExpectedValue = "(1, 2, 3)";
            this.Statement.Where(e => e.IntProperty == 1)
                .AndIn(e => e.Id, values);
            this.AssertConditionIsValid<TestEntity>(0,
                1,
                IdPropertyName,
                "IN",
                ExpectedValue,
                expectedLogicalOperator: LogicalOperator.And);
        }

        [Test]
        public void AddSpecifciationOnOrIn()
        {
            var values = new[] {1, 2, 3};
            const string ExpectedValue = "(1, 2, 3)";
            this.Statement.Where(e => e.IntProperty == 1)
                .OrIn(e => e.Id, values);
            this.AssertConditionIsValid<TestEntity>(0,
                1,
                IdPropertyName,
                "IN",
                ExpectedValue,
                expectedLogicalOperator: LogicalOperator.Or);
        }

        [Test]
        public void AddSpecificationOnWhereBetween()
        {
            const int start = 1;
            const int end = 10;
            this.Statement.WhereBetween(e => e.Id, start, end);
            this.AssertFilterGroupIsValid(0, FilterGroupType.Where);
            this.AssertConditionIsValid<TestEntity>(0, 0, IdPropertyName, ">=", start.ToString());
            this.AssertConditionIsValid<TestEntity>(0,
                1,
                IdPropertyName,
                "<=",
                end.ToString(),
                expectedLogicalOperator: LogicalOperator.And);
        }

        [Test]
        public void AddSpecificaationOnAndBetween()
        {
            const int start = 1;
            const int end = 10;
            this.Statement.Where(e => e.IntProperty == 1)
                .AndBetween(e => e.Id, start, end);
            this.AssertConditionIsValid<TestEntity>(0,
                1,
                IdPropertyName,
                ">=",
                start.ToString(),
                expectedLogicalOperator: LogicalOperator.And);
            this.AssertConditionIsValid<TestEntity>(0,
                2,
                IdPropertyName,
                "<=",
                end.ToString(),
                expectedLogicalOperator: LogicalOperator.And);
        }

        [Test]
        public void AddSpecificaationOnOrBetween()
        {
            const int start = 1;
            const int end = 10;
            this.Statement.Where(e => e.IntProperty == 1)
                .OrBetween(e => e.Id, start, end);
            this.AssertConditionIsValid<TestEntity>(0,
                1,
                IdPropertyName,
                ">=",
                start.ToString(),
                expectedLogicalOperator: LogicalOperator.Or);
            this.AssertConditionIsValid<TestEntity>(0,
                2,
                IdPropertyName,
                "<=",
                end.ToString(),
                expectedLogicalOperator: LogicalOperator.And);
        }

        [Test]
        public void ThrowErrorIfNestingAttemptedWithoutWhere()
        {
            const string ExpectedMessage = "Filtering has not been initialised, please use a Where method before any And or Or method.";
            this.Statement.Invoking(e => e.NestedAnd(e1 => e1.Id == 1)).Should().Throw<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
            this.Statement.Invoking(e => e.NestedOr(e1 => e1.Id == 1)).Should().Throw<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
             this.Statement.Invoking(e => e.NestedAndIn(e1 => e1.Id, new [] {1, 2, 3})).Should().Throw<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
             this.Statement.Invoking(e => e.NestedOrIn(e1 => e1.Id, new [] {1, 2, 3})).Should().Throw<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
             this.Statement.Invoking(e => e.NestedAndBetween(e1 => e1.Id, 5, 10)).Should().Throw<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
             this.Statement.Invoking(e => e.NestedOrBetween(e1 => e1.Id, 5, 10)).Should().Throw<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
        }

        [Test]
        public void AddNewFilterGroupAndConditionOnNestedAnd()
        {
            this.Statement.Where(e => e.Id == 1)
                .NestedAnd(e => e.IntProperty > 1);
            var parent = this.Statement.Specification.Filters[0];
            this.AssertNestedFilterGroupIsValid(parent, 0, FilterGroupType.And);
            this.AssertNestedConditionIsValid(parent, 0, 0, IntPropertyName, ">", "1");
        }

        [Test]
        public void AddNewFilterGroupAndConditionOnNestedAndIn()
        {
            this.Statement.Where(e => e.Id == 1)
                .NestedAndIn(e => e.IntProperty, new []{1, 2, 3});
            var parent = this.Statement.Specification.Filters[0];
            this.AssertNestedFilterGroupIsValid(parent, 0, FilterGroupType.And);
            this.AssertNestedConditionIsValid(parent, 0, 0, IntPropertyName, "IN", "(1, 2, 3)");
        }

        [Test]
        public void AddNewFilterGroupAndConditionOnNestedAndBetween()
        {
            this.Statement.Where(e => e.Id == 1)
                .NestedAndBetween(e => e.IntProperty, 5, 10);
            var parent = this.Statement.Specification.Filters[0];
            this.AssertNestedFilterGroupIsValid(parent, 0, FilterGroupType.And);
            this.AssertNestedConditionIsValid(parent, 0, 0, IntPropertyName, ">=", "5");
            this.AssertNestedConditionIsValid(parent, 0, 1, IntPropertyName, "<=", "10", expectedLogicalOperator: LogicalOperator.And);
        }

        [Test]
        public void AddNewFilterGroupAndConditionOnNestedOr()
        {
            this.Statement.Where(e => e.Id == 1)
                .NestedOr(e => e.IntProperty > 1);
            var parent = this.Statement.Specification.Filters[0];
            this.AssertNestedFilterGroupIsValid(parent, 0, FilterGroupType.Or);
            this.AssertNestedConditionIsValid(parent, 0, 0, IntPropertyName, ">", "1");
        }

        [Test]
        public void AddNewFilterGroupAndConditionOnNestedOrIn()
        {
            this.Statement.Where(e => e.Id == 1)
                .NestedOrIn(e => e.IntProperty, new []{1, 2, 3});
            var parent = this.Statement.Specification.Filters[0];
            this.AssertNestedFilterGroupIsValid(parent, 0, FilterGroupType.Or);
            this.AssertNestedConditionIsValid(parent, 0, 0, IntPropertyName, "IN", "(1, 2, 3)");
        }

        [Test]
        public void AddNewFilterGroupAndConditionOnNestedOrBetween()
        {
            this.Statement.Where(e => e.Id == 1)
                .NestedOrBetween(e => e.IntProperty, 5, 10);
            var parent = this.Statement.Specification.Filters[0];
            this.AssertNestedFilterGroupIsValid(parent, 0, FilterGroupType.Or);
            this.AssertNestedConditionIsValid(parent, 0, 0, IntPropertyName, ">=", "5");
            this.AssertNestedConditionIsValid(parent, 0, 1, IntPropertyName, "<=", "10", expectedLogicalOperator: LogicalOperator.And);
        }

        [Test]
        public void ReturnToUsingParentOnEndNesting()
        {
            this.Statement.Where(e => e.Id == 1)
                .NestedOrBetween(e => e.IntProperty, 5, 10)
                .EndNesting()
                .And(e => e.StringProperty == "something");
            this.AssertConditionIsValid<TestEntity>(0, 1, "StringProperty", "=", "'something'", expectedLogicalOperator: LogicalOperator.And);
        }

        private void AssertConditionIsValid<T>(int filterIndex,
            int conditionIndex,
            string expectedIdentifier,
            string expectedOperator,
            string expectedValue,
            string expectedAlias = null,
            LogicalOperator expectedLogicalOperator = LogicalOperator.NotSet)
        {
            var condition = this.Statement.Specification.Filters[filterIndex].Conditions[conditionIndex];
            condition.Alias.Should()
                     .Be(expectedAlias);
            condition.Left.Should()
                     .Be(expectedIdentifier);
            condition.Operator.Should()
                     .Be(expectedOperator);
            condition.LocigalOperator.Should()
                     .Be(expectedLogicalOperator);
            condition.Right.Should()
                     .Be(expectedValue);
            condition.EntityType.Should()
                     .Be(typeof(T));
        }

        private void AssertNestedConditionIsValid(FilterGroup parent, int groupIndex,
            int conditionIndex,
            string expectedIdentifier,
            string expectedOperator,
            string expectedValue,
            string expectedAlias = null,
            LogicalOperator expectedLogicalOperator = LogicalOperator.NotSet)
        {
            var condition = parent.Groups[groupIndex].Conditions[conditionIndex];
            condition.Alias.Should()
                     .Be(expectedAlias);
            condition.Left.Should()
                     .Be(expectedIdentifier);
            condition.Operator.Should()
                     .Be(expectedOperator);
            condition.LocigalOperator.Should()
                     .Be(expectedLogicalOperator);
            condition.Right.Should()
                     .Be(expectedValue);
        }

        private void AssertFilterGroupIsValid(int index,
            FilterGroupType expectedGroupType)
        {
            var specification = this.Statement.Specification.Filters[index];
            specification.GroupType.Should()
                         .Be(expectedGroupType);
            specification.Parent.Should()
                         .Be(null);
            specification.Conditions.Should()
                         .NotBeNullOrEmpty();
        }

        private void AssertNestedFilterGroupIsValid(FilterGroup parent,
            int index,
            FilterGroupType expectedGroupType)
        {
            parent.Groups.Should().NotBeNullOrEmpty();
            var specification = parent.Groups[index];
            specification.GroupType.Should()
                         .Be(expectedGroupType);
            specification.Parent.Should()
                         .Be(parent);
            specification.Conditions.Should()
                         .NotBeNullOrEmpty();
        }
    }
}