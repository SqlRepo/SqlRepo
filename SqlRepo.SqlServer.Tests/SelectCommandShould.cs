using System;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class SelectCommandShould : SelectCommandTestBase
    {
        [Test]
        public void BeCleanByDefault()
        {
            this.Command.IsClean.Should()
                .BeTrue();
        }

        [Test]
        public void ExecuteQueryOnGo()
        {
            this.AssumeGoIsRequested();
            this.CommandExecutor.Received()
                .ExecuteReader(ConnectionString, Arg.Any<string>());
        }

        [Test]
        public async Task ExecuteQueryOnGoAsyTask()
        {
            await this.AssumeGoAsyncIsRequested();
            await this.CommandExecutor.Received()
                .ExecuteReaderAsync(ConnectionString, Arg.Any<string>());
        }

        [Test]
        public void GenerateCorrectSqlForDefaultQuery()
        {
            const string ExpectedSql = "SELECT *\nFROM [dbo].[TestEntity];";
            this.Command.Sql()
                .Should()
                .Be(ExpectedSql);
        }


        [Test]
        public void GenerateCorrectSqlForTop()
        {
            const string ExpectedSql = "SELECT TOP (50) *\nFROM [dbo].[TestEntity];";
            this.Command
                .Top(50)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForJoinedQueryWithNoLocks()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]" + "\nWITH ( NOLOCK )"
                                       + "\nINNER JOIN [dbo].[InnerEntity]" + "\nWITH ( NOLOCK )"
                                       + "\nON [dbo].[TestEntity].[IntProperty] = [dbo].[InnerEntity].[IntProperty];";
            this.Command.NoLocks()
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
            this.Command.NoLocks()
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
            this.Command.Select(e => e.IntProperty, null, e => e.StringProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForMultipleColumnSelectWithAlias()
        {
            const string ExpectedSql = "SELECT [a].[IntProperty]" + "\n, [a].[StringProperty]"
                                       + "\nFROM [dbo].[TestEntity] AS [a];";
            this.Command.Select(e => e.IntProperty, "a", e => e.StringProperty)
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
            this.Command.GroupBy(e => e.IntProperty, null, e => e.StringProperty)
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
            this.Command.From("a")
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
            this.Command.OrderBy(e => e.IntProperty, null, e => e.StringProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForIsNotNullProperty()
        {
            const string ExpectedSql = "WHERE ([dbo].[TestEntity].[DateTimeProperty] IS NOT NULL)";
            this.Command.Where(e => e.DateTimeProperty != null)
                .Sql()
                .Should()
                .Contain(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForNullProperty()
        {
            const string ExpectedSql = "WHERE ([dbo].[TestEntity].[DateTimeProperty] IS NULL)";
            this.Command.Where(e => e.DateTimeProperty == null)
                .Sql()
                .Should()
                .Contain(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleGroupByQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nGROUP BY [dbo].[TestEntity].[IntProperty];";
            this.Command.GroupBy(e => e.IntProperty)
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
            this.Command.From(Alias)
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
            this.Command.InnerJoin<InnerEntity>()
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
            this.Command.From("a")
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
            this.Command.SelectAll("a")
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
            this.Command.SelectAll<InnerEntity>("b")
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
            this.Command.LeftOuterJoin<InnerEntity>()
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
            this.Command.From("a")
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
            this.Command.OrderByDescending(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleOrderByQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nORDER BY [dbo].[TestEntity].[IntProperty] ASC;";
            this.Command.OrderBy(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleOrderByQueryWithAlias()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity] AS [a]"
                                       + "\nORDER BY [a].[IntProperty] ASC;";
            this.Command.From("a")
                .OrderBy(e => e.IntProperty, "a")
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleQueryWithNoLocks()
        {
            const string ExpectedSql = "SELECT *\nFROM [dbo].[TestEntity]\nWITH ( NOLOCK );";
            this.Command.NoLocks()
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleQueryWithNoLocksAndAlias()
        {
            const string ExpectedSql = "SELECT *\nFROM [dbo].[TestEntity] AS [a]\nWITH ( NOLOCK );";
            this.Command.From("a")
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
            this.Command.RightOuterJoin<InnerEntity>()
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
            this.Command.From("a")
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
            this.Command.Where(e => e.IntProperty == 1)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSimpleWhereQueryWithAlias()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity] AS [a]"
                                       + "\nWHERE ([a].[IntProperty] = 1);";
            this.Command.From("a")
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
            this.Command.Avg(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSingleColumnAggregateWithAlias()
        {
            const string ExpectedSql = "SELECT SUM([a].[IntProperty]) AS [IntProperty]"
                                       + "\nFROM [dbo].[TestEntity] AS [a];";
            this.Command.Sum(e => e.IntProperty, "a")
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
            this.Command.Select(e => e.IntProperty)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForSingleColumnSelectWithAlias()
        {
            const string ExpectedSql = "SELECT [a].[IntProperty]" + "\nFROM [dbo].[TestEntity] AS [a];";
            this.Command.Select(e => e.IntProperty, "a")
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
            this.Command.Select(e => e.Id)
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
            this.Command.Where(e => e.IntProperty == 1)
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
            this.Command.Where(e => e.IntProperty == 1)
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
            this.Command.WhereBetween(e => e.IntProperty, 5, 10)
                .Sql()
                .Should()
                .Be(ExpectedSql);
        }

        [Test]
        public void GenerateCorrectSqlForWhereInQuery()
        {
            const string ExpectedSql = "SELECT *" + "\nFROM [dbo].[TestEntity]"
                                       + "\nWHERE ([dbo].[TestEntity].[IntProperty] IN (1, 2, 3));";
            this.Command.WhereIn(e => e.IntProperty, new[] {1, 2, 3})
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
            this.Command.Where(e => e.IntProperty == 1)
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
            this.Command.Where(e => e.IntProperty == 1)
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
            this.Command.Specification.Should()
                .NotBeNull();
        }

        private void AssumeGoIsRequested()
        {
            this.Command.Go();
        }

        private async Task AssumeGoAsyncIsRequested()
        {
            await this.Command.GoAsync();
        }
    }
}