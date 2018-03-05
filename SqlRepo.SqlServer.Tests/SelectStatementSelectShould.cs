using System;
using NUnit.Framework;
using FluentAssertions;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class SelectStatementSelectShould : SelectStatementTestBase
    {
        [Test]
        public void UpdateConfigOnTop()
        {
            const int Rows = 1;
            this.Statement.Top(Rows);
            this.Statement.Specification.Top.HasValue.Should()
                .BeTrue();
            this.Statement.Specification.Top.GetValueOrDefault()
                .Should()
                .Be(Rows);
        }

        [Test]
        public void UpdateConfigOnPercent()
        {
            this.Statement.Top(50)
                .Percent();
            this.Statement.Specification.UseTopPercent.Should()
                .BeTrue();
            ;
        }

        [Test]
        public void ThrowErrorIfPercentIsCalledWithoutSettingTop()
        {
            this.Statement.Invoking(e => e.Percent())
                .ShouldThrow<InvalidOperationException>()
                .WithMessage("Please call Top to set a value before calling Percent");
        }

        [Test]
        public void AddColumnSelectionForSingleColumnWithNoAlias()
        {
            this.Statement.Select(e => e.StringProperty);
            this.AssertColumSelectionIsValid(0, "StringProperty", typeof(TestEntity));
        }

        [Test]
        public void AddColumnSelectionForSingleColumnWithNoAliasWithTypeSepcifier()
        {
            this.Statement.Select<InnerEntity>(e => e.StringProperty);
            this.AssertColumSelectionIsValid(0, "StringProperty", typeof(InnerEntity));
        }

        [Test]
        public void AddColumnSelectionForMultipleColumnsWithNoAlias()
        {
            this.Statement.Select(e => e.StringProperty, e => e.ByteProperty, e => e.DecimalProperty);
            this.AssertColumSelectionIsValid(0, "StringProperty", typeof(TestEntity));
            this.AssertColumSelectionIsValid(1, "ByteProperty", typeof(TestEntity));
            this.AssertColumSelectionIsValid(2, "DecimalProperty", typeof(TestEntity));
        }

        [Test]
        public void AddColumnSelectionForSingleColumnWithAlias()
        {
            this.Statement.Select(e => e.StringProperty, "a");
            this.AssertColumSelectionIsValid(0, "StringProperty", typeof(TestEntity), "a");
        }

        [Test]
        public void AddColumnSelectionForMultipleColumnsWithAlias()
        {
            this.Statement.Select(e => e.StringProperty, "a", e => e.ByteProperty, e => e.DecimalProperty);
            this.AssertColumSelectionIsValid(0, "StringProperty", typeof(TestEntity), "a");
            this.AssertColumSelectionIsValid(1, "ByteProperty", typeof(TestEntity), "a");
            this.AssertColumSelectionIsValid(2, "DecimalProperty", typeof(TestEntity), "a");
        }

        [Test]
        public void AddColumnSelectionForSelectAllWithNoAliasOrTypeSpecifier()
        {
            this.Statement.SelectAll();
            this.AssertColumSelectionIsValid(0, "*", typeof(TestEntity));
        }

        [Test]
        public void AddColumnSelectionForSelectAllWithAliasAndNoTypeSpecifier()
        {
            this.Statement.SelectAll("a");
            this.AssertColumSelectionIsValid(0, "*", typeof(TestEntity), "a");
        }

        [Test]
        public void AddColumnSelectionForSelectAllWithNoAliasWithTypeSpecifier()
        {
            this.Statement.SelectAll<InnerEntity>();
            this.AssertColumSelectionIsValid(0, "*", typeof(InnerEntity));
        }

        [Test]
        public void AddColumnSelectionForSelectAllWithAliasAndTypeSpecifier()
        {
            this.Statement.SelectAll<InnerEntity>("a");
            this.AssertColumSelectionIsValid(0, "*", typeof(InnerEntity), "a");
        }

        [Test]
        public void AddColumnSelectionOnAvgWithoutAlias()
        {
            this.Statement.Avg(e => e.IntProperty);
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), null, Aggregation.Avg);
        }

        [Test]
        public void AddColumnSelectorOnAvgWithAlias()
        {
            this.Statement.Avg(e => e.IntProperty, "a");
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), "a", Aggregation.Avg);
        }

        [Test]
        public void AddColumnSelectionOnCountWithoutAlias()
        {
            this.Statement.Count(e => e.IntProperty);
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), null, Aggregation.Count);
        }

        [Test]
        public void AddColumnSelectorOnCountWithAlias()
        {
            this.Statement.Count(e => e.IntProperty, "a");
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), "a", Aggregation.Count);
        }

        [Test]
        public void AddColunSelectionOnCountAll()
        {
            this.Statement.CountAll();
            var columns = this.Statement.Specification.Columns;
            columns.Should()
                   .NotBeEmpty();
            var selection = columns[0];
            selection.Identifier.Should()
                     .Be("*");
            selection.Aggregation.Should()
                     .Be(Aggregation.Count);
        }

        [Test]
        public void AddColumnSelectionOnMaxWithoutAlias()
        {
            this.Statement.Max(e => e.IntProperty);
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), null, Aggregation.Max);
        }

        [Test]
        public void AddColumnSelectorOnMaxWithAlias()
        {
            this.Statement.Max(e => e.IntProperty, "a");
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), "a", Aggregation.Max);
        }

        [Test]
        public void AddColumnSelectionOnMinWithoutAlias()
        {
            this.Statement.Min(e => e.IntProperty);
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), null, Aggregation.Min);
        }

        [Test]
        public void AddColumnSelectorOnMinWithAlias()
        {
            this.Statement.Min(e => e.IntProperty, "a");
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), "a", Aggregation.Min);
        }

        [Test]
        public void AddColumnSelectionOnSumWithoutAlias()
        {
            this.Statement.Sum(e => e.IntProperty);
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), null, Aggregation.Sum);
        }

        [Test]
        public void AddColumnSelectorOnSumWithAlias()
        {
            this.Statement.Sum(e => e.IntProperty, "a");
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), "a", Aggregation.Sum);
        }

        private void AssertColumSelectionIsValid(int index,
            string expectedName,
            Type expectedType,
            string expectedAlias = null,
            Aggregation expectedAggregation = Aggregation.None)
        {
            var columns = this.Statement.Specification.Columns;
            columns.Should()
                   .NotBeEmpty();
            var selection = columns[index];
            selection.Identifier.Should()
                     .Be(expectedName);
            selection.Alias.Should()
                     .Be(expectedAlias);
            selection.EntityType.Should()
                     .Be(expectedType);
            selection.Aggregation.Should()
                     .Be(expectedAggregation);
        }
    }
}