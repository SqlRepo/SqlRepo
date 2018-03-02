using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class SelectStatement<TEntity> : SqlStatement<TEntity, IEnumerable<TEntity>>, ISelectStatement<TEntity>
        where TEntity: class, new()
    {
        private FilterGroup currentFilterGroup;
        private FilterGroup rootFilterGroup;

        public SelectStatement(IStatementExecutor statementExecutor, IEntityMapper entityMapper)
            : base(statementExecutor, entityMapper)
        {
            this.InitialiseConfig();
        }

        public SelectStatementSpecification Specification { get; private set; }

        public ISelectStatement<TEntity> And<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            this.ThrowIfFilteringNotInitialised();
            this.AddFilterCondition(selector, @alias, LogicalOperator.And);
            return this;
        }

        public ISelectStatement<TEntity> And(Expression<Func<TEntity, bool>> selector, string @alias = null)
        {
            return this.And<TEntity>(selector, @alias);
        }

        public ISelectStatement<TEntity> AndBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string @alias = null)
        {
            this.ThrowIfFilteringNotInitialised();
            this.AddBetweenFilterCondition(selector, start, end, alias, LogicalOperator.And);
            return this;
        }

        public ISelectStatement<TEntity> AndBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string @alias = null)
        {
            return this.AndBetween<TEntity, TMember>(selector, start, end, @alias);
        }

        public ISelectStatement<TEntity> AndIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string @alias = null)
        {
            this.ThrowIfFilteringNotInitialised();
            this.AddInFilterCondition(selector, values, alias, LogicalOperator.And);
            return this;
        }

        public ISelectStatement<TEntity> AndIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string @alias = null)
        {
            return this.AndIn<TEntity, TMember>(selector, values, @alias);
        }

        public ISelectStatement<TEntity> AndOn<TRight>(Expression<Func<TEntity, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null)
        {
            return this.AndOn<TEntity, TRight>(expression, leftTableAlias, rightTableAlias);
        }

        public ISelectStatement<TEntity> AndOn<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null)
        {
            if(this.Specification.Tables.Count < 2)
            {
                throw new InvalidOperationException(
                    "On cannot be used before initialising a join with one of the Join methods.");
            }

            this.AddJoinCondition(expression, leftTableAlias, rightTableAlias, LogicalOperator.And);
            return this;
        }

        public ISelectStatement<TEntity> Avg(Expression<Func<TEntity, object>> selector, string @alias = null)
        {
            return this.Avg<TEntity>(selector, @alias);
        }

        public ISelectStatement<TEntity> Avg<T>(Expression<Func<T, object>> selector, string @alias = null)
        {
            this.AddColumnSelection<T>(this.GetMemberName(selector), @alias, Aggregation.Avg);
            return this;
        }

        public ISelectStatement<TEntity> Count(Expression<Func<TEntity, object>> selector, string @alias = null)
        {
            return this.Count<TEntity>(selector, @alias);
        }

        public ISelectStatement<TEntity> Count<T>(Expression<Func<T, object>> selector, string @alias = null)
        {
            this.AddColumnSelection<T>(this.GetMemberName(selector), @alias, Aggregation.Count);
            return this;
        }

        public ISelectStatement<TEntity> CountAll()
        {
            this.Specification.Columns.Add(new ColumnSpecification
                                           {
                                               Identifier = "*",
                                               Aggregation = Aggregation.Count,
                                               EntityType = typeof(TEntity)
                                           });
            return this;
        }

        public ISelectStatement<TEntity> EndNesting()
        {
            this.currentFilterGroup = this.currentFilterGroup.Parent;
            return this;
        }

        public ISelectStatement<TEntity> From(string @alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            var rootTable = this.Specification.Tables.First();
            if(!string.IsNullOrEmpty(alias))
            {
                rootTable.Alias = alias;
            }

            if(!string.IsNullOrEmpty(tableName))
            {
                rootTable.TableName = tableName;
            }

            if(!string.IsNullOrEmpty(tableSchema))
            {
                rootTable.Schema = tableSchema;
            }
            return this;
        }

        public override IEnumerable<TEntity> Go()
        {
            using(var reader = this.StatementExecutor.ExecuteReader(this.Sql()))
            {
                return this.EntityMapper.Map<TEntity>(reader);
            }
        }

        public override async Task<IEnumerable<TEntity>> GoAsync()
        {
            using (var reader = await this.StatementExecutor.ExecuteReaderAsync(this.Sql()))
            {
                return this.EntityMapper.Map<TEntity>(reader);
            }
        }

        public ISelectStatement<TEntity> GroupBy<T>(Expression<Func<T, object>> selector,
            string @alias = null,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            this.ThrowIfTableNotSpecified<T>(@alias);
            this.AddGroupSpecification(selector, @alias);
            foreach(var additionalSelector in additionalSelectors)
            {
                this.AddGroupSpecification(additionalSelector, @alias);
            }
            return this;
        }

        public ISelectStatement<TEntity> GroupBy(Expression<Func<TEntity, object>> selector,
            string @alias = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return this.GroupBy<TEntity>(selector, @alias, additionalSelectors);
        }

        public ISelectStatement<TEntity> HavingAvg<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            this.ThrowIfGroupingNotInitialised();
            this.AddHavingSpecification(selector, Aggregation.Avg, alias);
            return this;
        }

        public ISelectStatement<TEntity> HavingAvg(Expression<Func<TEntity, bool>> selector,
            string @alias = null)
        {
            return this.HavingAvg<TEntity>(selector, @alias);
        }

        public ISelectStatement<TEntity> HavingCount<T>(Expression<Func<T, bool>> selector,
            string @alias = null)
        {
            this.ThrowIfGroupingNotInitialised();
            this.AddHavingSpecification(selector, Aggregation.Count, alias);
            return this;
        }

        public ISelectStatement<TEntity> HavingCount(Expression<Func<TEntity, bool>> selector,
            string @alias = null)
        {
            return this.HavingCount<TEntity>(selector, @alias);
        }

        public ISelectStatement<TEntity> HavingCountAll<T>(Comparison comparison, int @value)
        {
            this.ThrowIfGroupingNotInitialised();
            this.Specification.Havings.Add(new SelectStatementHavingSpecification
                                           {
                                               Aggregation = Aggregation
                                                   .Count,
                                               EntityType = typeof(T),
                                               Identifier = "*",
                                               Operator = this
                                                   .OperatorStringFromComparison(
                                                       comparison),
                                               Value = this.FormatValue(
                                                   @value)
                                           });
            return this;
        }

        public ISelectStatement<TEntity> HavingCountAll(Comparison comparison, int @value)
        {
            return this.HavingCountAll<TEntity>(comparison, @value);
        }

        public ISelectStatement<TEntity> HavingMax<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            this.ThrowIfGroupingNotInitialised();
            this.AddHavingSpecification(selector, Aggregation.Max, alias);
            return this;
        }

        public ISelectStatement<TEntity> HavingMax(Expression<Func<TEntity, bool>> selector,
            string @alias = null)
        {
            return this.HavingMax<TEntity>(selector, @alias);
        }

        public ISelectStatement<TEntity> HavingMin<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            this.ThrowIfGroupingNotInitialised();
            this.AddHavingSpecification(selector, Aggregation.Min, alias);
            return this;
        }

        public ISelectStatement<TEntity> HavingMin(Expression<Func<TEntity, bool>> selector,
            string @alias = null)
        {
            return this.HavingMin<TEntity>(selector, @alias);
        }

        public ISelectStatement<TEntity> HavingSum<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            this.ThrowIfGroupingNotInitialised();
            this.AddHavingSpecification(selector, Aggregation.Sum, alias);
            return this;
        }

        public ISelectStatement<TEntity> HavingSum(Expression<Func<TEntity, bool>> selector,
            string @alias = null)
        {
            return this.HavingSum<TEntity>(selector, @alias);
        }

        public ISelectStatement<TEntity> InnerJoin<TRight>(string @alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.AddTableSpecification<TRight>(JoinType.Inner, @alias, tableName, tableSchema);
            return this;
        }

        public ISelectStatement<TEntity> LeftOuterJoin<TRight>(string @alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.AddTableSpecification<TRight>(JoinType.LeftOuter, alias, tableName, tableSchema);
            return this;
        }

        public ISelectStatement<TEntity> Max(Expression<Func<TEntity, object>> selector, string @alias = null)
        {
            return this.Max<TEntity>(selector, @alias);
        }

        public ISelectStatement<TEntity> Max<T>(Expression<Func<T, object>> selector, string @alias = null)
        {
            this.AddColumnSelection<T>(this.GetMemberName(selector), @alias, Aggregation.Max);
            return this;
        }

        public ISelectStatement<TEntity> Min(Expression<Func<TEntity, object>> selector, string @alias = null)
        {
            return this.Min<TEntity>(selector, @alias);
        }

        public ISelectStatement<TEntity> Min<T>(Expression<Func<T, object>> selector, string @alias = null)
        {
            this.AddColumnSelection<T>(this.GetMemberName(selector), @alias, Aggregation.Min);
            return this;
        }

        public ISelectStatement<TEntity> NestedAnd<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            this.ThrowIfFilteringNotInitialised();
            this.AddNewFilterGroup(FilterGroupType.And);
            this.AddFilterCondition(selector, alias);
            return this;
        }

        public ISelectStatement<TEntity> NestedAnd(Expression<Func<TEntity, bool>> selector,
            string alias = null)
        {
            return this.NestedAnd<TEntity>(selector, alias);
        }

        public ISelectStatement<TEntity> NestedAndBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null)
        {
            this.ThrowIfFilteringNotInitialised();
            this.AddNewFilterGroup(FilterGroupType.And);
            this.AddBetweenFilterCondition(selector, start, end, alias);
            return this;
        }

        public ISelectStatement<TEntity> NestedAndBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null)
        {
            return this.NestedAndBetween<TEntity, TMember>(selector, start, end, @alias);
        }

        public ISelectStatement<TEntity> NestedAndIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null)
        {
            this.ThrowIfFilteringNotInitialised();
            this.AddNewFilterGroup(FilterGroupType.And);
            this.AddInFilterCondition(selector, values, alias);
            return this;
        }

        public ISelectStatement<TEntity> NestedAndIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null)
        {
            return this.NestedAndIn<TEntity, TMember>(selector, values, @alias);
        }

        public ISelectStatement<TEntity> NestedOr<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            this.ThrowIfFilteringNotInitialised();
            this.AddNewFilterGroup(FilterGroupType.Or);
            this.AddFilterCondition(selector, alias);
            return this;
        }

        public ISelectStatement<TEntity> NestedOr(Expression<Func<TEntity, bool>> selector, string alias = null)
        {
            return this.NestedOr<TEntity>(selector, alias);
        }

        public ISelectStatement<TEntity> NestedOrBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null)
        {
            this.ThrowIfFilteringNotInitialised();
            this.AddNewFilterGroup(FilterGroupType.Or);
            this.AddBetweenFilterCondition(selector, start, end, alias);
            return this;
        }

        public ISelectStatement<TEntity> NestedOrBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null)
        {
            return this.NestedOrBetween<TEntity, TMember>(selector, start, end, alias);
        }

        public ISelectStatement<TEntity> NestedOrIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null)
        {
            this.ThrowIfFilteringNotInitialised();
            this.AddNewFilterGroup(FilterGroupType.Or);
            this.AddInFilterCondition(selector, values, alias);
            return this;
        }

        public ISelectStatement<TEntity> NestedOrIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null)
        {
            return this.NestedOrIn<TEntity, TMember>(selector, values, alias);
        }

        public ISelectStatement<TEntity> NoLocks()
        {
            this.Specification.NoLocks = true;
            return this;
        }

        public ISelectStatement<TEntity> On<TRight>(Expression<Func<TEntity, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null)
        {
            return this.On<TEntity, TRight>(expression, leftTableAlias, rightTableAlias);
        }

        public ISelectStatement<TEntity> On<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null)
        {
            if(this.Specification.Tables.Count < 2)
            {
                throw new InvalidOperationException(
                    "On cannot be used before initialising a join with one of the Join methods.");
            }

            this.AddJoinCondition(expression, leftTableAlias, rightTableAlias);
            return this;
        }

        public ISelectStatement<TEntity> Or<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            this.ThrowIfFilteringNotInitialised();
            this.AddFilterCondition(selector, @alias, LogicalOperator.Or);
            return this;
        }

        public ISelectStatement<TEntity> Or(Expression<Func<TEntity, bool>> selector, string @alias = null)
        {
            return this.Or<TEntity>(selector, @alias);
        }

        public ISelectStatement<TEntity> OrBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string @alias = null)
        {
            this.ThrowIfFilteringNotInitialised();
            this.AddBetweenFilterCondition(selector, start, end, alias, LogicalOperator.Or);
            return this;
        }

        public ISelectStatement<TEntity> OrBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string @alias = null)
        {
            return this.OrBetween<TEntity, TMember>(selector, start, end, @alias);
        }

        public ISelectStatement<TEntity> OrderBy<T>(Expression<Func<T, object>> selector,
            string alias = null,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            this.ThrowIfTableNotSpecified<T>(@alias);
            this.AddOrderSpecification(selector, alias, OrderByDirection.Ascending, additionalSelectors);
            return this;
        }

        public ISelectStatement<TEntity> OrderBy(Expression<Func<TEntity, object>> selector,
            string @alias = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return this.OrderBy<TEntity>(selector, alias, additionalSelectors);
        }

        public ISelectStatement<TEntity> OrderByDescending<T>(Expression<Func<T, object>> selector,
            string @alias = null,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            this.ThrowIfTableNotSpecified<T>(@alias);
            this.AddOrderSpecification(selector, alias, OrderByDirection.Descending, additionalSelectors);
            return this;
        }

        public ISelectStatement<TEntity> OrderByDescending(Expression<Func<TEntity, object>> selector,
            string @alias = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return this.OrderByDescending<TEntity>(selector, alias, additionalSelectors);
        }

        public ISelectStatement<TEntity> OrIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string @alias = null)
        {
            this.ThrowIfFilteringNotInitialised();
            this.AddInFilterCondition(selector, values, @alias, LogicalOperator.Or);
            return this;
        }

        public ISelectStatement<TEntity> OrIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string @alias = null)
        {
            return this.OrIn<TEntity, TMember>(selector, values, @alias);
        }

        public ISelectStatement<TEntity> OrOn<TRight>(Expression<Func<TEntity, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null)
        {
            return this.OrOn<TEntity, TRight>(expression, leftTableAlias, rightTableAlias);
        }

        public ISelectStatement<TEntity> OrOn<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null)
        {
            if(this.Specification.Tables.Count < 2)
            {
                throw new InvalidOperationException(
                    "On cannot be used before initialising a join with one of the Join methods.");
            }

            this.AddJoinCondition(expression, leftTableAlias, rightTableAlias, LogicalOperator.Or);
            return this;
        }

        public ISelectStatement<TEntity> Percent(bool useTopPercent = true)
        {
            if(!this.Specification.Top.HasValue)
            {
                throw new InvalidOperationException("Please call Top to set a value before calling Percent");
            }

            this.Specification.UseTopPercent = useTopPercent;
            return this;
        }

        public ISelectStatement<TEntity> RightOuterJoin<TLeft>(string @alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.AddTableSpecification<TLeft>(JoinType.RightOuter, alias, tableName, tableSchema);
            return this;
        }

        public ISelectStatement<TEntity> Select(Expression<Func<TEntity, object>> selector,
            string @alias = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return this.Select<TEntity>(selector, @alias, additionalSelectors);
        }

        public ISelectStatement<TEntity> Select<T>(Expression<Func<T, object>> selector,
            string @alias = null,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            this.AddColumnSelection(selector, @alias, additionalSelectors);
            return this;
        }

        public ISelectStatement<TEntity> SelectAll(string @alias = null)
        {
            return this.SelectAll<TEntity>(alias);
        }

        public ISelectStatement<TEntity> SelectAll<T>(string @alias = null)
        {
            this.AddColumnSelection<T>("*", @alias);
            return this;
        }

        public override string Sql()
        {
            this.FinalizeColumnSpecifications();
            this.FinalizeJoinConditions();
            this.FinalizeWhereConditions(this.Specification.Filters);
            this.FinalizeGroupings();
            this.FinalizeOrderings();
            this.FinalizeHavings();
            return this.Specification.ToString();
        }

        public ISelectStatement<TEntity> Sum(Expression<Func<TEntity, object>> selector, string @alias = null)
        {
            return this.Sum<TEntity>(selector, @alias);
        }

        public ISelectStatement<TEntity> Sum<T>(Expression<Func<T, object>> selector, string @alias = null)
        {
            this.AddColumnSelection<T>(this.GetMemberName(selector), @alias, Aggregation.Sum);
            return this;
        }

        public ISelectStatement<TEntity> Top(int rows)
        {
            this.Specification.Top = rows;
            return this;
        }

        public ISelectStatement<TEntity> Where<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            this.ThrowIfTableNotSpecified<T>(@alias);
            this.InitialiseFiltering();
            this.AddFilterCondition(selector, @alias);
            return this;
        }

        public ISelectStatement<TEntity> Where(Expression<Func<TEntity, bool>> selector, string @alias = null)
        {
            return this.Where<TEntity>(selector, @alias);
        }

        public ISelectStatement<TEntity> WhereBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string @alias = null)
        {
            this.InitialiseFiltering();
            this.AddBetweenFilterCondition(selector, start, end, alias);
            return this;
        }

        public ISelectStatement<TEntity> WhereBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string @alias = null)
        {
            return this.WhereBetween<TEntity, TMember>(selector, start, end, @alias);
        }

        public ISelectStatement<TEntity> WhereIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string @alias = null)
        {
            this.InitialiseFiltering();
            this.AddInFilterCondition(selector, values, alias);
            return this;
        }

        public ISelectStatement<TEntity> WhereIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string @alias = null)
        {
            return this.WhereIn<TEntity, TMember>(selector, values, @alias);
        }

        private void AddBetweenFilterCondition<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string @alias = null,
            LogicalOperator logicalOperator = LogicalOperator.NotSet)
        {
            this.currentFilterGroup.Conditions.Add(new FilterCondition
                                                   {
                                                       Alias = @alias,
                                                       EntityType = typeof(T),
                                                       Operator = ">=",
                                                       Left = this
                                                           .GetMemberName(selector),
                                                       LocigalOperator = logicalOperator,
                                                       Right = this.FormatValue(start)
                                                   });
            this.currentFilterGroup.Conditions.Add(new FilterCondition
                                                   {
                                                       Alias = @alias,
                                                       EntityType = typeof(T),
                                                       Operator = "<=",
                                                       Left = this
                                                           .GetMemberName(selector),
                                                       LocigalOperator = LogicalOperator
                                                           .And,
                                                       Right = this.FormatValue(end)
                                                   });
        }

        private void AddColumnSelection<T>(Expression<Func<T, object>> selector,
            string @alias = null,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            this.AddColumnSelection<T>(this.GetMemberName(selector), @alias);
            foreach(var additionalSelector in additionalSelectors)
            {
                this.AddColumnSelection<T>(this.GetMemberName(additionalSelector), @alias);
            }
        }

        private void AddColumnSelection<T>(string name,
            string @alias = null,
            Aggregation aggregation = Aggregation.None)
        {
            this.Specification.Columns.Add(new ColumnSpecification
                                           {
                                               Aggregation = aggregation,
                                               Alias = alias,
                                               EntityType = typeof(T),
                                               Identifier = name
                                           });
        }

        private void AddFilterCondition<T>(Expression<Func<T, bool>> selector,
            string @alias = null,
            LogicalOperator logicalOperator = LogicalOperator.NotSet)
        {
            var binaryExpression = selector.Body as BinaryExpression;
            this.currentFilterGroup.Conditions.Add(new FilterCondition
                                                   {
                                                       Alias = @alias,
                                                       EntityType = typeof(T),
                                                       Left = this.GetMemberName(
                                                           binaryExpression.Left),
                                                       LocigalOperator = logicalOperator,
                                                       Operator = this.OperatorString(
                                                           binaryExpression.NodeType),
                                                       Right = this.FormatValue(this
                                                           .GetExpressionValue(selector))
                                                   });
        }

        private void AddGroupSpecification<T>(Expression<Func<T, object>> selector,
            string alias = null,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            var entityType = typeof(T);
            this.Specification.Groupings.Add(new GroupSpecification
                                             {
                                                 Alias = @alias,
                                                 EntityType = entityType,
                                                 Identifer =
                                                     this.GetMemberName(selector)
                                             });

            foreach(var additionalSelector in additionalSelectors)
            {
                this.Specification.Groupings.Add(new GroupSpecification
                                                 {
                                                     Alias = @alias,
                                                     EntityType = entityType,
                                                     Identifer =
                                                         this.GetMemberName(
                                                             additionalSelector)
                                                 });
            }
        }

        private void AddHavingSpecification<T>(Expression<Func<T, bool>> selector,
            Aggregation aggregation,
            string @alias = null)
        {
            var binaryExpression = selector.Body as BinaryExpression;
            this.Specification.Havings.Add(new SelectStatementHavingSpecification
                                           {
                                               Aggregation = aggregation,
                                               Alias = @alias,
                                               EntityType = typeof(T),
                                               Identifier =
                                                   this.GetMemberName(
                                                       binaryExpression
                                                           .Left),
                                               Operator = this
                                                   .OperatorString(
                                                       binaryExpression
                                                           .NodeType),
                                               Value = this.FormatValue(
                                                   this
                                                       .GetExpressionValue(
                                                           selector))
                                           });
        }

        private void AddInFilterCondition<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null,
            LogicalOperator locigalOperator = LogicalOperator.NotSet)
        {
            if(values != null && values.Any())
            {
                this.currentFilterGroup.Conditions.Add(new FilterCondition
                                                       {
                                                           Alias = alias,
                                                           EntityType = typeof(T),
                                                           LocigalOperator =
                                                               locigalOperator,
                                                           Left = this.GetMemberName(this
                                                               .ConvertExpression(
                                                                   selector)),
                                                           Operator = "IN",
                                                           Right = "(" + string.Join(", ",
                                                                       values.Select(
                                                                           v => this
                                                                               .FormatValue(
                                                                                   v)))
                                                                   + ")"
                                                       });
            }
        }

        private void AddJoinCondition<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null,
            LogicalOperator locLogicalOperator = LogicalOperator.NotSet)
        {
            var binaryExpression = expression.Body as BinaryExpression;
            this.Specification.Joins.Add(new JoinSpecification
                                         {
                                             LeftEntityType = typeof(TLeft),
                                             LeftIdentifier =
                                                 this.GetMemberName(binaryExpression
                                                     .Left),
                                             LeftTableAlias = leftTableAlias,
                                             Operator = this.OperatorString(
                                                 binaryExpression.NodeType),
                                             RightEntityType = typeof(TRight),
                                             RightIdentifier =
                                                 this.GetMemberName(binaryExpression
                                                     .Right),
                                             RightTableAlias = rightTableAlias,
                                             LogicalOperator = locLogicalOperator
                                         });
        }

        private void AddNewFilterGroup(FilterGroupType filterGroupType)
        {
            var filterGroup = new FilterGroup
                              {
                                  GroupType = filterGroupType,
                                  Parent = this.currentFilterGroup
                              };
            this.currentFilterGroup.Groups.Add(filterGroup);
            this.currentFilterGroup = filterGroup;
        }

        private void AddOrderSpecification<T>(Expression<Func<T, object>> selector,
            string alias = null,
            OrderByDirection orderByDirection = OrderByDirection.Ascending,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            var entityType = typeof(T);
            this.Specification.Orderings.Add(new OrderSpecification
                                             {
                                                 Alias = @alias,
                                                 Direction = orderByDirection,
                                                 EntityType = entityType,
                                                 Identifer =
                                                     this.GetMemberName(selector)
                                             });

            foreach(var additionalSelector in additionalSelectors)
            {
                this.Specification.Orderings.Add(new OrderSpecification
                                                 {
                                                     Alias = @alias,
                                                     Direction = orderByDirection,
                                                     EntityType = entityType,
                                                     Identifer =
                                                         this.GetMemberName(
                                                             additionalSelector)
                                                 });
            }
        }

        private void AddTableSpecification<T>(JoinType joinType,
            string @alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.ThrowIfTableAlreadyJoined<T>(@alias, tableName, tableSchema);
            this.Specification.Tables.Add(new SelectStatementTableSpecification
                                          {
                                              Alias = alias,
                                              EntityType = typeof(T),
                                              JoinType = joinType,
                                              TableName =
                                                  string.IsNullOrEmpty(
                                                      tableName)
                                                      ? typeof(T).Name
                                                      : tableName,
                                              Schema =
                                                  string.IsNullOrEmpty(
                                                      tableSchema)
                                                      ? DefaultSchema
                                                      : tableSchema
                                          });
        }

        private void FinalizeColumnSpecifications()
        {
            foreach(var specification in this.Specification.Columns)
            {
                var tableSpecification =
                    this.FindTableSpecification(specification.EntityType, specification.Alias);
                specification.Table = tableSpecification.TableName;
                specification.Schema = tableSpecification.Schema;
            }
        }

        private void FinalizeGroupings()
        {
            foreach(var grouping in this.Specification.Groupings)
            {
                var tableSpecification = this.FindTableSpecification(grouping.EntityType, grouping.Alias);
                grouping.TableName = tableSpecification.TableName;
                grouping.Schema = tableSpecification.Schema;
            }
        }

        private void FinalizeHavings()
        {
            foreach(var having in this.Specification.Havings)
            {
                var tableSpecification = this.FindTableSpecification(having.EntityType, having.Alias);
                having.TableName = tableSpecification.TableName;
                having.Schema = tableSpecification.Schema;
            }
        }

        private void FinalizeJoinConditions()
        {
            foreach(var joinSpecification in this.Specification.Joins)
            {
                var leftTableSpecification = this.FindTableSpecification(joinSpecification.LeftEntityType,
                    joinSpecification.LeftTableAlias);
                joinSpecification.LeftTableName = leftTableSpecification.TableName;
                joinSpecification.LeftSchema = leftTableSpecification.Schema;

                var rightTableSpecification = this.FindTableSpecification(joinSpecification.RightEntityType,
                    joinSpecification.RightTableAlias);
                if(rightTableSpecification == null)
                {
                    continue;
                }
                joinSpecification.RightTableName = rightTableSpecification.TableName;
                joinSpecification.RightSchema = rightTableSpecification.Schema;
            }
        }

        private void FinalizeOrderings()
        {
            foreach(var ordering in this.Specification.Orderings)
            {
                var tableSpecification = this.FindTableSpecification(ordering.EntityType, ordering.Alias);
                ordering.TableName = tableSpecification.TableName;
                ordering.Schema = tableSpecification.Schema;
            }
        }

        private void FinalizeWhereConditions(IList<FilterGroup> filterGroups)
        {
            foreach(var filter in filterGroups)
            {
                foreach(var condition in filter.Conditions)
                {
                    var tableSpecication = this.FindTableSpecification(condition.EntityType, condition.Alias);
                    condition.TableName = tableSpecication.TableName;
                    condition.Schema = tableSpecication.Schema;
                }

                this.FinalizeWhereConditions(filter.Groups);
            }
        }

        private SelectStatementTableSpecification FindTableSpecification<T>(string alias = null)
        {
            return this.FindTableSpecification(typeof(T), alias);
        }

        private SelectStatementTableSpecification FindTableSpecification(Type entityType, string alias = null)
        {
            return this.Specification.Tables.FirstOrDefault(
                e => e.EntityType == entityType && e.Alias == @alias);
        }

        private void InitialiseConfig()
        {
            this.Specification = new SelectStatementSpecification();
            var entityType = typeof(TEntity);
            this.Specification.Tables.Add(new SelectStatementTableSpecification
                                          {
                                              EntityType = entityType,
                                              Schema = DefaultSchema,
                                              TableName = entityType.Name
                                          });
        }

        private void InitialiseFiltering()
        {
            this.rootFilterGroup = new FilterGroup();
            this.Specification.Filters.Add(this.rootFilterGroup);
            this.currentFilterGroup = this.rootFilterGroup;
        }

        private string OperatorStringFromComparison(Comparison comparison)
        {
            switch(comparison)
            {
                case Comparison.GreaterThan: return ">";
                case Comparison.GreaterThanOrEqual: return ">=";
                case Comparison.LessThan: return "<";
                case Comparison.LessThanOrEqual: return "<=";
                case Comparison.NotEqual: return "<>";
                default: return "=";
            }
        }

        private void ThrowIfFilteringNotInitialised()
        {
            if(this.Specification.Filters.Count == 0)
            {
                throw new InvalidOperationException(
                    "Filtering has not been initialised, please use a Where method before any And or Or method.");
            }
        }

        private void ThrowIfGroupingNotInitialised()
        {
            if(this.Specification.Groupings.Count == 0)
            {
                throw new InvalidOperationException(
                    "Grouping has not been initialised, pluase a GroupBy method before any Having method.");
            }
        }

        private void ThrowIfTableAlreadyJoined<T>(string @alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            if(this.FindTableSpecification<T>(@alias) != null)
            {
                throw new InvalidOperationException(
                    "The entity has already been joined into the query, you must use a unique alias, table name override or schema override to join it again.");
            }
        }

        private void ThrowIfTableNotSpecified<T>(string @alias = null)
        {
            if(this.FindTableSpecification<T>(@alias) == null)
            {
                throw new InvalidOperationException(
                    "A table specification for the entity type and alias must be set using From or one of the Join methods before filtering, sorting or grouping can be applied.");
            }
        }
    }
}