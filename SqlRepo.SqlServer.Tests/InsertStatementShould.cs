using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.SqlServer.Abstractions;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class InsertStatementShould : SqlStatementTestBase<InsertStatement<TestEntity>, TestEntity>
    {
        [Test]
        public void DefaultSchemaToDbo()
        {
            this.Statement.TableSchema.Should()
                .Be("dbo");
        }

        [Test]
        public void DefaultTableNameToNameOfType()
        {
            this.Statement.TableName.Should()
                .Be("TestEntity");
        }

        [Test]
        public void SupportUsingSpecificSchema()
        {
            this.Statement.UsingTableSchema(OtherValue);
            this.Statement.TableSchema.Should()
                .Be(OtherValue);
        }

        [Test]
        public void SupportChainingAfterSettingTableSchema()
        {
            this.Statement.UsingTableSchema(OtherValue)
                .Should()
                .Be(this.Statement);
        }

        [Test]
        public void SupportUsingSpecificTableName()
        {
            this.Statement.UsingTableName(OtherValue);
            this.Statement.TableName.Should()
                .Be(OtherValue);
        }

        [Test]
        public void SupportChainingAfterSettingTableName()
        {
            this.Statement.UsingTableName(OtherValue)
                .Should()
                .Be(this.Statement);
        }

        [Test]
        public void ProduceCorrectDefaultTableSpecfication()
        {
            this.Statement.With(e => e.IntProperty, 1)
                .Sql()
                .Should()
                .Contain(this.ExpectedTableSpecification(this.Statement.TableSchema, this.Statement.TableName));
        }

        [Test]
        public void ProduceCorrectNonDefaultTableSpecfication()
        {
            this.Statement.UsingTableSchema(OtherValue)
                .UsingTableName(OtherValue)
                .With(e => e.IntProperty, 1)
                .Sql()
                .Should()
                .Contain(this.ExpectedTableSpecification(OtherValue, OtherValue));
        }

        [Test]
        public void BeCleanByDefault()
        {
            this.Statement.IsClean.Should()
                .BeTrue();
        }

        [Test]
        public void BuildCorrectStatementWithOnlyStringPropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([StringProperty])\nVALUES('My Name');";
            this.Statement.With(e => e.StringProperty, "My Name")
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyDateTimePropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([DateTimeProperty])\nVALUES('{0}');";
            var now = DateTime.UtcNow;
            this.Statement.With(e => e.DateTimeProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, now.ToString(FormatString.DateTime)));
        }

        [Test]
        public void BuildCorrectStatementWithOnlyNullableDateTimePropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([NullableDateTimeProperty])\nVALUES('{0}');";
            var now = DateTime.UtcNow;
            this.Statement.With(e => e.NullableDateTimeProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, now.ToString(FormatString.DateTime)));
        }

        [Test]
        public void BuildCorrectStatementWithOnlyDateTimeOffsetPropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([DateTimeOffsetProperty])\nVALUES('{0}');";
            var now = DateTimeOffset.UtcNow;
            this.Statement.With(e => e.DateTimeOffsetProperty, now)
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
            this.Statement.With(e => e.NullableDateTimeOffsetProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, now.ToString(FormatString.DateTimeOffset)));
        }

        [Test]
        public void BuildCorrectStatementWithOnlyIntPropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([IntProperty])\nVALUES(1);";
            this.Statement.With(e => e.IntProperty, 1)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyDoublePropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([DoubleProperty])\nVALUES(1.01);";
            this.Statement.With(e => e.DoubleProperty, 1.01)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyDecimalPropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([DecimalProperty])\nVALUES(1.01);";
            this.Statement.With(e => e.DecimalProperty, 1.01M)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlySinglePropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([SingleProperty])\nVALUES(1.01);";
            this.Statement.With(e => e.SingleProperty, 1.01)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyBytePropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([ByteProperty])\nVALUES(1);";
            this.Statement.With(e => e.ByteProperty, 1)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyShortPropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([ShortProperty])\nVALUES(1);";
            this.Statement.With(e => e.ShortProperty, 1)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyGuidPropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([GuidProperty])\nVALUES('{0}');";
            var guid = Guid.NewGuid();
            this.Statement.With(e => e.GuidProperty, guid)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, guid));
        }

        [Test]
        public void BuildCorrectStatementWithOnlyEnumPropertySet()
        {
            const string expected = "INSERT [dbo].[TestEntity]([TestEnumProperty])\nVALUES(1);";
            this.Statement.With(e => e.TestEnumProperty, TestEnum.One)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void AlwaysAppendSelectStatementToReturnInsertedRow()
        {
            const string expected = "SELECT *\nFROM [dbo].[TestEntity]\nWHERE [Id] = SCOPE_IDENTITY();";

            this.Statement.With(e => e.StringProperty, "My Name")
                .Sql()
                .Should()
                .EndWith(expected);
        }

        [Test]
        public void ReplaceSingleQuoteWithDoubleQuote()
        {
            const string expected = @"'The Formatted ''teststring'' string'''";
            this.Statement.With(e => e.StringProperty, "The Formatted 'teststring' string'")
                .Sql()
                .Should()
                .Contain(expected);
        }

        [Test]
        public void NotReplaceDoubleQuote()
        {
            const string expected = @"'''teststring'''";
            this.Statement.With(e => e.StringProperty, "''teststring''")
                .Sql()
                .Should()
                .Contain(expected);
        }

        [Test]
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
            this.Statement.For(this.Entity)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void ThrowExceptionIfForCalledAfterWith()
        {
            this.AssumeTestEntityIsInitialised();
            this.Statement.For(this.Entity);
            this.Statement.Invoking(s => s.With(e => e.ByteProperty, 1))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfWithCalledAfterFor()
        {
            this.AssumeTestEntityIsInitialised();
            this.Statement.With(e => e.ByteProperty, 1);
            this.Statement.Invoking(s => s.For(this.Entity))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfBuildCalledWithoutInitialisingStatement()
        {
            this.Statement.Invoking(s => s.Sql())
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void BeDirtyOnceWithHasBeenUsed()
        {
            this.Statement.With(e => e.StringProperty, "My String")
                .IsClean.Should()
                .BeFalse();
        }

        [Test]
        public void BeDirtyOnceForHasBeenUsed()
        {
            this.Statement.For(this.Entity)
                .IsClean.Should()
                .BeFalse();
        }

        [Test]
        public void BeCleanAfterFromScratch()
        {
            this.Statement.For(this.Entity)
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
            this.StatementExecutor.Received()
                .ExecuteReader(expected);
        }

        [Test]
        public async Task ExecuteQueryOnGoAsync()
        {
            const string expected =
                "INSERT [dbo].[TestEntity]([StringProperty])\nVALUES('My Name');\nSELECT *\nFROM [dbo].[TestEntity]\nWHERE [Id] = SCOPE_IDENTITY();";
            await this.AssumeGoAsyncIsRequested();
            await this.StatementExecutor.Received()
                .ExecuteReaderAsync(expected);
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

        protected override InsertStatement<TestEntity> CreateStatement(IStatementExecutor statementExecutor,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher,
            ISelectClauseBuilder selectClauseBuilder,
            IFromClauseBuilder fromClauseBuilder,
            IWhereClauseBuilder whereClauseBuilder,
            string connectionString)
        {
            var command =
                new InsertStatement<TestEntity>(statementExecutor, entityMapper, writablePropertyMatcher);
            command.UseConnectionString(connectionString);
            return command;
        }

        private async Task AssumeGoAsyncIsRequested()
        {
            await this.Statement.With(e => e.StringProperty, "My Name")
                      .GoAsync();
        }

        private void AssumeGoIsRequested()
        {
            this.Statement.With(e => e.StringProperty, "My Name")
                .Go();
        }
    }
}