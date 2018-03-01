using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.SqlServer.Abstractions;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class UpdateStatementShould : SqlStatementTestBase<UpdateStatement<TestEntity>, int>
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
        public void ProduceCorrectDefaultTableSpecfication()
        {
            this.Statement.Set(e => e.IntProperty, 1)
                .Sql()
                .Should()
                .Contain(this.ExpectedTableSpecification("dbo", "TestEntity"));
        }

        [Test]
        public void ProduceCorrectNonDefaultTableSpecfication()
        {
            this.Statement.Set(e => e.IntProperty, 1, OtherValue, OtherValue)
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
        public void BuildCorrectSetClauseWithOnlyStringPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [StringProperty] = 'My Name'";
            this.Statement.Set(e => e.StringProperty, "My Name")
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyDateTimePropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [DateTimeProperty] = '{0}'";
            var now = DateTime.UtcNow;
            this.Statement.Set(e => e.DateTimeProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, now.ToString(FormatString.DateTime)));
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyNullableDateTimePropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [NullableDateTimeProperty] = '{0}'";
            var now = DateTime.UtcNow;
            this.Statement.Set(e => e.NullableDateTimeProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, now.ToString(FormatString.DateTime)));
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyDateTimeOffsetPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [DateTimeOffsetProperty] = '{0}'";
            var now = DateTimeOffset.UtcNow;
            this.Statement.Set(e => e.DateTimeOffsetProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, now.ToString(FormatString.DateTimeOffset)));
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyNullableDateTimeOffsetPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [NullableDateTimeOffsetProperty] = '{0}'";
            var now = DateTimeOffset.UtcNow;
            this.Statement.Set(e => e.NullableDateTimeOffsetProperty, now)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, now.ToString(FormatString.DateTimeOffset)));
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyIntPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [IntProperty] = 1";
            this.Statement.Set(e => e.IntProperty, 1)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyDoublePropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [DoubleProperty] = 1.01";
            this.Statement.Set(e => e.DoubleProperty, 1.01)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyDecimalPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [DecimalProperty] = 2.01";
            this.Statement.Set(e => e.DecimalProperty, 2.01M)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlySinglePropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [SingleProperty] = 1.01";
            this.Statement.Set(e => e.SingleProperty, 1.01)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyShortPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [ShortProperty] = 1";
            this.Statement.Set(e => e.ShortProperty, 1)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyBytePropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [ByteProperty] = 1";
            this.Statement.Set(e => e.ByteProperty, 1)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyGuidPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [GuidProperty] = '{0}'";
            var guid = Guid.NewGuid();
            this.Statement.Set(e => e.GuidProperty, guid)
                .Sql()
                .Should()
                .StartWith(string.Format(expected, guid));
        }

        [Test]
        public void BuildCorrectSetClauseWithOnlyTestEnumPropertySet()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [TestEnumProperty] = 1";
            this.Statement.Set(e => e.TestEnumProperty, TestEnum.One)
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void BuildCorrectSetClauseWithMultipleSets()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [IntProperty] = 1, [StringProperty] = 'My String'";
            this.Statement.Set(e => e.IntProperty, 1)
                .Set(e => e.StringProperty, "My String")
                .Sql()
                .Should()
                .StartWith(expected);
        }

        [Test]
        public void UserBuilderOnWhere()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Statement.Set(e => e.IntProperty, 1)
                .Where(expression);
            this.WhereClauseBuilder.Received()
                .Where(expression);
        }

        [Test]
        public void UserWhereBuilderOnAnd()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Statement.Set(e => e.IntProperty, 1)
                .Where(expression)
                .And(expression);
            this.WhereClauseBuilder.Received()
                .And(expression);
        }

        [Test]
        public void UserWhereBuilderOnOr()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Statement.Set(e => e.IntProperty, 1)
                .Where(expression)
                .Or(expression);
            this.WhereClauseBuilder.Received()
                .Or(expression);
        }

        [Test]
        public void UserWhereBuilderOnNestedAnd()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Statement.Set(e => e.IntProperty, 1)
                .Where(expression)
                .NestedAnd(expression);
            this.WhereClauseBuilder.Received()
                .NestedAnd(expression);
        }

        [Test]
        public void UserWhereBuilderOnNestedOr()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 5;
            this.Statement.Set(e => e.IntProperty, 1)
                .Where(expression)
                .NestedOr(expression);
            this.WhereClauseBuilder.Received()
                .NestedOr(expression);
        }

        [Test]
        public void UseBuilderOnBuild()
        {
            this.Statement.Set(e => e.IntProperty, 1)
                .Set(e => e.StringProperty, "My String")
                .Where(e => e.Id == 55)
                .Sql();
            this.WhereClauseBuilder.Received()
                .Sql();
        }

        [Test]
        public void EmbedWhereClauseFromBuilderInStatement()
        {
            const string whereClause = "WHERE [Id] = 55";
            const string expected =
                "UPDATE [dbo].[TestEntity]\nSET [IntProperty] = 1, [StringProperty] = 'My String'\n" + whereClause + ";";
            this.WhereClauseBuilder.Sql()
                .Returns(whereClause);
            var result = this.Statement.Set(e => e.IntProperty, 1)
                             .Set(e => e.StringProperty, "My String")
                             .Where(e => e.Id == 55)
                             .Sql();
            result.Should()
                  .Be(expected);
        }

        [Test]
        [Ignore("Obsolete")]
        public void BuildCorrectStatementFromEntity()
        {
            this.AssumeWhereClauseBuilderReportsClean();
            this.AssumeTestEntityIsInitialised();
            var expected =
                $"UPDATE [dbo].[TestEntity]\nSET [DateTimeOffsetProperty] = '{this.Entity.DateTimeOffsetProperty.ToString(FormatString.DateTimeOffset)}', [NullableDateTimeOffsetProperty] = '{this.Entity.NullableDateTimeOffsetProperty.GetValueOrDefault().ToString(FormatString.DateTimeOffset)}', [DateTimeProperty] = '{this.Entity.DateTimeProperty.ToString(FormatString.DateTime)}', [NullableDateTimeProperty] = '{this.Entity.NullableDateTimeProperty.GetValueOrDefault().ToString(FormatString.DateTime)}', [DoubleProperty] = {this.Entity.DoubleProperty}, [IntProperty] = {this.Entity.IntProperty}, [IntProperty2] = {this.Entity.IntProperty2}, [StringProperty] = '{this.Entity.StringProperty}', [TestEnumProperty] = {(int)this.Entity.TestEnumProperty}, [DecimalProperty] = {this.Entity.DecimalProperty}, [ByteProperty] = {this.Entity.ByteProperty}, [ShortProperty] = {this.Entity.ShortProperty}, [SingleProperty] = {this.Entity.SingleProperty}, [GuidProperty] = '{this.Entity.GuidProperty}'\nWHERE [Id] = {this.Entity.Id};";
            this.Statement.For(this.Entity)
                .Sql()
                .Should()
                .Be(expected);
        }
        
        [Test]
        public void GenerateCorrectSqlForWhereInQuery()
        {
            const string whereClause = "WHERE ([dbo].[TestEntity].[IntProperty] IN (1, 2, 3))";
            this.WhereClauseBuilder.Sql()
                .Returns(whereClause);

            const string ExpectedSql = "UPDATE [dbo].[TestEntity]" + "\nSET [StringProperty] = 'TestValue'"
                                       + "\n" + whereClause + ";";
            this.Statement
                .Set(p => p.StringProperty, "TestValue")
                .WhereIn(e => e.IntProperty, new[] { 1, 2, 3 })
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void ThrowExceptionIfBuildCalledWithoutInitialisingStatement()
        {
            this.Statement.Invoking(s => s.Sql())
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfForCalledAfterWith()
        {
            this.AssumeTestEntityIsInitialised();
            this.AssumeWhereClauseBuilderReportsClean();
            this.Statement
                .For(this.Entity);
            this.Statement.Invoking(s => s.Set(e => e.ByteProperty, 1))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfWithCalledAfterFor()
        {
            this.AssumeTestEntityIsInitialised();
            this.Statement.Set(e => e.ByteProperty, 1);
            this.Statement.Invoking(s => s.For(this.Entity))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfForCalledAfterWhere()
        {
            this.AssumeTestEntityIsInitialised();
            this.Statement.Where(e => e.ByteProperty == 1);
            this.Statement.Invoking(s => s.For(this.Entity))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfWhereCalledAfterFor()
        {
            this.AssumeWhereClauseBuilderReportsClean();
            this.AssumeTestEntityIsInitialised();
            this.Statement.For(this.Entity);
            this.Statement.Invoking(s => s.Where(e => e.ByteProperty == 1))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ThrowExceptionIfWhereInCalledAfterFor()
        {
            this.AssumeWhereClauseBuilderReportsClean();
            this.AssumeTestEntityIsInitialised();
            this.Statement.For(this.Entity);
            var intArray = new []{ 1, 2};
            this.Statement.Invoking(s => s.WhereIn(e => e.IntProperty, intArray))
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ExecuteQueryOnGo()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [StringProperty] = 'My Name';";
            this.AssumeGoIsRequested();
            this.StatementExecutor.Received()
                .ExecuteNonQuery(expected);
        }

        [Test]
        public async Task ExecuteQueryOnGoAsync()
        {
            const string expected = "UPDATE [dbo].[TestEntity]\nSET [StringProperty] = 'My Name';";
            await this.AssumeGoAsyncIsRequested();
            await this.StatementExecutor.Received()
                .ExecuteNonQueryAsync(expected);
        }

        protected override UpdateStatement<TestEntity> CreateStatement(IStatementExecutor statementExecutor,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher,
            ISelectClauseBuilder selectClauseBuilder,
            IFromClauseBuilder fromClauseBuilder,
            IWhereClauseBuilder whereClauseBuilder,
            string connectionString)
        {
            var command = new UpdateStatement<TestEntity>(statementExecutor,
                entityMapper,
                writablePropertyMatcher,
                whereClauseBuilder);
            command.UseConnectionString(connectionString);
            return command;
        }

        private void AssumeGoIsRequested()
        {
            this.Statement.Set(e => e.StringProperty, "My Name")
                .Go();
        }

        private async Task AssumeGoAsyncIsRequested()
        {
            await this.Statement.Set(e => e.StringProperty, "My Name")
                .GoAsync();
        }

        private string ExpectedTableSpecification(string schema, string table)
        {
            return string.Format("UPDATE [{0}].[{1}]\nSET", schema, table);
        }
    }
}