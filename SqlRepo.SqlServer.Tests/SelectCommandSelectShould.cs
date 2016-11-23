using System;
using NUnit.Framework;
using FluentAssertions;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class SelectCommandSelectShould : SelectCommandTestBase
    {
        [Test]
        public void UpdateConfigOnTop()
        {
            const int Rows = 1;
            this.Command.Top(Rows);
            this.Command.Specification.Top.HasValue.Should()
                .BeTrue();
            this.Command.Specification.Top.GetValueOrDefault()
                .Should()
                .Be(Rows);
        }

        [Test]
        public void UpdateConfigOnPercent()
        {
            this.Command.Top(50)
                .Percent();
            this.Command.Specification.UseTopPercent.Should()
                .BeTrue();
            ;
        }

        [Test]
        public void ThrowErrorIfPercentIsCalledWithoutSettingTop()
        {
            this.Command.Invoking(e => e.Percent())
                .ShouldThrow<InvalidOperationException>()
                .WithMessage("Please call Top to set a value before calling Percent");
        }

        [Test]
        public void AddColumnSelectionForSingleColumnWithNoAlias()
        {
            this.Command.Select(e => e.StringProperty);
            this.AssertColumSelectionIsValid(0, "StringProperty", typeof(TestEntity));
        }

        [Test]
        public void AddColumnSelectionForSingleColumnWithNoAliasWithTypeSepcifier()
        {
            this.Command.Select<InnerEntity>(e => e.StringProperty);
            this.AssertColumSelectionIsValid(0, "StringProperty", typeof(InnerEntity));
        }

        [Test]
        public void AddColumnSelectionForMultipleColumnsWithNoAlias()
        {
            this.Command.Select(e => e.StringProperty, null, e => e.ByteProperty, e => e.DecimalProperty);
            this.AssertColumSelectionIsValid(0, "StringProperty", typeof(TestEntity));
            this.AssertColumSelectionIsValid(1, "ByteProperty", typeof(TestEntity));
            this.AssertColumSelectionIsValid(2, "DecimalProperty", typeof(TestEntity));
        }

        [Test]
        public void AddColumnSelectionForSingleColumnWithAlias()
        {
            this.Command.Select(e => e.StringProperty, "a");
            this.AssertColumSelectionIsValid(0, "StringProperty", typeof(TestEntity), "a");
        }

        [Test]
        public void AddColumnSelectionForMultipleColumnsWithAlias()
        {
            this.Command.Select(e => e.StringProperty, "a", e => e.ByteProperty, e => e.DecimalProperty);
            this.AssertColumSelectionIsValid(0, "StringProperty", typeof(TestEntity), "a");
            this.AssertColumSelectionIsValid(1, "ByteProperty", typeof(TestEntity), "a");
            this.AssertColumSelectionIsValid(2, "DecimalProperty", typeof(TestEntity), "a");
        }

        [Test]
        public void AddColumnSelectionForSelectAllWithNoAliasOrTypeSpecifier()
        {
            this.Command.SelectAll();
            this.AssertColumSelectionIsValid(0, "*", typeof(TestEntity));
        }

        [Test]
        public void AddColumnSelectionForSelectAllWithAliasAndNoTypeSpecifier()
        {
            this.Command.SelectAll("a");
            this.AssertColumSelectionIsValid(0, "*", typeof(TestEntity), "a");
        }

        [Test]
        public void AddColumnSelectionForSelectAllWithNoAliasWithTypeSpecifier()
        {
            this.Command.SelectAll<InnerEntity>();
            this.AssertColumSelectionIsValid(0, "*", typeof(InnerEntity));
        }

        [Test]
        public void AddColumnSelectionForSelectAllWithAliasAndTypeSpecifier()
        {
            this.Command.SelectAll<InnerEntity>("a");
            this.AssertColumSelectionIsValid(0, "*", typeof(InnerEntity), "a");
        }

        [Test]
        public void AddColumnSelectionOnAvgWithoutAlias()
        {
            this.Command.Avg(e => e.IntProperty);
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), null, Aggregation.Avg);
        }

        [Test]
        public void AddColumnSelectorOnAvgWithAlias()
        {
            this.Command.Avg(e => e.IntProperty, "a");
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), "a", Aggregation.Avg);
        }

        [Test]
        public void AddColumnSelectionOnCountWithoutAlias()
        {
            this.Command.Count(e => e.IntProperty);
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), null, Aggregation.Count);
        }

        [Test]
        public void AddColumnSelectorOnCountWithAlias()
        {
            this.Command.Count(e => e.IntProperty, "a");
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), "a", Aggregation.Count);
        }

        [Test]
        public void AddColunSelectionOnCountAll()
        {
            this.Command.CountAll();
            var columns = this.Command.Specification.Columns;
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
            this.Command.Max(e => e.IntProperty);
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), null, Aggregation.Max);
        }

        [Test]
        public void AddColumnSelectorOnMaxWithAlias()
        {
            this.Command.Max(e => e.IntProperty, "a");
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), "a", Aggregation.Max);
        }

        [Test]
        public void AddColumnSelectionOnMinWithoutAlias()
        {
            this.Command.Min(e => e.IntProperty);
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), null, Aggregation.Min);
        }

        [Test]
        public void AddColumnSelectorOnMinWithAlias()
        {
            this.Command.Min(e => e.IntProperty, "a");
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), "a", Aggregation.Min);
        }

        [Test]
        public void AddColumnSelectionOnSumWithoutAlias()
        {
            this.Command.Sum(e => e.IntProperty);
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), null, Aggregation.Sum);
        }

        [Test]
        public void AddColumnSelectorOnSumWithAlias()
        {
            this.Command.Sum(e => e.IntProperty, "a");
            this.AssertColumSelectionIsValid(0, "IntProperty", typeof(TestEntity), "a", Aggregation.Sum);
        }

        private void AssertColumSelectionIsValid(int index,
            string expectedName,
            Type expectedType,
            string expectedAlias = null,
            Aggregation expectedAggregation = Aggregation.None)
        {
            var columns = this.Command.Specification.Columns;
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