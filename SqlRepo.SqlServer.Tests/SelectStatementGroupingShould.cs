using System;
using NUnit.Framework;
using FluentAssertions;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class SelectStatementGroupingShould : SelectStatementTestBase
    {
        [Test]
        public void ThrowErrorIfOrderingAttemptedBeforeTableSpecified()
        {
            const string ExpectedMessage =
                "A table specification for the entity type and alias must be set using From or one of the Join methods before filtering, sorting or grouping can be applied.";
            this.Statement.Invoking(c => c.GroupBy<InnerEntity>(e => e.IntProperty))
                .Should().Throw<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
        }

        [Test]
        public void AddSpecificationOnGroupBy()
        {
            this.Statement.GroupBy(e => e.IntProperty);
            this.AssertGroupBySpecificationIsValid<TestEntity>(0, IntPropertyName);
        }

        [Test]
        public void AddMultipleSpecificationsOnGroupBy()
        {
            this.Statement.GroupBy(e => e.IntProperty, null, e => e.StringProperty);
            this.AssertGroupBySpecificationIsValid<TestEntity>(0, IntPropertyName);
            this.AssertGroupBySpecificationIsValid<TestEntity>(1, StringPropertyName);
        }

        [Test]
        public void AddSpecificationOnGroupByWithAlias()
        {
            const string Alias = "a";
            this.Statement.From(Alias)
                .GroupBy(e => e.IntProperty, Alias);
            this.AssertGroupBySpecificationIsValid<TestEntity>(0, IntPropertyName, Alias);
        }

        [Test]
        public void AddMultipleSpecificationsOnGroupByWithAlias()
        {
            const string Alias = "a";
            this.Statement.From(Alias)
                .GroupBy(e => e.IntProperty, Alias, e => e.StringProperty);
            this.AssertGroupBySpecificationIsValid<TestEntity>(0, IntPropertyName, Alias);
            this.AssertGroupBySpecificationIsValid<TestEntity>(1, StringPropertyName, Alias);
        }

        [Test]
        public void ThrowErrorOnHavingWhenGroupingNotInitialised()
        {
            const string ExpectedMessage =
                "Grouping has not been initialised, pluase a GroupBy method before any Having method.";
            this.Statement.Invoking(c => c.HavingAvg(e => e.IntProperty > 5))
                .Should().Throw<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
            this.Statement.Invoking(c => c.HavingCount(e => e.IntProperty > 5))
                .Should().Throw<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
            this.Statement.Invoking(c => c.HavingMax(e => e.IntProperty > 5))
                .Should().Throw<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
            this.Statement.Invoking(c => c.HavingMin(e => e.IntProperty > 5))
                .Should().Throw<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
            this.Statement.Invoking(c => c.HavingSum(e => e.IntProperty > 5))
                .Should().Throw<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
            this.Statement.Invoking(c => c.HavingCountAll(Comparison.GreaterThan, 5))
                .Should().Throw<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
        }

        [Test]
        public void AddSpecificationOnHavingAvg()
        {
            this.Statement.GroupBy(e => e.IntProperty)
                .HavingAvg(e => e.IntProperty > 1);
            this.AssertHavingSpecificationIsValid<TestEntity>(0, Aggregation.Avg, IntPropertyName, ">", "1");
        }

        [Test]
        public void AddSpecificationOnHavingAvgWithAlias()
        {
            const string Alias = "a";
            this.Statement.GroupBy(e => e.IntProperty)
                .HavingAvg(e => e.IntProperty > 1, Alias);
            this.AssertHavingSpecificationIsValid<TestEntity>(0,
                Aggregation.Avg,
                IntPropertyName,
                ">",
                "1",
                Alias);
        }

        [Test]
        public void AddSpecificationOnHavingCountAll()
        {
            this.Statement.GroupBy(e => e.IntProperty)
                .HavingCountAll(Comparison.GreaterThan, 1);
            this.AssertHavingSpecificationIsValid<TestEntity>(0, Aggregation.Count, "*", ">", "1");
        }

        [Test]
        public void AddSpecificationOnHavingCount()
        {
            this.Statement.GroupBy(e => e.IntProperty)
                .HavingCount(e => e.IntProperty > 1);
            this.AssertHavingSpecificationIsValid<TestEntity>(0, Aggregation.Count, IntPropertyName, ">", "1");
        }

        [Test]
        public void AddSpecificationOnHavingCountWithAlias()
        {
            const string Alias = "a";
            this.Statement.GroupBy(e => e.IntProperty)
                .HavingCount(e => e.IntProperty > 1, Alias);
            this.AssertHavingSpecificationIsValid<TestEntity>(0,
                Aggregation.Count,
                IntPropertyName,
                ">",
                "1",
                Alias);
        }

        [Test]
        public void AddSpecificationOnHavingMax()
        {
            this.Statement.GroupBy(e => e.IntProperty)
                .HavingMax(e => e.IntProperty > 1);
            this.AssertHavingSpecificationIsValid<TestEntity>(0, Aggregation.Max, IntPropertyName, ">", "1");
        }

        [Test]
        public void AddSpecificationOnHavingMaxWithAlias()
        {
            const string Alias = "a";
            this.Statement.GroupBy(e => e.IntProperty)
                .HavingMax(e => e.IntProperty > 1, Alias);
            this.AssertHavingSpecificationIsValid<TestEntity>(0,
                Aggregation.Max,
                IntPropertyName,
                ">",
                "1",
                Alias);
        }

        [Test]
        public void AddSpecificationOnHavingMin()
        {
            this.Statement.GroupBy(e => e.IntProperty)
                .HavingMin(e => e.IntProperty > 1);
            this.AssertHavingSpecificationIsValid<TestEntity>(0, Aggregation.Min, IntPropertyName, ">", "1");
        }

        [Test]
        public void AddSpecificationOnHavingMinWithAlias()
        {
            const string Alias = "a";
            this.Statement.GroupBy(e => e.IntProperty)
                .HavingMin(e => e.IntProperty > 1, Alias);
            this.AssertHavingSpecificationIsValid<TestEntity>(0,
                Aggregation.Min,
                IntPropertyName,
                ">",
                "1",
                Alias);
        }

        [Test]
        public void AddSpecificationOnHavingSum()
        {
            this.Statement.GroupBy(e => e.IntProperty)
                .HavingSum(e => e.IntProperty > 1);
            this.AssertHavingSpecificationIsValid<TestEntity>(0, Aggregation.Sum, IntPropertyName, ">", "1");
        }

        [Test]
        public void AddSpecificationOnHavingSumWithAlias()
        {
            const string Alias = "a";
            this.Statement.GroupBy(e => e.IntProperty)
                .HavingSum(e => e.IntProperty > 1, Alias);
            this.AssertHavingSpecificationIsValid<TestEntity>(0,
                Aggregation.Sum,
                IntPropertyName,
                ">",
                "1",
                Alias);
        }

        private void AssertGroupBySpecificationIsValid<T>(int index, string identifier, string @alias = null)
        {
            this.Statement.Specification.Groupings.Should()
                .NotBeNullOrEmpty();
            var specification = this.Statement.Specification.Groupings[index];
            specification.Identifer.Should()
                         .Be(identifier);
            specification.Alias.Should()
                         .Be(@alias);
            specification.EntityType.Should()
                         .Be(typeof(T));
        }

        private void AssertHavingSpecificationIsValid<TEntity>(int index,
            Aggregation expectedAggregation,
            string expectedIdentifier,
            string expectedOperator,
            string expectedValue,
            string expectedAlias = null)
        {
            this.Statement.Specification.Havings.Should()
                .NotBeNullOrEmpty();
            var specification = this.Statement.Specification.Havings[0];
            specification.Alias.Should()
                         .Be(expectedAlias);
            specification.Aggregation.Should()
                         .Be(expectedAggregation);
            specification.Identifier.Should()
                         .Be(expectedIdentifier);
            specification.Operator.Should()
                         .Be(expectedOperator);
            specification.EntityType.Should()
                         .Be(typeof(TEntity));
            specification.Value.Should()
                         .Be(expectedValue);
        }
    }
}