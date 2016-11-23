using NUnit.Framework;
using FluentAssertions;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class FromClauseBuilderShould
    {
        [SetUp]
        public void SetUp()
        {
            this.builder = new FromClauseBuilder();
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
            this.builder.From<TestEntity>();
            this.builder.IsClean.Should()
                .BeFalse();
        }

        [Test]
        public void ReturnCorrectSqlForFromWithNoOverridesOrAlias()
        {
            const string expected = "FROM [dbo].[TestEntity]";
            this.builder.From<TestEntity>()
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectSqlForFromWithOverridesAndAlias()
        {
            const string expected = "FROM [Schema].[Table] AS [a]";
            this.builder.From<TestEntity>("a", "Table", "Schema")
                .Sql()
                .Should()
                .Be(expected);
        }

        [Test]
        public void ReturnCorrectTablesSqlForInnerJoinWithNoOverridesOrAlias()
        {
            const string expected = "FROM [dbo].[TestEntity]\nINNER JOIN [dbo].[InnerEntity]";
            this.builder.From<TestEntity>()
                .InnerJoin<TestEntity, InnerEntity>()
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ReturnCorrectTablesSqlForInnerJoinWithOverridesAndAlias()
        {
            const string expected = "FROM [Schema].[Left] AS [a]\nINNER JOIN [Schema].[Right] AS [b]";
            this.builder.From<TestEntity>("a", "Left", "Schema")
                .InnerJoin<TestEntity, InnerEntity>("a", "b", "Right", "Schema")
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ReturnCorrectSqlForInnerJoinWithOverridesAndAlias()
        {
            const string expected =
                "FROM [dbo].[TestEntity]\nINNER JOIN [dbo].[InnerEntity]\nON [dbo].[TestEntity].[Id] = [dbo].[InnerEntity].[TestEntityId]";
            this.builder.From<TestEntity>()
                .InnerJoin<TestEntity, InnerEntity>()
                .On<TestEntity, InnerEntity>((l, r) => l.Id == r.TestEntityId)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ReturnCorrectTablesSqlForLeftOuterJoinWithNoOverridesOrAlias()
        {
            const string expected = "FROM [dbo].[TestEntity]\nLEFT OUTER JOIN [dbo].[InnerEntity]";
            this.builder.From<TestEntity>()
                .LeftOuterJoin<TestEntity, InnerEntity>()
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ReturnCorrectTablesSqlForLeftOuterJoinWithOverridesAndAlias()
        {
            const string expected = "FROM [Schema].[Left] AS [a]\nLEFT OUTER JOIN [Schema].[Right] AS [b]";
            this.builder.From<TestEntity>("a", "Left", "Schema")
                .LeftOuterJoin<TestEntity, InnerEntity>("a", "b", "Right", "Schema")
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ReturnCorrectSqlForLeftOuterJoinWithOverridesAndAlias()
        {
            const string expected =
                "FROM [dbo].[TestEntity]\nLEFT OUTER JOIN [dbo].[InnerEntity]\nON [dbo].[TestEntity].[Id] = [dbo].[InnerEntity].[TestEntityId]";
            this.builder.From<TestEntity>()
                .LeftOuterJoin<TestEntity, InnerEntity>()
                .On<TestEntity, InnerEntity>((l, r) => l.Id == r.TestEntityId)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ReturnCorrectTablesSqlForRightOuterJoinWithNoOverridesOrAlias()
        {
            const string expected = "FROM [dbo].[TestEntity]\nRIGHT OUTER JOIN [dbo].[InnerEntity]";
            this.builder.From<TestEntity>()
                .RightOuterJoin<TestEntity, InnerEntity>()
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ReturnCorrectTablesSqlForRightOuterJoinWithOverridesAndAlias()
        {
            const string expected = "FROM [Schema].[Right] AS [a]\nRIGHT OUTER JOIN [Schema].[Right] AS [b]";
            this.builder.From<TestEntity>("a", "Right", "Schema")
                .RightOuterJoin<TestEntity, InnerEntity>("a", "b", "Right", "Schema")
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ReturnCorrectSqlForRightOuterJoinWithoutOverridesAndAlias()
        {
            const string expected =
                "FROM [dbo].[TestEntity]\nRIGHT OUTER JOIN [dbo].[InnerEntity]\nON [dbo].[TestEntity].[Id] = [dbo].[InnerEntity].[TestEntityId]";
            this.builder.From<TestEntity>()
                .RightOuterJoin<TestEntity, InnerEntity>()
                .On<TestEntity, InnerEntity>((l, r) => l.Id == r.TestEntityId)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ThrowErrorIfSameTypeJoinedWithoutAliasing()
        {
            this.builder.Invoking(b => b.From<TestEntity>()
                                        .InnerJoin<TestEntity, TestEntity>())
                .ShouldThrow<AliasRequiredException>();
        }

        [Test]
        public void NotThrowErrorIfDifferentTypesJoinedWithoutAliasing()
        {
            this.builder.Invoking(b => b.From<TestEntity>()
                                        .InnerJoin<TestEntity, InnerEntity>())
                .ShouldNotThrow<AliasRequiredException>();
        }

        [Test]
        public void ThrowErrorIfSameTypeLeftJoinedWithoutAliasing()
        {
            this.builder.Invoking(b => b.From<TestEntity>()
                                        .LeftOuterJoin<TestEntity, TestEntity>())
                .ShouldThrow<AliasRequiredException>();
        }

        [Test]
        public void ThrowErrorIfSameTypeRightJoinedWithoutAliasing()
        {
            this.builder.Invoking(b => b.From<TestEntity>()
                                        .RightOuterJoin<TestEntity, TestEntity>())
                .ShouldThrow<AliasRequiredException>();
        }

        [Test]
        public void ThrowErrorIfTableIsJoinedWithDuplicateAlias()
        {
            this.builder.Invoking(b => b.From<TestEntity>("a")
                                       .InnerJoin<TestEntity, InnerEntity>(rightTableAlias: "a"))
               .ShouldThrow<DuplicateAliasException>();
        }

        [Test]
        public void ThrowErrorIfTableIsLeftJoinedWithDuplicateAlias()
        {
            this.builder.Invoking(b => b.From<TestEntity>("a")
                                       .LeftOuterJoin<TestEntity, InnerEntity>(rightTableAlias: "a"))
               .ShouldThrow<DuplicateAliasException>();
        }

        [Test]
        public void ThrowErrorIfTableIsRightJoinedWithDuplicateAlias()
        {
            this.builder.Invoking(b => b.From<TestEntity>("a")
                                       .RightOuterJoin<TestEntity, InnerEntity>(rightTableAlias: "a"))
               .ShouldThrow<DuplicateAliasException>();
        }

        [Test]
        public void ReturnDefaultRootTableDefinition()
        {
            var result = this.builder.From<TestEntity>()
                             .TableDefinition<TestEntity>();
            this.AssertTableDefinitionIsCorrect<TestEntity>(result, "TestEntity", "dbo", null);
        }

        [Test]
        public void ReturnCorrectRootTableDefinitionWithAlias()
        {
            var result = this.builder.From<TestEntity>("a")
                            .TableDefinition<TestEntity>();
            this.AssertTableDefinitionIsCorrect<TestEntity>(result, "TestEntity", "dbo", "a");
        }

        [Test]
        public void ReturnCorrectRootTableDefinitionWithNameOverride()
        {
            var result = this.builder.From<TestEntity>(tableName: "Table1")
                            .TableDefinition<TestEntity>();
            this.AssertTableDefinitionIsCorrect<TestEntity>(result, "Table1", "dbo", null);
        }

        [Test]
        public void ReturnCorrectRootTableDefinitionWithSchemaOverride()
        {
            var result = this.builder.From<TestEntity>(tableSchema: "schema")
                            .TableDefinition<TestEntity>();
            this.AssertTableDefinitionIsCorrect<TestEntity>(result, "TestEntity", "schema", null);
        }

        [Test]
        public void ReturnCorrectRootTableDefinitionOfJoinedTableWithoutOverrides()
        {
            var result = this.builder.From<TestEntity>()
                           .InnerJoin<TestEntity, InnerEntity>()
                           .TableDefinition<InnerEntity>();
            this.AssertTableDefinitionIsCorrect<InnerEntity>(result, "InnerEntity", "dbo", null);
        }

        [Test]
        public void ReturnCorrectTableDefinitionOfJoinedTableWithoutNameOverride()
        {
            var result = this.builder.From<TestEntity>()
                           .InnerJoin<TestEntity, InnerEntity>(rightTableName: "Inner")
                           .TableDefinition<InnerEntity>();
            this.AssertTableDefinitionIsCorrect<InnerEntity>(result, "Inner", "dbo", null);
        }

        [Test]
        public void ReturnCorrectTableDefinitionOfJoinedTableWithSchemaOverride()
        {
            var result = this.builder.From<TestEntity>()
                           .InnerJoin<TestEntity, InnerEntity>(rightTableSchema: "schema")
                           .TableDefinition<InnerEntity>();
            this.AssertTableDefinitionIsCorrect<InnerEntity>(result, "InnerEntity", "schema", null);
        }

        [Test]
        public void ReturnCorrectTableDefinitionOfJoinedTableWithAlias()
        {
            var result = this.builder.From<TestEntity>()
                           .InnerJoin<TestEntity, InnerEntity>(rightTableAlias: "alias")
                           .TableDefinition<InnerEntity>();
            this.AssertTableDefinitionIsCorrect<InnerEntity>(result, "InnerEntity", "dbo", "alias");
        }

        [Test]
        public void ReturnCorrectTableDefinitionOfLeftJoinedTableWithoutNameOverride()
        {
            var result = this.builder.From<TestEntity>()
                           .LeftOuterJoin<TestEntity, InnerEntity>(rightTableName: "Inner")
                           .TableDefinition<InnerEntity>();
            this.AssertTableDefinitionIsCorrect<InnerEntity>(result, "Inner", "dbo", null);
        }

        [Test]
        public void ReturnCorrectTableDefinitionOfLeftJoinedTableWithSchemaOverride()
        {
            var result = this.builder.From<TestEntity>()
                           .LeftOuterJoin<TestEntity, InnerEntity>(rightTableSchema: "schema")
                           .TableDefinition<InnerEntity>();
            this.AssertTableDefinitionIsCorrect<InnerEntity>(result, "InnerEntity", "schema", null);
        }

        [Test]
        public void ReturnCorrectTableDefinitionOfLeftJoinedTableWithAlias()
        {
            var result = this.builder.From<TestEntity>()
                           .LeftOuterJoin<TestEntity, InnerEntity>(rightTableAlias: "alias")
                           .TableDefinition<InnerEntity>();
            this.AssertTableDefinitionIsCorrect<InnerEntity>(result, "InnerEntity", "dbo", "alias");
        }

        [Test]
        public void ReturnCorrectTableDefinitionOfRightJoinedTableWithoutNameOverride()
        {
            var result = this.builder.From<TestEntity>()
                           .RightOuterJoin<TestEntity, InnerEntity>(rightTableName: "Inner")
                           .TableDefinition<InnerEntity>();
            this.AssertTableDefinitionIsCorrect<InnerEntity>(result, "Inner", "dbo", null);
        }

        [Test]
        public void ReturnCorrectTableDefinitionOfRightJoinedTableWithSchemaOverride()
        {
            var result = this.builder.From<TestEntity>()
                           .RightOuterJoin<TestEntity, InnerEntity>(rightTableSchema: "schema")
                           .TableDefinition<InnerEntity>();
            this.AssertTableDefinitionIsCorrect<InnerEntity>(result, "InnerEntity", "schema", null);
        }

        [Test]
        public void ReturnCorrectTableDefinitionOfRightJoinedTableWithAlias()
        {
            var result = this.builder.From<TestEntity>()
                           .RightOuterJoin<TestEntity, InnerEntity>(rightTableAlias: "alias")
                           .TableDefinition<InnerEntity>();
            this.AssertTableDefinitionIsCorrect<InnerEntity>(result, "InnerEntity", "dbo", "alias");
        }

        [Test]
        public void ReturnCorrectTableDefinitionByAlias()
        {
            this.builder.From<TestEntity>()
                .RightOuterJoin<TestEntity, TestEntity>(rightTableAlias: "a");

            this.AssertTableDefinitionIsCorrect<TestEntity>(this.builder.TableDefinition<TestEntity>(), "TestEntity", "dbo", null);
            this.AssertTableDefinitionIsCorrect<TestEntity>(this.builder.TableDefinition<TestEntity>("a"), "TestEntity", "dbo", "a");
        }

        private IFromClauseBuilder builder;

        private void AssertTableDefinitionIsCorrect<T>(TableDefinition actual,
            string expectedName,
            string expectedSchema,
            string expectedAlias)
        {
            actual.Should()
                  .NotBeNull();
            actual.TableType.Should()
                  .Be(typeof(T));
            actual.Name.Should()
                  .Be(expectedName);
            actual.Schema.Should()
                  .Be(expectedSchema);
            actual.Alias.Should()
                  .Be(expectedAlias);
        }
    }
}