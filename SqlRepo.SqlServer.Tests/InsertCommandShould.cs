using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class InsertCommandShould : SqlCommandTestBase<InsertCommand<TestEntity>, TestEntity>
    {
        [Test]
        public void DefaultSchemaToDbo()
        {
            this.Command.TableSchema.Should()
                .Be("dbo");
        }

        [Test]
        public void DefaultTableNameToNameOfType()
        {
            this.Command.TableName.Should()
                .Be("TestEntity");
        }

        [Test]
        public void SupportUsingSpecificSchema()
        {
            this.Command.UsingTableSchema(OtherValue);
            this.Command.TableSchema.Should()
                .Be(OtherValue);
        }

        [Test]
        public void SupportChainingAfterSettingTableSchema()
        {
            this.Command.UsingTableSchema(OtherValue)
                .Should()
                .Be(this.Command);
        }

        [Test]
        public void SupportUsingSpecificTableName()
        {
            this.Command.UsingTableName(OtherValue);
            this.Command.TableName.Should()
                .Be(OtherValue);
        }

        [Test]
        public void SupportChainingAfterSettingTableName()
        {
            this.Command.UsingTableName(OtherValue)
                .Should()
                .Be(this.Command);
        }

        [Test]
        public void ProduceCorrectDefaultTableSpecfication()
        {
            this.Command.With(e => e.IntProperty, 1)
                .Sql()
                .Should()
                .Contain(this.ExpectedTableSpecification(this.Command.TableSchema, this.Command.TableName));
        }

        [Test]
        public void ProduceCorrectNonDefaultTableSpecfication()
        {
            this.Command.UsingTableSchema(OtherValue)
                .UsingTableName(OtherValue)
                .With(e => e.IntProperty, 1)
                .Sql()
                .Should()
                .Contain(this.ExpectedTableSpecification(OtherValue, OtherValue));
        }

        [Test]
        public void BeCleanByDefault()
        {
            this.Command.IsClean.Should()
                .BeTrue();
        }

        [Test]
        public void BuildCorrectStatementWithOnlyStringPropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([StringProperty])\nVALUES('My Name');";
            this.Command.With(e => e.StringProperty, "My Name")
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyDateTimePropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([DateTimeProperty])\nVALUES('{0}');";
            var now = DateTime.UtcNow;
            this.Command.With(e => e.DateTimeProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, now.ToString(FormatString.DateTime)));
        }

        [Test]
        public void BuildCorrectStatementWithOnlyNullableDateTimePropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([NullableDateTimeProperty])\nVALUES('{0}');";
            var now = DateTime.UtcNow;
            this.Command.With(e => e.NullableDateTimeProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, now.ToString(FormatString.DateTime)));
        }

        [Test]
        public void BuildCorrectStatementWithOnlyDateTimeOffsetPropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([DateTimeOffsetProperty])\nVALUES('{0}');";
            var now = DateTimeOffset.UtcNow;
            this.Command.With(e => e.DateTimeOffsetProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, now.ToString(FormatString.DateTimeOffset)));
        }

        [Test]
        public void BuildCorrectStatementWithOnlyNullableDateTimeOffsetPropertySet()
        {
            const string expected =
                "INSERT [dbo].[TestEntity]([NullableDateTimeOffsetProperty])\nVALUES('{0}');";
            var now = DateTimeOffset.UtcNow;
            this.Command.With(e => e.NullableDateTimeOffsetProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, now.ToString(FormatString.DateTimeOffset)));
        }

        [Test]
        public void BuildCorrectStatementWithOnlyIntPropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([IntProperty])\nVALUES(1);";
            this.Command.With(e => e.IntProperty, 1)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyDoublePropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([DoubleProperty])\nVALUES(1.01);";
            this.Command.With(e => e.DoubleProperty, 1.01)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyDecimalPropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([DecimalProperty])\nVALUES(1.01);";
            this.Command.With(e => e.DecimalProperty, 1.01M)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlySinglePropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([SingleProperty])\nVALUES(1.01);";
            this.Command.With(e => e.SingleProperty, 1.01)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyBytePropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([ByteProperty])\nVALUES(1);";
            this.Command.With(e => e.ByteProperty, 1)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyShortPropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([ShortProperty])\nVALUES(1);";
            this.Command.With(e => e.ShortProperty, 1)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyGuidPropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([GuidProperty])\nVALUES('{0}');";
            var guid = Guid.NewGuid();
            this.Command.With(e => e.GuidProperty, guid)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, guid));
        }

        [Test]
        public void BuildCorrectStatementWithOnlyEnumPropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([TestEnumProperty])\nVALUES(1);";
            this.Command.With(e => e.TestEnumProperty, TestEnum.One)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void AlwaysAppendSelectStatementToReturnInsertedRow()
        {
            const string expected = "SELECT *\nFROM [dbo].[TestEntity]\nWHERE [Id] = SCOPE_IDENTITY();";

            this.Command.With(e => e.StringProperty, "My Name")
                .Sql()
                .Should()
                .EndWith(expected);
        }

        [Test]
        public void ReplaceSingleQuoteWithDoubleQuote()
        {
            const string expected = @"'The Formatted ''teststring'' string'''";
            this.Command.With(e => e.StringProperty, "The Formatted 'teststring' string'")
                .Sql()
                .Should()
                .Contain(expected);
        }

        [Test]
        public void NotReplaceDoubleQuote()
        {
            const string expected = @"'''teststring'''";
            this.Command.With(e => e.StringProperty, "''teststring''")
                .Sql()
                .Should()
                .Contain(expected);
        }

        [Test]
        [Ignore("Obsolete")]
        public void BuildCorrectStatementFromEntity()
        {
            this.AssumeTestEntityIsInitialised();
            var expected =
                string.Format(
                    "INSERT [dbo].[TestEntity]([DateTimeOffsetProperty], [NullableDateTimeOffsetProperty], [DateTimeProperty], [NullableDateTimeProperty], [DoubleProperty], [IntProperty], [IntProperty2], [StringProperty], [TestEnumProperty], [DecimalProperty], [ByteProperty], [ShortProperty], [SingleProperty], [GuidProperty])\n"
                    + "VALUES('{0}', '{1}', '{2}', '{3}', {4}, {5}, {6}, '{7}', {8}, {9}, {10}, {11}, {12}, '{13}');",
                    this.Entity.DateTimeOffsetProperty.ToString(FormatString.DateTimeOffset),
                    this.Entity.NullableDateTimeOffsetProperty.Value.ToString(FormatString.DateTimeOffset),
                    this.Entity.DateTimeProperty.ToString(FormatString.DateTime),
                    this.Entity.NullableDateTimeProperty.Value.ToString(FormatString.DateTime),
                    this.Entity.DoubleProperty,
                    this.Entity.IntProperty,
                    this.Entity.IntProperty2,
                    this.Entity.StringProperty,
                    (int)this.Entity.TestEnumProperty,
                    this.Entity.DecimalProperty,
                    this.Entity.ByteProperty,
                    this.Entity.ShortProperty,
                    this.Entity.SingleProperty,
                    this.Entity.GuidProperty);
            this.Command.For(this.Entity)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ThrowExceptionIfForCalledAfterWith()
        {
            this.AssumeTestEntityIsInitialised();
            this.Command.For(this.Entity);
            this.Command.Invoking(s => s.With(e => e.ByteProperty, 1))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfWithCalledAfterFor()
        {
            this.AssumeTestEntityIsInitialised();
            this.Command.With(e => e.ByteProperty, 1);
            this.Command.Invoking(s => s.For(this.Entity))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfBuildCalledWithoutInitialisingStatement()
        {
            this.Command.Invoking(s => s.Sql())
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void BeDirtyOnceWithHasBeenUsed()
        {
            this.Command.With(e => e.StringProperty, "My String")
                .IsClean.Should()
                .BeFalse();
        }

        [Test]
        public void BeDirtyOnceForHasBeenUsed()
        {
            this.Command.For(this.Entity)
                .IsClean.Should()
                .BeFalse();
        }

        [Test]
        public void BeCleanAfterFromScratch()
        {
            this.Command.For(this.Entity)
                .FromScratch()
                .IsClean.Should()
                .BeTrue();
        }

        [Test]
        public void ExecuteQueryOnGo()
        {
            const string expected =
                "INSERT [dbo].[TestEntity]([StringProperty])\nVALUES('My Name');\nSELECT *\nFROM [dbo].[TestEntity]\nWHERE [Id] = SCOPE_IDENTITY();";
            this.AssumeGoIsRequested();
            this.CommandExecutor.Received()
                .ExecuteReader(ConnectionString, expected);
        }

        [Test]
        public async Task ExecuteQueryOnGoAsync()
        {
            const string expected =
                "INSERT [dbo].[TestEntity]([StringProperty])\nVALUES('My Name');\nSELECT *\nFROM [dbo].[TestEntity]\nWHERE [Id] = SCOPE_IDENTITY();";
            await this.AssumeGoAsyncIsRequested();
            await this.CommandExecutor.Received()
                .ExecuteReaderAsync(ConnectionString, expected);
        }

        [Test]
        public void MapResultFromExecution()
        {
            this.AssumeGoIsRequested();
            this.EntityMapper.Received()
                .Map<TestEntity>(this.DataReader);
        }

        public string ExpectedTableSpecification(string schema, string table)
        {
            return $"INSERT [{schema}].[{table}]";
        }

        protected override InsertCommand<TestEntity> CreateCommand(ICommandExecutor commandExecutor,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher,
            ISelectClauseBuilder selectClauseBuilder,
            IFromClauseBuilder fromClauseBuilder,
            IWhereClauseBuilder whereClauseBuilder,
            string connectionString)
        {
            var command =
                new InsertCommand<TestEntity>(commandExecutor, entityMapper, writablePropertyMatcher);
            command.UseConnectionString(connectionString);
            return command;
        }

        private async Task AssumeGoAsyncIsRequested()
        {
            await this.Command.With(e => e.StringProperty, "My Name")
                      .GoAsync();
        }

        private void AssumeGoIsRequested()
        {
            this.Command.With(e => e.StringProperty, "My Name")
                .Go();
        }
    }
}