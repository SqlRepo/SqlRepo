using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
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
            const string Expected = "INSERT [dbo].[TestEntity]([StringProperty])\nVALUES('My Name');";
            this.Statement.With(e => e.StringProperty, "My Name")
                .Sql()
                .Should()
                .StartWith(Expected);
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
            const string Expected = "INSERT [dbo].[TestEntity]([NullableDateTimeProperty])\nVALUES('{0}');";
            var now = DateTime.UtcNow;
            this.Statement.With(e => e.NullableDateTimeProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(Expected, now.ToString(FormatString.DateTime)));
        }

        [Test]
        public void BuildCorrectStatementWithOnlyDateTimeOffsetPropertySet()
        {
            const string Expected = "INSERT [dbo].[TestEntity]([DateTimeOffsetProperty])\nVALUES('{0}');";
            var now = DateTimeOffset.UtcNow;
            this.Statement.With(e => e.DateTimeOffsetProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(Expected, now.ToString(FormatString.DateTimeOffset)));
        }

        [Test]
        public void BuildCorrectStatementWithOnlyNullableDateTimeOffsetPropertySet()
        {
            const string Expected =
                "INSERT [dbo].[TestEntity]([NullableDateTimeOffsetProperty])\nVALUES('{0}');";
            var now = DateTimeOffset.UtcNow;
            this.Statement.With(e => e.NullableDateTimeOffsetProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(Expected, now.ToString(FormatString.DateTimeOffset)));
        }

        [Test]
        public void BuildCorrectStatementWithOnlyIntPropertySet()
        {
            const string Expected = "INSERT [dbo].[TestEntity]([IntProperty])\nVALUES(1);";
            this.Statement.With(e => e.IntProperty, 1)
                .Sql()
                .Should()
                .StartWith(Expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyDoublePropertySet()
        {
            const string Expected = "INSERT [dbo].[TestEntity]([DoubleProperty])\nVALUES(1.01);";
            this.Statement.With(e => e.DoubleProperty, 1.01)
                .Sql()
                .Should()
                .StartWith(Expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyDecimalPropertySet()
        {
            const string Expected = "INSERT [dbo].[TestEntity]([DecimalProperty])\nVALUES(1.01);";
            this.Statement.With(e => e.DecimalProperty, 1.01M)
                .Sql()
                .Should()
                .StartWith(Expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlySinglePropertySet()
        {
            const string Expected = "INSERT [dbo].[TestEntity]([SingleProperty])\nVALUES(1.01);";
            this.Statement.With(e => e.SingleProperty, 1.01)
                .Sql()
                .Should()
                .StartWith(Expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyBytePropertySet()
        {
            const string Expected = "INSERT [dbo].[TestEntity]([ByteProperty])\nVALUES(1);";
            this.Statement.With(e => e.ByteProperty, 1)
                .Sql()
                .Should()
                .StartWith(Expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyShortPropertySet()
        {
            const string Expected = "INSERT [dbo].[TestEntity]([ShortProperty])\nVALUES(1);";
            this.Statement.With(e => e.ShortProperty, 1)
                .Sql()
                .Should()
                .StartWith(Expected);
        }

        [Test]
        public void BuildCorrectStatementWithOnlyGuidPropertySet()
        {
            const string Expected = "INSERT [dbo].[TestEntity]([GuidProperty])\nVALUES('{0}');";
            var guid = Guid.NewGuid();
            this.Statement.With(e => e.GuidProperty, guid)
                .Sql()
                .Should()
                .StartWith(string.Format(Expected, guid));
        }

        [Test]
        public void BuildCorrectStatementWithOnlyEnumPropertySet()
        {
            const string Expected = "INSERT [dbo].[TestEntity]([TestEnumProperty])\nVALUES(1);";
            this.Statement.With(e => e.TestEnumProperty, TestEnum.One)
                .Sql()
                .Should()
                .StartWith(Expected);
        }

        [Test]
        public void AlwaysAppendSelectStatementToReturnInsertedRow()
        {
            const string Expected = "SELECT *\nFROM [dbo].[TestEntity]\nWHERE [Id] = SCOPE_IDENTITY();";

            this.Statement.With(e => e.StringProperty, "My Name")
                .Sql()
                .Should()
                .EndWith(Expected);
        }

        [Test]
        public void ReplaceSingleQuoteWithDoubleQuote()
        {
            const string Expected = @"'The Formatted ''teststring'' string'''";
            this.Statement.With(e => e.StringProperty, "The Formatted 'teststring' string'")
                .Sql()
                .Should()
                .Contain(Expected);
        }

        [Test]
        public void NotReplaceDoubleQuote()
        {
            const string Expected = @"'''teststring'''";
            this.Statement.With(e => e.StringProperty, "''teststring''")
                .Sql()
                .Should()
                .Contain(Expected);
        }

        [Test]
        public void BuildCorrectStatementFromEntity()
        {
            this.AssumeTestEntityIsInitialised();
            var expected =
                "INSERT [dbo].[TestEntity]([BooleanProperty], [BooleanProperty2], [ByteProperty], [DateTimeOffsetProperty], [DateTimeProperty], [DecimalProperty], [DoubleProperty], [GuidProperty], [IntProperty], [IntProperty2], [NullableDateTimeOffsetProperty], [NullableDateTimeProperty], [ShortProperty], [SingleProperty], [StringProperty], [TestEnumProperty])\n"
                + $"VALUES(0, 0, 1, '{this.Entity.DateTimeOffsetProperty.ToString(FormatString.DateTimeOffset)}', '{this.Entity.DateTimeProperty.ToString(FormatString.DateTime)}', {this.Entity.DecimalProperty}, {this.Entity.DoubleProperty}, '{this.Entity.GuidProperty}', {this.Entity.IntProperty}, {this.Entity.IntProperty2}, '{this.Entity.NullableDateTimeOffsetProperty.Value.ToString(FormatString.DateTimeOffset)}', '{this.Entity.NullableDateTimeProperty.Value.ToString(FormatString.DateTime)}', {this.Entity.ShortProperty}, {this.Entity.SingleProperty}, '{this.Entity.StringProperty}', {(int)this.Entity.TestEnumProperty});";
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
                .Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfWithCalledAfterFor()
        {
            this.AssumeTestEntityIsInitialised();
            this.Statement.With(e => e.ByteProperty, 1);
            this.Statement.Invoking(s => s.For(this.Entity))
                .Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfBuildCalledWithoutInitialisingStatement()
        {
            this.Statement.Invoking(s => s.Sql())
                .Should().Throw<InvalidOperationException>();
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
            ISqlConnectionProvider connectionProvider)
        {
            var statement =
                new InsertStatement<TestEntity>(statementExecutor, entityMapper, writablePropertyMatcher);
            statement.UseConnectionProvider(connectionProvider);
            return statement;
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