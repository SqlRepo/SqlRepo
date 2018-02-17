using System;
using NUnit.Framework;
using FluentAssertions;
using SqlRepo.Testing;
using System.Linq;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class SelectStatementFromShould : SelectStatementTestBase
    {
        [Test]
        public void DefaultTableConfiguration()
        {
            this.Statement.Specification.Tables.Should()
                .NotBeEmpty();
            var first = this.Statement.Specification.Tables.First();
            first.TableName.Should()
                 .Be(TestEntityName);
            first.Schema.Should()
                 .Be(ClauseBuilder.DefaultSchema);
            first.Alias.Should()
                 .BeNull();
        }

        [Test]
        public void UpdateTableScpecificationOnAliasOverride()
        {
            const string Alias = "a";
            this.Statement.From(Alias);
            this.Statement.Specification.Tables.First()
                .Alias.Should()
                .Be(Alias);
        }

        [Test]
        public void UpdateTableSpecificationOnOverridingTable()
        {
            const string TableName = "Table1";
            this.Statement.From(tableName: TableName);
            this.Statement.Specification.Tables.First()
                .TableName.Should()
                .Be(TableName);
        }

        [Test]
        public void UpdateTableSpecificationOnOverridingSchema()
        {
            const string TableSchema = "schema";
            this.Statement.From(tableSchema: TableSchema);
            this.Statement.Specification.Tables.First()
                .Schema.Should()
                .Be(TableSchema);
        }

        [Test]
        public void ThrowErrorIfSameTableJoinedWithoutAlias()
        {
            const string ExpectedMessage =
                "The entity has already been joined into the query, you must use a unique alias, table name override or schema override to join it again.";
            this.Statement.Invoking(e => e.InnerJoin<TestEntity>())
                .ShouldThrow<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
            this.Statement.Invoking(e => e.LeftOuterJoin<TestEntity>())
                .ShouldThrow<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
            this.Statement.Invoking(e => e.RightOuterJoin<TestEntity>())
                .ShouldThrow<InvalidOperationException>()
                .WithMessage(ExpectedMessage);
        }

        [Test]
        public void AddTableSpecificationOnInnerJoin()
        {
            this.Statement.InnerJoin<InnerEntity>();
            this.Statement.Specification.Tables.Count.Should()
                .Be(2);
            this.AssertTableSpecIsValid<InnerEntity>(1,
                JoinType.Inner,
                expectedTableName: InnerEntityName,
                expectedSchema: ClauseBuilder.DefaultSchema);
        }

        [Test]
        public void SetAliasOnInnerJoin()
        {
            const string Alias = "b";
            this.Statement.InnerJoin<InnerEntity>(Alias);
            this.AssertTableSpecIsValid<InnerEntity>(1, JoinType.Inner, Alias, InnerEntityName, ClauseBuilder.DefaultSchema);
        }

        [Test]
        public void SetTableNameOnInnerJoin()
        {
            const string TableName = "table1";
            this.Statement.InnerJoin<InnerEntity>(tableName: TableName);
            this.AssertTableSpecIsValid<InnerEntity>(1, JoinType.Inner, null, TableName, ClauseBuilder.DefaultSchema);
        }

        [Test]
        public void SetSchemaOnInnerJoin()
        {
            const string Schema = "schema";
            this.Statement.InnerJoin<InnerEntity>(tableSchema: Schema);
            this.AssertTableSpecIsValid<InnerEntity>(1, JoinType.Inner, null, InnerEntityName, Schema);
        }

        [Test]
        public void AddTableSpecificationOnLeftOuterJoin()
        {
            this.Statement.LeftOuterJoin<InnerEntity>();
            this.Statement.Specification.Tables.Count.Should()
                .Be(2);
            this.AssertTableSpecIsValid<InnerEntity>(1,
                JoinType.LeftOuter,
                expectedTableName: InnerEntityName,
                expectedSchema: ClauseBuilder.DefaultSchema);
        }

        [Test]
        public void SetAliasOnLeftOuterJoin()
        {
            const string Alias = "b";
            this.Statement.LeftOuterJoin<InnerEntity>(Alias);
            this.AssertTableSpecIsValid<InnerEntity>(1,
                JoinType.LeftOuter,
                Alias,
                InnerEntityName,
                ClauseBuilder.DefaultSchema);
        }

        [Test]
        public void SetTableNameOnLeftOuterJoin()
        {
            const string TableName = "table1";
            this.Statement.LeftOuterJoin<InnerEntity>(tableName: TableName);
            this.AssertTableSpecIsValid<InnerEntity>(1, JoinType.LeftOuter, null, TableName, ClauseBuilder.DefaultSchema);
        }

        [Test]
        public void SetSchemaOnLeftOuterJoin()
        {
            const string Schema = "schema";
            this.Statement.LeftOuterJoin<InnerEntity>(tableSchema: Schema);
            this.AssertTableSpecIsValid<InnerEntity>(1, JoinType.LeftOuter, null, InnerEntityName, Schema);
        }

        [Test]
        public void AddTableSpecificationOnRightOuterJoin()
        {
            this.Statement.RightOuterJoin<InnerEntity>();
            this.Statement.Specification.Tables.Count.Should()
                .Be(2);
            this.AssertTableSpecIsValid<InnerEntity>(1,
                JoinType.RightOuter,
                expectedTableName: InnerEntityName,
                expectedSchema: ClauseBuilder.DefaultSchema);
        }

        [Test]
        public void SetAliasOnRightOuterJoin()
        {
            const string Alias = "b";
            this.Statement.RightOuterJoin<InnerEntity>(Alias);
            this.AssertTableSpecIsValid<InnerEntity>(1,
                JoinType.RightOuter,
                Alias,
                InnerEntityName,
                ClauseBuilder.DefaultSchema);
        }

        [Test]
        public void SetTableNameOnRightOuterJoin()
        {
            const string TableName = "table1";
            this.Statement.RightOuterJoin<InnerEntity>(tableName: TableName);
            this.AssertTableSpecIsValid<InnerEntity>(1, JoinType.RightOuter, null, TableName, ClauseBuilder.DefaultSchema);
        }

        [Test]
        public void SetSchemaOnRightOuterJoin()
        {
            const string Schema = "schema";
            this.Statement.RightOuterJoin<InnerEntity>(tableSchema: Schema);
            this.AssertTableSpecIsValid<InnerEntity>(1, JoinType.RightOuter, null, InnerEntityName, Schema);
        }

        [Test]
        public void ThrowErrorIfOnUsedBeforeAnyJoin()
        {
            this.Statement.Invoking(e => e.On<TestEntity, InnerEntity>((l, r) => l.IntProperty == r.IntProperty))
                .ShouldThrow<InvalidOperationException>()
                .WithMessage("On cannot be used before initialising a join with one of the Join methods.");
            this.Statement.Invoking(e => e.OrOn<TestEntity, InnerEntity>((l, r) => l.IntProperty == r.IntProperty))
                .ShouldThrow<InvalidOperationException>()
                .WithMessage("On cannot be used before initialising a join with one of the Join methods.");
            this.Statement.Invoking(e => e.AndOn<TestEntity, InnerEntity>((l, r) => l.IntProperty == r.IntProperty))
                .ShouldThrow<InvalidOperationException>()
                .WithMessage("On cannot be used before initialising a join with one of the Join methods.");
        }

        [Test]
        public void AddConditionForJoinBetweenBaseAndSpecifiedTable()
        {
            this.Statement.InnerJoin<InnerEntity>()
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty);
            this.AssertJoinConditionIsValid<TestEntity, InnerEntity>(0, IntPropertyName, "=", IntPropertyName);
        }

        [Test]
        public void AddConditionForJoinBetweenBaseAndSpecifiedTableWithAlias()
        {
            const string Alias = "a";
            this.Statement.InnerJoin<InnerEntity>(Alias)
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty, rightTableAlias: Alias);
            this.AssertJoinConditionIsValid<TestEntity, InnerEntity>(0, IntPropertyName, "=", IntPropertyName, Alias);
        }

        [Test]
        public void AddConditionForJoinBetweenBaseWithAliasAndSpecifiedTableWithAlias()
        {
            const string LeftTableAlias = "a";
            const string RightTableAlias = "b";
            this.Statement
                .From(LeftTableAlias)
                .InnerJoin<InnerEntity>(RightTableAlias)
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty, LeftTableAlias, RightTableAlias);
            this.AssertJoinConditionIsValid<TestEntity, InnerEntity>(0, IntPropertyName, "=", IntPropertyName, RightTableAlias, LeftTableAlias);
        }

        [Test]
        public void AddConditionForJoinBetweenSameTableWithAliases()
        {
            const string LeftTableAlias = "a";
            const string RightTableAlias = "b";
            this.Statement
                .InnerJoin<InnerEntity>(LeftTableAlias)
                .InnerJoin<InnerEntity>(RightTableAlias)
                .On<InnerEntity, InnerEntity>((l, r) => l.IntProperty == r.IntProperty, LeftTableAlias, RightTableAlias);
            this.AssertJoinConditionIsValid<InnerEntity, InnerEntity>(0, IntPropertyName, "=", IntPropertyName, RightTableAlias, LeftTableAlias);
        }

        [Test]
        public void AddConditionForJoinBetweenSameTableWithOnlyRightAliased()
        {
            const string RightTableAlias = "b";
            this.Statement
                .InnerJoin<InnerEntity>()
                .InnerJoin<InnerEntity>(RightTableAlias)
                .On<InnerEntity, InnerEntity>((l, r) => l.IntProperty == r.IntProperty, rightTableAlias: RightTableAlias);
            this.AssertJoinConditionIsValid<InnerEntity, InnerEntity>(0, IntPropertyName, "=", IntPropertyName, RightTableAlias);
        }

        [Test]
        public void AddConditionForLeftOuterJoinBetweenBaseAndSpecifiedTable()
        {
            this.Statement.LeftOuterJoin<InnerEntity>()
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty);
            this.AssertJoinConditionIsValid<TestEntity, InnerEntity>(0, IntPropertyName, "=", IntPropertyName);
        }

        [Test]
        public void AddConditionForLeftOuterJoinBetweenBaseAndSpecifiedTableWithAlias()
        {
            const string Alias = "a";
            this.Statement.LeftOuterJoin<InnerEntity>(Alias)
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty, rightTableAlias: Alias);
            this.AssertJoinConditionIsValid<TestEntity, InnerEntity>(0, IntPropertyName, "=", IntPropertyName, Alias);
        }

        [Test]
        public void AddConditionForLeftOuterJoinBetweenBaseWithAliasAndSpecifiedTableWithAlias()
        {
            const string LeftTableAlias = "a";
            const string RightTableAlias = "b";
            this.Statement
                .From(LeftTableAlias)
                .LeftOuterJoin<InnerEntity>(RightTableAlias)
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty, LeftTableAlias, RightTableAlias);
            this.AssertJoinConditionIsValid<TestEntity, InnerEntity>(0, IntPropertyName, "=", IntPropertyName, RightTableAlias, LeftTableAlias);
        }

        [Test]
        public void AddConditionForLeftOuterJoinBetweenSameTableWithAliases()
        {
            const string LeftTableAlias = "a";
            const string RightTableAlias = "b";
            this.Statement
                .LeftOuterJoin<InnerEntity>(LeftTableAlias)
                .LeftOuterJoin<InnerEntity>(RightTableAlias)
                .On<InnerEntity, InnerEntity>((l, r) => l.IntProperty == r.IntProperty, LeftTableAlias, RightTableAlias);
            this.AssertJoinConditionIsValid<InnerEntity, InnerEntity>(0, IntPropertyName, "=", IntPropertyName, RightTableAlias, LeftTableAlias);
        }

        [Test]
        public void AddConditionForLeftOuterJoinBetweenSameTableWithOnlyRightAliased()
        {
            const string RightTableAlias = "b";
            this.Statement
                .LeftOuterJoin<InnerEntity>()
                .LeftOuterJoin<InnerEntity>(RightTableAlias)
                .On<InnerEntity, InnerEntity>((l, r) => l.IntProperty == r.IntProperty, rightTableAlias: RightTableAlias);
            this.AssertJoinConditionIsValid<InnerEntity, InnerEntity>(0, IntPropertyName, "=", IntPropertyName, RightTableAlias);
        }

        [Test]
        public void AddConditionForRightOuterJoinBetweenBaseAndSpecifiedTable()
        {
            this.Statement.RightOuterJoin<InnerEntity>()
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty);
            this.AssertJoinConditionIsValid<TestEntity, InnerEntity>(0, IntPropertyName, "=", IntPropertyName);
        }

        [Test]
        public void AddConditionForRightOuterJoinBetweenBaseAndSpecifiedTableWithAlias()
        {
            const string Alias = "a";
            this.Statement.RightOuterJoin<InnerEntity>(Alias)
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty, rightTableAlias: Alias);
            this.AssertJoinConditionIsValid<TestEntity, InnerEntity>(0, IntPropertyName, "=", IntPropertyName, Alias);
        }

        [Test]
        public void AddConditionForRightOuterJoinBetweenBaseWithAliasAndSpecifiedTableWithAlias()
        {
            const string LeftTableAlias = "a";
            const string RightTableAlias = "b";
            this.Statement
                .From(LeftTableAlias)
                .RightOuterJoin<InnerEntity>(RightTableAlias)
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty, LeftTableAlias, RightTableAlias);
            this.AssertJoinConditionIsValid<TestEntity, InnerEntity>(0, IntPropertyName, "=", IntPropertyName, RightTableAlias, LeftTableAlias);
        }

        [Test]
        public void AddConditionForRightOuterJoinBetweenSameTableWithAliases()
        {
            const string LeftTableAlias = "a";
            const string RightTableAlias = "b";
            this.Statement
                .RightOuterJoin<InnerEntity>(LeftTableAlias)
                .RightOuterJoin<InnerEntity>(RightTableAlias)
                .On<InnerEntity, InnerEntity>((l, r) => l.IntProperty == r.IntProperty, LeftTableAlias, RightTableAlias);
            this.AssertJoinConditionIsValid<InnerEntity, InnerEntity>(0, IntPropertyName, "=", IntPropertyName, RightTableAlias, LeftTableAlias);
        }

        [Test]
        public void AddConditionForRightOuterJoinBetweenSameTableWithOnlyRightAliased()
        {
            const string RightTableAlias = "b";
            this.Statement
                .RightOuterJoin<InnerEntity>()
                .RightOuterJoin<InnerEntity>(RightTableAlias)
                .On<InnerEntity, InnerEntity>((l, r) => l.IntProperty == r.IntProperty, rightTableAlias: RightTableAlias);
            this.AssertJoinConditionIsValid<InnerEntity, InnerEntity>(0, IntPropertyName, "=", IntPropertyName, RightTableAlias);
        }

        [Test]
        public void AddConditionCombinedWithAnd()
        {
            this.Statement.InnerJoin<InnerEntity>()
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty)
                .AndOn<InnerEntity>((l, r) => l.StringProperty == r.StringProperty);
            this.AssertJoinConditionIsValid<TestEntity, InnerEntity>(1, "StringProperty", "=", "StringProperty", expectedLogicalOperator: LogicalOperator.And);
        }

        [Test]
        public void AddConditionCombinedWithOr()
        {
            this.Statement.InnerJoin<InnerEntity>()
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty)
                .OrOn<InnerEntity>((l, r) => l.StringProperty == r.StringProperty);
            this.AssertJoinConditionIsValid<TestEntity, InnerEntity>(1, "StringProperty", "=", "StringProperty", expectedLogicalOperator: LogicalOperator.Or);
        }

        private void AssertJoinConditionIsValid<TLeft, TRight>(int index,
            string expectedLeftIdentifier,
            string expectedOperator,
            string expectedRightIdentifier,
            string expectedRightTableAlias = null,
            string expectedLeftTableAlias = null,
            LogicalOperator expectedLogicalOperator = LogicalOperator.NotSet)
        {
            this.Statement.Specification.Joins.Should()
                .NotBeNullOrEmpty();
            var condition = this.Statement.Specification.Joins[index];
            condition.LeftEntityType.Should()
                     .Be(typeof(TLeft));
            condition.LeftIdentifier.Should()
                     .Be(expectedLeftIdentifier);
            condition.LeftTableAlias.Should()
                     .Be(expectedLeftTableAlias);
            condition.LogicalOperator.Should()
                     .Be(expectedLogicalOperator);
            condition.Operator.Should()
                     .Be(expectedOperator);
            condition.RightIdentifier.Should()
                     .Be(expectedRightIdentifier);
            condition.RightTableAlias.Should()
                     .Be(expectedRightTableAlias);
            condition.RightEntityType.Should()
                     .Be(typeof(TRight));
        }

        private void AssertTableSpecIsValid<T>(int index,
            JoinType expectedJoinType = JoinType.None,
            string expectedAlias = null,
            string expectedTableName = null,
            string expectedSchema = null)
        {
            var joinedTableSpec = this.Statement.Specification.Tables[index];
            joinedTableSpec.JoinType.Should()
                           .Be(expectedJoinType);
            joinedTableSpec.Alias.Should()
                           .Be(expectedAlias);
            joinedTableSpec.TableName.Should()
                           .Be(expectedTableName);
            joinedTableSpec.Schema.Should()
                           .Be(expectedSchema);
            joinedTableSpec.EntityType.Should()
                           .Be(typeof(T));
        }
    }
}