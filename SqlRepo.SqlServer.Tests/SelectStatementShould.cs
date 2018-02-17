using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class SelectStatementShould : SelectStatementTestBase
    {
        [Test]
        public void BeCleanByDefault()
        {
            this.Statement.IsClean.Should()
                .BeTrue();
        }

        [Test]
        public void ExecuteQueryOnGo()
        {
            this.AssumeGoIsRequested();
            this.StatementExecutor.Received()
                .ExecuteReader(Arg.Any<string>());
        }

        [Test]
        public async Task ExecuteQueryOnGoAsyTask()
        {
            await this.AssumeGoAsyncIsRequested();
            await this.StatementExecutor.Received()
                .ExecuteReaderAsync(Arg.Any<string>());
        }

        [Test]
        public void GenerateCorrectSqlForDefaultQuery()
        {
            const string ExpectedSql = "SELECT *\nFROM [dbo].[TestEntity];";
            this.Statement.Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForJoinedQueryWithNoLocks()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]" + "\nWITH ( NOLOCK )"
                                       + "\nINNER JOIN [dbo].[InnerEntity]" + "\nWITH ( NOLOCK )"
                                       + "\nON [dbo].[TestEntity].[IntProperty] = [dbo].[InnerEntity].[IntProperty];";
            this.Statement.NoLocks()
                .InnerJoin<InnerEntity>()
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForJoinedQueryWithNoLocksAndAliases()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity] AS [a]" + "\nWITH ( NOLOCK )"
                                       + "\nINNER JOIN [dbo].[InnerEntity] AS [b]" + "\nWITH ( NOLOCK )"
                                       + "\nON [a].[IntProperty] = [b].[IntProperty];";
            this.Statement.NoLocks()
                .From("a")
                .InnerJoin<InnerEntity>("b")
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty, "a", "b")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForMultipleColumnSelect()
        {
            const string ExpectedSql = "SELECT [dbo].[TestEntity].[IntProperty]"
                                       + "\n, [dbo].[TestEntity].[StringProperty]"
                                       + "\nFROM [dbo].[TestEntity];";
            this.Statement.Select(e => e.IntProperty, null, e => e.StringProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForMultipleColumnSelectWithAlias()
        {
            const string ExpectedSql = "SELECT [a].[IntProperty]" + "\n, [a].[StringProperty]"
                                       + "\nFROM [dbo].[TestEntity] AS [a];";
            this.Statement.Select(e => e.IntProperty, "a", e => e.StringProperty)
                .From("a")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForMultipleGroupByQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nGROUP BY [dbo].[TestEntity].[IntProperty]"
                                       + "\n, [dbo].[TestEntity].[StringProperty];";
            this.Statement.GroupBy(e => e.IntProperty, null, e => e.StringProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForMultipleInnerJoinQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity] AS [a]"
                                       + "\nINNER JOIN [dbo].[InnerEntity] AS [b]"
                                       + "\nON [a].[IntProperty] = [b].[IntProperty]"
                                       + "\nINNER JOIN [dbo].[InnerEntity] AS [c]"
                                       + "\nON [a].[IntProperty] = [c].[IntProperty];";
            this.Statement.From("a")
                .InnerJoin<InnerEntity>("b")
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty, "a", "b")
                .InnerJoin<InnerEntity>("c")
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty, "a", "c")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForMultipleOrderByQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nORDER BY [dbo].[TestEntity].[IntProperty] ASC"
                                       + "\n, [dbo].[TestEntity].[StringProperty] ASC;";
            this.Statement.OrderBy(e => e.IntProperty, null, e => e.StringProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForIsNotNullProperty()
        {
            const string ExpectedSql = "WHERE ([dbo].[TestEntity].[DateTimeProperty] IS NOT NULL)";
            this.Statement.Where(e => e.DateTimeProperty != null)
                .Sql()
                .Should()
                .Contain(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForNullProperty()
        {
            const string ExpectedSql = "WHERE ([dbo].[TestEntity].[DateTimeProperty] IS NULL)";
            this.Statement.Where(e => e.DateTimeProperty == null)
                .Sql()
                .Should()
                .Contain(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleGroupByQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nGROUP BY [dbo].[TestEntity].[IntProperty];";
            this.Statement.GroupBy(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleGroupByQueryWithAlias()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity] AS [a]"
                                       + "\nGROUP BY [a].[IntProperty];";
            const string Alias = "a";
            this.Statement.From(Alias)
                .GroupBy(e => e.IntProperty, Alias)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleInnerJoinQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nINNER JOIN [dbo].[InnerEntity]"
                                       + "\nON [dbo].[TestEntity].[IntProperty] = [dbo].[InnerEntity].[IntProperty];";
            this.Statement.InnerJoin<InnerEntity>()
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleInnerJoinQueryWithAlias()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity] AS [a]"
                                       + "\nINNER JOIN [dbo].[InnerEntity] AS [b]"
                                       + "\nON [a].[IntProperty] = [b].[IntProperty];";
            this.Statement.From("a")
                .InnerJoin<InnerEntity>("b")
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty, "a", "b")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleInnerJoinQueryWithAliasedSelectAllOfLeftTable()
        {
            const string ExpectedSql = "SELECT [a].*" + "\nFROM [dbo].[TestEntity] AS [a]"
                                       + "\nINNER JOIN [dbo].[InnerEntity] AS [b]"
                                       + "\nON [a].[IntProperty] = [b].[IntProperty];";
            this.Statement.SelectAll("a")
                .From("a")
                .InnerJoin<InnerEntity>("b")
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty, "a", "b")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleInnerJoinQueryWithAliasedSelectAllOfRightTable()
        {
            const string ExpectedSql = "SELECT [b].*" + "\nFROM [dbo].[TestEntity] AS [a]"
                                       + "\nINNER JOIN [dbo].[InnerEntity] AS [b]"
                                       + "\nON [a].[IntProperty] = [b].[IntProperty];";
            this.Statement.SelectAll<InnerEntity>("b")
                .From("a")
                .InnerJoin<InnerEntity>("b")
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty, "a", "b")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleLeftOuterJoinQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nLEFT OUTER JOIN [dbo].[InnerEntity]"
                                       + "\nON [dbo].[TestEntity].[IntProperty] = [dbo].[InnerEntity].[IntProperty];";
            this.Statement.LeftOuterJoin<InnerEntity>()
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleLeftOuterJoinQueryWithAlias()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity] AS [a]"
                                       + "\nLEFT OUTER JOIN [dbo].[InnerEntity] AS [b]"
                                       + "\nON [a].[IntProperty] = [b].[IntProperty];";
            this.Statement.From("a")
                .LeftOuterJoin<InnerEntity>("b")
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty, "a", "b")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleOrderByDescendingQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nORDER BY [dbo].[TestEntity].[IntProperty] DESC;";
            this.Statement.OrderByDescending(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleOrderByQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nORDER BY [dbo].[TestEntity].[IntProperty] ASC;";
            this.Statement.OrderBy(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleOrderByQueryWithAlias()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity] AS [a]"
                                       + "\nORDER BY [a].[IntProperty] ASC;";
            this.Statement.From("a")
                .OrderBy(e => e.IntProperty, "a")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleQueryWithNoLocks()
        {
            const string ExpectedSql = "SELECT *\nFROM [dbo].[TestEntity]\nWITH ( NOLOCK );";
            this.Statement.NoLocks()
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleQueryWithNoLocksAndAlias()
        {
            const string ExpectedSql = "SELECT *\nFROM [dbo].[TestEntity] AS [a]\nWITH ( NOLOCK );";
            this.Statement.From("a")
                .NoLocks()
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleRightOuterJoinQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nRIGHT OUTER JOIN [dbo].[InnerEntity]"
                                       + "\nON [dbo].[TestEntity].[IntProperty] = [dbo].[InnerEntity].[IntProperty];";
            this.Statement.RightOuterJoin<InnerEntity>()
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleRightOuterJoinQueryWithAlias()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity] AS [a]"
                                       + "\nRIGHT OUTER JOIN [dbo].[InnerEntity] AS [b]"
                                       + "\nON [a].[IntProperty] = [b].[IntProperty];";
            this.Statement.From("a")
                .RightOuterJoin<InnerEntity>("b")
                .On<InnerEntity>((l, r) => l.IntProperty == r.IntProperty, "a", "b")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleWhereQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nWHERE ([dbo].[TestEntity].[IntProperty] = 1);";
            this.Statement.Where(e => e.IntProperty == 1)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleWhereQueryWithAlias()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity] AS [a]"
                                       + "\nWHERE ([a].[IntProperty] = 1);";
            this.Statement.From("a")
                .Where(e => e.IntProperty == 1, "a")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSingleAggregateColumnSelect()
        {
            const string ExpectedSql = "SELECT AVG([dbo].[TestEntity].[IntProperty]) AS [IntProperty]"
                                       + "\nFROM [dbo].[TestEntity];";
            this.Statement.Avg(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSingleColumnAggregateWithAlias()
        {
            const string ExpectedSql = "SELECT SUM([a].[IntProperty]) AS [IntProperty]"
                                       + "\nFROM [dbo].[TestEntity] AS [a];";
            this.Statement.Sum(e => e.IntProperty, "a")
                .From("a")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSingleColumnSelect()
        {
            const string ExpectedSql = "SELECT [dbo].[TestEntity].[IntProperty]"
                                       + "\nFROM [dbo].[TestEntity];";
            this.Statement.Select(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSingleColumnSelectWithAlias()
        {
            const string ExpectedSql = "SELECT [a].[IntProperty]" + "\nFROM [dbo].[TestEntity] AS [a];";
            this.Statement.Select(e => e.IntProperty, "a")
                .From("a")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSingleHavingFilter()
        {
            const string ExpectedSql = "SELECT [dbo].[TestEntity].[Id]"
                                       + "\n, AVG([dbo].[TestEntity].[IntProperty]) AS [IntProperty]"
                                       + "\nFROM [dbo].[TestEntity]" + "\nGROUP BY [dbo].[TestEntity].[Id]"
                                       + "\nHAVING AVG([dbo].[TestEntity].[IntProperty]) > 5;";
            this.Statement.Select(e => e.Id)
                .Avg(e => e.IntProperty)
                .GroupBy(e => e.Id)
                .HavingAvg(e => e.IntProperty > 5)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForWhereAndNestedQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nWHERE ([dbo].[TestEntity].[IntProperty] = 1"
                                       + "\nAND ([dbo].[TestEntity].[StringProperty] = 'something'));";
            this.Statement.Where(e => e.IntProperty == 1)
                .NestedAnd(e => e.StringProperty == "something")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForWhereAndQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nWHERE ([dbo].[TestEntity].[IntProperty] = 1"
                                       + "\nAND [dbo].[TestEntity].[StringProperty] = 'something');";
            this.Statement.Where(e => e.IntProperty == 1)
                .And(e => e.StringProperty == "something")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForWhereBetweenQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nWHERE ([dbo].[TestEntity].[IntProperty] >= 5"
                                       + "\nAND [dbo].[TestEntity].[IntProperty] <= 10);";
            this.Statement.WhereBetween(e => e.IntProperty, 5, 10)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForWhereInQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nWHERE ([dbo].[TestEntity].[IntProperty] IN (1, 2, 3));";
            this.Statement.WhereIn(e => e.IntProperty, new[] {1, 2, 3})
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForWhereOrNestedQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nWHERE ([dbo].[TestEntity].[IntProperty] = 1"
                                       + "\nOR ([dbo].[TestEntity].[StringProperty] = 'something'));";
            this.Statement.Where(e => e.IntProperty == 1)
                .NestedOr(e => e.StringProperty == "something")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForWhereOrQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nWHERE ([dbo].[TestEntity].[IntProperty] = 1"
                                       + "\nOR [dbo].[TestEntity].[StringProperty] = 'something');";
            this.Statement.Where(e => e.IntProperty == 1)
                .Or(e => e.StringProperty == "something")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void MapResultFromExecution()
        {
            this.AssumeGoIsRequested();
            this.EntityMapper.Received()
                .Map<TestEntity>(this.DataReader);
        }

        [Test]
        public void ProvideCurrentConfigurationOnRequest()
        {
            this.Statement.Specification.Should()
                .NotBeNull();
        }

        private void AssumeGoIsRequested()
        {
            this.Statement.Go();
        }

        private async Task AssumeGoAsyncIsRequested()
        {
            await this.Statement.GoAsync();
        }
    }
}