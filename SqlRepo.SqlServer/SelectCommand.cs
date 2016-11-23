using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SqlRepo.SqlServer
{
    public class SelectCommand<TEntity> : SqlCommand<TEntity, IEnumerable<TEntity>>, ISelectCommand<TEntity>
        where TEntity : class, new()
    {
        private FilterGroup currentFilterGroup;
        private FilterGroup rootFilterGroup;

        public SelectCommand(ICommandExecutor commandExecutor, IEntityMapper entityMapper)
            : base(commandExecutor, entityMapper)
        {
            InitialiseConfig();
        }

        public SelectCommandSpecification Specification { get; private set; }

        public ISelectCommand<TEntity> And<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            ThrowIfFilteringNotInitialised();
            AddFilterCondition(selector, @alias, LogicalOperator.And);
            return this;
        }

        public ISelectCommand<TEntity> And(Expression<Func<TEntity, bool>> selector, string @alias = null)
        {
            return And<TEntity>(selector, @alias);
        }

        public ISelectCommand<TEntity> AndBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string @alias = null)
        {
            ThrowIfFilteringNotInitialised();
            AddBetweenFilterCondition(selector, start, end, alias, LogicalOperator.And);
            return this;
        }

        public ISelectCommand<TEntity> AndBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string @alias = null)
        {
            return AndBetween<TEntity, TMember>(selector, start, end, @alias);
        }

        public ISelectCommand<TEntity> AndIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string @alias = null)
        {
            ThrowIfFilteringNotInitialised();
            AddInFilterCondition(selector, values, alias, LogicalOperator.And);
            return this;
        }

        public ISelectCommand<TEntity> AndIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string @alias = null)
        {
            return AndIn<TEntity, TMember>(selector, values, @alias);
        }

        public ISelectCommand<TEntity> AndOn<TRight>(Expression<Func<TEntity, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null)
        {
            return AndOn<TEntity, TRight>(expression, leftTableAlias, rightTableAlias);
        }

        public ISelectCommand<TEntity> AndOn<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null)
        {
            if (Specification.Tables.Count < 2)
            {
                throw new InvalidOperationException(
                    "On cannot be used before initialising a join with one of the Join methods.");
            }

            AddJoinCondition(expression, leftTableAlias, rightTableAlias, LogicalOperator.And);
            return this;
        }

        public ISelectCommand<TEntity> Avg(Expression<Func<TEntity, object>> selector, string @alias = null)
        {
            return Avg<TEntity>(selector, @alias);
        }

        public ISelectCommand<TEntity> Avg<T>(Expression<Func<T, object>> selector, string @alias = null)
        {
            AddColumnSelection<T>(GetMemberName(selector), @alias, Aggregation.Avg);
            return this;
        }

        public ISelectCommand<TEntity> Count(Expression<Func<TEntity, object>> selector, string @alias = null)
        {
            return Count<TEntity>(selector, @alias);
        }

        public ISelectCommand<TEntity> Count<T>(Expression<Func<T, object>> selector, string @alias = null)
        {
            AddColumnSelection<T>(GetMemberName(selector), @alias, Aggregation.Count);
            return this;
        }

        public ISelectCommand<TEntity> CountAll()
        {
            Specification.Columns.Add(new ColumnSpecification
            {
                Identifier = "*",
                Aggregation = Aggregation.Count
            });
            return this;
        }

        public ISelectCommand<TEntity> EndNesting()
        {
            currentFilterGroup = currentFilterGroup.Parent;
            return this;
        }

        public ISelectCommand<TEntity> From(string @alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            var rootTable = Specification.Tables.First();
            if (!string.IsNullOrEmpty(alias))
            {
                rootTable.Alias = alias;
            }

            if (!string.IsNullOrEmpty(tableName))
            {
                rootTable.TableName = tableName;
            }

            if (!string.IsNullOrEmpty(tableSchema))
            {
                rootTable.Schema = tableSchema;
            }
            return this;
        }

        public override IEnumerable<TEntity> Go(string connectionString = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = ConnectionString;
            }

            using (var reader = CommandExecutor.ExecuteReader(connectionString, Sql()))
            {
                return EntityMapper.Map<TEntity>(reader);
            }
        }

        public ISelectCommand<TEntity> GroupBy<T>(Expression<Func<T, object>> selector,
            string @alias = null,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            ThrowIfTableNotSpecified<T>(@alias);
            AddGroupSpecification(selector, @alias);
            foreach (var additionalSelector in additionalSelectors)
            {
                AddGroupSpecification(additionalSelector, @alias);
            }
            return this;
        }

        public ISelectCommand<TEntity> GroupBy(Expression<Func<TEntity, object>> selector,
            string @alias = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return GroupBy<TEntity>(selector, @alias, additionalSelectors);
        }

        public ISelectCommand<TEntity> HavingAvg<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            ThrowIfGroupingNotInitialised();
            AddHavingSpecification(selector, Aggregation.Avg, alias);
            return this;
        }

        public ISelectCommand<TEntity> HavingAvg(Expression<Func<TEntity, bool>> selector,
            string @alias = null)
        {
            return HavingAvg<TEntity>(selector, @alias);
        }

        public ISelectCommand<TEntity> HavingCount<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            ThrowIfGroupingNotInitialised();
            AddHavingSpecification(selector, Aggregation.Count, alias);
            return this;
        }

        public ISelectCommand<TEntity> HavingCount(Expression<Func<TEntity, bool>> selector,
            string @alias = null)
        {
            return HavingCount<TEntity>(selector, @alias);
        }

        public ISelectCommand<TEntity> HavingCountAll<T>(Comparison comparison, int @value)
        {
            ThrowIfGroupingNotInitialised();
            Specification.Havings.Add(new SelectCommandHavingSpecification
            {
                Aggregation = Aggregation.Count,
                EntityType = typeof (T),
                Identifier = "*",
                Operator = OperatorStringFromComparison(comparison),
                Value = FormatValue(@value)
            });
            return this;
        }

        public ISelectCommand<TEntity> HavingCountAll(Comparison comparison, int @value)
        {
            return HavingCountAll<TEntity>(comparison, @value);
        }

        public ISelectCommand<TEntity> HavingMax<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            ThrowIfGroupingNotInitialised();
            AddHavingSpecification(selector, Aggregation.Max, alias);
            return this;
        }

        public ISelectCommand<TEntity> HavingMax(Expression<Func<TEntity, bool>> selector,
            string @alias = null)
        {
            return HavingMax<TEntity>(selector, @alias);
        }

        public ISelectCommand<TEntity> HavingMin<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            ThrowIfGroupingNotInitialised();
            AddHavingSpecification(selector, Aggregation.Min, alias);
            return this;
        }

        public ISelectCommand<TEntity> HavingMin(Expression<Func<TEntity, bool>> selector,
            string @alias = null)
        {
            return HavingMin<TEntity>(selector, @alias);
        }

        public ISelectCommand<TEntity> HavingSum<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            ThrowIfGroupingNotInitialised();
            AddHavingSpecification(selector, Aggregation.Sum, alias);
            return this;
        }

        public ISelectCommand<TEntity> HavingSum(Expression<Func<TEntity, bool>> selector,
            string @alias = null)
        {
            return HavingSum<TEntity>(selector, @alias);
        }

        public ISelectCommand<TEntity> InnerJoin<TRight>(string @alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            AddTableSpecification<TRight>(JoinType.Inner, @alias, tableName, tableSchema);
            return this;
        }

        public ISelectCommand<TEntity> LeftOuterJoin<TRight>(string @alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            AddTableSpecification<TRight>(JoinType.LeftOuter, alias, tableName, tableSchema);
            return this;
        }

        public ISelectCommand<TEntity> Max(Expression<Func<TEntity, object>> selector, string @alias = null)
        {
            return Max<TEntity>(selector, @alias);
        }

        public ISelectCommand<TEntity> Max<T>(Expression<Func<T, object>> selector, string @alias = null)
        {
            AddColumnSelection<T>(GetMemberName(selector), @alias, Aggregation.Max);
            return this;
        }

        public ISelectCommand<TEntity> Min(Expression<Func<TEntity, object>> selector, string @alias = null)
        {
            return Min<TEntity>(selector, @alias);
        }

        public ISelectCommand<TEntity> Min<T>(Expression<Func<T, object>> selector, string @alias = null)
        {
            AddColumnSelection<T>(GetMemberName(selector), @alias, Aggregation.Min);
            return this;
        }

        public ISelectCommand<TEntity> NestedAnd<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            ThrowIfFilteringNotInitialised();
            AddNewFilterGroup(FilterGroupType.And);
            AddFilterCondition(selector, alias);
            return this;
        }

        public ISelectCommand<TEntity> NestedAnd(Expression<Func<TEntity, bool>> selector, string alias = null)
        {
            return NestedAnd<TEntity>(selector, alias);
        }

        public ISelectCommand<TEntity> NestedAndBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null)
        {
            ThrowIfFilteringNotInitialised();
            AddNewFilterGroup(FilterGroupType.And);
            AddBetweenFilterCondition(selector, start, end, alias);
            return this;
        }

        public ISelectCommand<TEntity> NestedAndBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null)
        {
            return NestedAndBetween<TEntity, TMember>(selector, start, end, @alias);
        }

        public ISelectCommand<TEntity> NestedAndIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null)
        {
            ThrowIfFilteringNotInitialised();
            AddNewFilterGroup(FilterGroupType.And);
            AddInFilterCondition(selector, values, alias);
            return this;
        }

        public ISelectCommand<TEntity> NestedAndIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null)
        {
            return NestedAndIn<TEntity, TMember>(selector, values, @alias);
        }

        public ISelectCommand<TEntity> NestedOr<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            ThrowIfFilteringNotInitialised();
            AddNewFilterGroup(FilterGroupType.Or);
            AddFilterCondition(selector, alias);
            return this;
        }

        public ISelectCommand<TEntity> NestedOr(Expression<Func<TEntity, bool>> selector, string alias = null)
        {
            return NestedOr<TEntity>(selector, alias);
        }

        public ISelectCommand<TEntity> NestedOrBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null)
        {
            ThrowIfFilteringNotInitialised();
            AddNewFilterGroup(FilterGroupType.Or);
            AddBetweenFilterCondition(selector, start, end, alias);
            return this;
        }

        public ISelectCommand<TEntity> NestedOrBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null)
        {
            return NestedOrBetween<TEntity, TMember>(selector, start, end, alias);
        }

        public ISelectCommand<TEntity> NestedOrIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null)
        {
            ThrowIfFilteringNotInitialised();
            AddNewFilterGroup(FilterGroupType.Or);
            AddInFilterCondition(selector, values, alias);
            return this;
        }

        public ISelectCommand<TEntity> NestedOrIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null)
        {
            return NestedOrIn<TEntity, TMember>(selector, values, alias);
        }

        public ISelectCommand<TEntity> NoLocks()
        {
            Specification.NoLocks = true;
            return this;
        }

        public ISelectCommand<TEntity> On<TRight>(Expression<Func<TEntity, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null)
        {
            return On<TEntity, TRight>(expression, leftTableAlias, rightTableAlias);
        }

        public ISelectCommand<TEntity> On<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null)
        {
            if (Specification.Tables.Count < 2)
            {
                throw new InvalidOperationException(
                    "On cannot be used before initialising a join with one of the Join methods.");
            }

            AddJoinCondition(expression, leftTableAlias, rightTableAlias);
            return this;
        }

        public ISelectCommand<TEntity> Or<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            ThrowIfFilteringNotInitialised();
            AddFilterCondition(selector, @alias, LogicalOperator.Or);
            return this;
        }

        public ISelectCommand<TEntity> Or(Expression<Func<TEntity, bool>> selector, string @alias = null)
        {
            return Or<TEntity>(selector, @alias);
        }

        public ISelectCommand<TEntity> OrBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string @alias = null)
        {
            ThrowIfFilteringNotInitialised();
            AddBetweenFilterCondition(selector, start, end, alias, LogicalOperator.Or);
            return this;
        }

        public ISelectCommand<TEntity> OrBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string @alias = null)
        {
            return OrBetween<TEntity, TMember>(selector, start, end, @alias);
        }

        public ISelectCommand<TEntity> OrderBy<T>(Expression<Func<T, object>> selector,
            string alias = null,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            ThrowIfTableNotSpecified<T>(@alias);
            AddOrderSpecification(selector, alias, OrderByDirection.Ascending, additionalSelectors);
            return this;
        }

        public ISelectCommand<TEntity> OrderBy(Expression<Func<TEntity, object>> selector,
            string @alias = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return OrderBy<TEntity>(selector, alias, additionalSelectors);
        }

        public ISelectCommand<TEntity> OrderByDescending<T>(Expression<Func<T, object>> selector,
            string @alias = null,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            ThrowIfTableNotSpecified<T>(@alias);
            AddOrderSpecification(selector, alias, OrderByDirection.Descending, additionalSelectors);
            return this;
        }

        public ISelectCommand<TEntity> OrderByDescending(Expression<Func<TEntity, object>> selector,
            string @alias = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return OrderByDescending<TEntity>(selector, alias, additionalSelectors);
        }

        public ISelectCommand<TEntity> OrIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string @alias = null)
        {
            ThrowIfFilteringNotInitialised();
            AddInFilterCondition(selector, values, @alias, LogicalOperator.Or);
            return this;
        }

        public ISelectCommand<TEntity> OrIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string @alias = null)
        {
            return OrIn<TEntity, TMember>(selector, values, @alias);
        }

        public ISelectCommand<TEntity> OrOn<TRight>(Expression<Func<TEntity, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null)
        {
            return OrOn<TEntity, TRight>(expression, leftTableAlias, rightTableAlias);
        }

        public ISelectCommand<TEntity> OrOn<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null)
        {
            if (Specification.Tables.Count < 2)
            {
                throw new InvalidOperationException(
                    "On cannot be used before initialising a join with one of the Join methods.");
            }

            AddJoinCondition(expression, leftTableAlias, rightTableAlias, LogicalOperator.Or);
            return this;
        }

        public ISelectCommand<TEntity> Percent(bool useTopPercent = true)
        {
            if (!Specification.Top.HasValue)
            {
                throw new InvalidOperationException("Please call Top to set a value before calling Percent");
            }

            Specification.UseTopPercent = useTopPercent;
            return this;
        }

        public ISelectCommand<TEntity> RightOuterJoin<TLeft>(string @alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            AddTableSpecification<TLeft>(JoinType.RightOuter, alias, tableName, tableSchema);
            return this;
        }

        public ISelectCommand<TEntity> Select(Expression<Func<TEntity, object>> selector,
            string @alias = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return Select<TEntity>(selector, @alias, additionalSelectors);
        }

        public ISelectCommand<TEntity> Select<T>(Expression<Func<T, object>> selector,
            string @alias = null,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            AddColumnSelection(selector, @alias, additionalSelectors);
            return this;
        }

        public ISelectCommand<TEntity> SelectAll(string @alias = null)
        {
            return SelectAll<TEntity>(alias);
        }

        public ISelectCommand<TEntity> SelectAll<T>(string @alias = null)
        {
            AddColumnSelection<T>("*", @alias);
            return this;
        }

        public override string Sql()
        {
            FinalizeColumnSpecifications();
            FinalizeJoinConditions();
            FinalizeWhereConditions(Specification.Filters);
            FinalizeOrderings();
            FinalizeGroupings();
            FinalizeHavings();
            return Specification.ToString();
        }

        public ISelectCommand<TEntity> Sum(Expression<Func<TEntity, object>> selector, string @alias = null)
        {
            return Sum<TEntity>(selector, @alias);
        }

        public ISelectCommand<TEntity> Sum<T>(Expression<Func<T, object>> selector, string @alias = null)
        {
            AddColumnSelection<T>(GetMemberName(selector), @alias, Aggregation.Sum);
            return this;
        }

        public ISelectCommand<TEntity> Top(int rows)
        {
            Specification.Top = rows;
            return this;
        }

        public ISelectCommand<TEntity> Where<T>(Expression<Func<T, bool>> selector, string @alias = null)
        {
            ThrowIfTableNotSpecified<T>(@alias);
            InitialiseFiltering();
            AddFilterCondition(selector, @alias);
            return this;
        }

        public ISelectCommand<TEntity> Where(Expression<Func<TEntity, bool>> selector, string @alias = null)
        {
            return Where<TEntity>(selector, @alias);
        }

        public ISelectCommand<TEntity> WhereBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string @alias = null)
        {
            InitialiseFiltering();
            AddBetweenFilterCondition(selector, start, end, alias);
            return this;
        }

        public ISelectCommand<TEntity> WhereBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string @alias = null)
        {
            return WhereBetween<TEntity, TMember>(selector, start, end, @alias);
        }

        public ISelectCommand<TEntity> WhereIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string @alias = null)
        {
            InitialiseFiltering();
            AddInFilterCondition(selector, values, alias);
            return this;
        }

        public ISelectCommand<TEntity> WhereIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string @alias = null)
        {
            return WhereIn<TEntity, TMember>(selector, values, @alias);
        }

        private void FinalizeHavings()
        {
            foreach (var having in Specification.Havings)
            {
                var tableSpecification = FindTableSpecification(having.EntityType, having.Alias);
                having.TableName = tableSpecification.TableName;
                having.Schema = tableSpecification.Schema;
            }
        }

        private void FinalizeGroupings()
        {
            foreach (var grouping in Specification.Groupings)
            {
                var tableSpecification = FindTableSpecification(grouping.EntityType, grouping.Alias);
                grouping.TableName = tableSpecification.TableName;
                grouping.Schema = tableSpecification.Schema;
            }
        }

        private void FinalizeOrderings()
        {
            foreach (var ordering in Specification.Orderings)
            {
                var tableSpecification = FindTableSpecification(ordering.EntityType, ordering.Alias);
                ordering.TableName = tableSpecification.TableName;
                ordering.Schema = tableSpecification.Schema;
            }
        }

        private void AddBetweenFilterCondition<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string @alias = null,
            LogicalOperator logicalOperator = LogicalOperator.NotSet)
        {
            currentFilterGroup.Conditions.Add(new FilterCondition
            {
                Alias = @alias,
                EntityType = typeof (T),
                Operator = ">=",
                Left = GetMemberName(selector),
                LocigalOperator = logicalOperator,
                Right = FormatValue(start)
            });
            currentFilterGroup.Conditions.Add(new FilterCondition
            {
                Alias = @alias,
                EntityType = typeof (T),
                Operator = "<=",
                Left = GetMemberName(selector),
                LocigalOperator = LogicalOperator.And,
                Right = FormatValue(end)
            });
        }

        private void AddColumnSelection<T>(Expression<Func<T, object>> selector,
            string @alias = null,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            AddColumnSelection<T>(GetMemberName(selector), @alias);
            foreach (var additionalSelector in additionalSelectors)
            {
                AddColumnSelection<T>(GetMemberName(additionalSelector), @alias);
            }
        }

        private void AddColumnSelection<T>(string name,
            string @alias = null,
            Aggregation aggregation = Aggregation.None)
        {
            Specification.Columns.Add(new ColumnSpecification
            {
                Aggregation = aggregation,
                Alias = alias,
                EntityType = typeof (T),
                Identifier = name
            });
        }

        private void AddFilterCondition<T>(Expression<Func<T, bool>> selector,
            string @alias = null,
            LogicalOperator logicalOperator = LogicalOperator.NotSet)
        {
            var binaryExpression = selector.Body as BinaryExpression;
            currentFilterGroup.Conditions.Add(new FilterCondition
            {
                Alias = @alias,
                EntityType = typeof (T),
                Left = GetMemberName(binaryExpression.Left),
                LocigalOperator = logicalOperator,
                Operator =
                    OperatorString(binaryExpression.NodeType),
                Right =
                    FormatValue(GetExpressionValue(selector))
            });
        }

        private void AddGroupSpecification<T>(Expression<Func<T, object>> selector,
            string alias = null,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            var entityType = typeof (T);
            Specification.Groupings.Add(new GroupSpecification
            {
                Alias = @alias,
                EntityType = entityType,
                Identifer = GetMemberName(selector)
            });

            foreach (var additionalSelector in additionalSelectors)
            {
                Specification.Groupings.Add(new GroupSpecification
                {
                    Alias = @alias,
                    EntityType = entityType,
                    Identifer = GetMemberName(additionalSelector)
                });
            }
        }

        private void AddHavingSpecification<T>(Expression<Func<T, bool>> selector,
            Aggregation aggregation,
            string @alias = null)
        {
            var binaryExpression = selector.Body as BinaryExpression;
            Specification.Havings.Add(new SelectCommandHavingSpecification
            {
                Aggregation = aggregation,
                Alias = @alias,
                EntityType = typeof (T),
                Identifier = GetMemberName(binaryExpression.Left),
                Operator = OperatorString(binaryExpression.NodeType),
                Value = FormatValue(GetExpressionValue(selector))
            });
        }

        private void AddInFilterCondition<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null,
            LogicalOperator locigalOperator = LogicalOperator.NotSet)
        {
            if (values != null && values.Any())
            {
                currentFilterGroup.Conditions.Add(new FilterCondition
                {
                    Alias = alias,
                    EntityType = typeof (T),
                    LocigalOperator = locigalOperator,
                    Left =
                        GetMemberName(
                            ConvertExpression(selector)),
                    Operator = "IN",
                    Right =
                        "("
                        + string.Join(", ",
                            values.Select(v => FormatValue(v)))
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
            Specification.Joins.Add(new JoinSpecification
            {
                LeftEntityType = typeof (TLeft),
                LeftIdentifier = GetMemberName(binaryExpression.Left),
                LeftTableAlias = leftTableAlias,
                Operator = OperatorString(binaryExpression.NodeType),
                RightEntityType = typeof (TRight),
                RightIdentifier = GetMemberName(binaryExpression.Right),
                RightTableAlias = rightTableAlias,
                LogicalOperator = locLogicalOperator
            });
        }

        private void AddNewFilterGroup(FilterGroupType filterGroupType)
        {
            var filterGroup = new FilterGroup
            {
                GroupType = filterGroupType,
                Parent = currentFilterGroup
            };
            currentFilterGroup.Groups.Add(filterGroup);
            currentFilterGroup = filterGroup;
        }

        private void AddOrderSpecification<T>(Expression<Func<T, object>> selector,
            string alias = null,
            OrderByDirection orderByDirection = OrderByDirection.Ascending,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            var entityType = typeof (T);
            Specification.Orderings.Add(new OrderSpecification
            {
                Alias = @alias,
                Direction = orderByDirection,
                EntityType = entityType,
                Identifer = GetMemberName(selector)
            });

            foreach (var additionalSelector in additionalSelectors)
            {
                Specification.Orderings.Add(new OrderSpecification
                {
                    Alias = @alias,
                    Direction = orderByDirection,
                    EntityType = entityType,
                    Identifer = GetMemberName(additionalSelector)
                });
            }
        }

        private void AddTableSpecification<T>(JoinType joinType,
            string @alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            ThrowIfTableAlreadyJoined<T>(@alias, tableName, tableSchema);
            Specification.Tables.Add(new SelectCommandTableSpecification
            {
                Alias = alias,
                EntityType = typeof (T),
                JoinType = joinType,
                TableName =
                    string.IsNullOrEmpty(tableName) ? typeof (T).Name : tableName,
                Schema =
                    string.IsNullOrEmpty(tableSchema)
                        ? DefaultSchema
                        : tableSchema
            });
        }

        private void FinalizeColumnSpecifications()
        {
            foreach (var specification in Specification.Columns)
            {
                var tableSpecification = FindTableSpecification(specification.EntityType,
                    specification.Alias);
                specification.Table = tableSpecification.TableName;
                specification.Schema = tableSpecification.Schema;
            }
        }

        private void FinalizeJoinConditions()
        {
            foreach (var joinSpecification in Specification.Joins)
            {
                var leftTableSpecification = FindTableSpecification(joinSpecification.LeftEntityType,
                    joinSpecification.LeftTableAlias);
                joinSpecification.LeftTableName = leftTableSpecification.TableName;
                joinSpecification.LeftSchema = leftTableSpecification.Schema;

                var rightTableSpecification = FindTableSpecification(joinSpecification.RightEntityType,
                    joinSpecification.RightTableAlias);
                if (rightTableSpecification == null)
                {
                    continue;
                }
                joinSpecification.RightTableName = rightTableSpecification.TableName;
                joinSpecification.RightSchema = rightTableSpecification.Schema;
            }
        }

        private void FinalizeWhereConditions(IList<FilterGroup> filterGroups)
        {
            foreach (var filter in filterGroups)
            {
                foreach (var condition in filter.Conditions)
                {
                    var tableSpecication = FindTableSpecification(condition.EntityType, condition.Alias);
                    condition.TableName = tableSpecication.TableName;
                    condition.Schema = tableSpecication.Schema;
                }

                FinalizeWhereConditions(filter.Groups);
            }
        }

        private SelectCommandTableSpecification FindTableSpecification<T>(string alias = null)
        {
            return FindTableSpecification(typeof (T), alias);
        }

        private SelectCommandTableSpecification FindTableSpecification(Type entityType,
            string alias = null)
        {
            return
                Specification.Tables.FirstOrDefault(
                    e =>
                        e.EntityType == entityType && e.Alias == @alias);
        }

        private void InitialiseConfig()
        {
            Specification = new SelectCommandSpecification();
            var entityType = typeof (TEntity);
            Specification.Tables.Add(new SelectCommandTableSpecification
            {
                EntityType = entityType,
                Schema = DefaultSchema,
                TableName = entityType.Name
            });
        }

        private void InitialiseFiltering()
        {
            rootFilterGroup = new FilterGroup();
            Specification.Filters.Add(rootFilterGroup);
            currentFilterGroup = rootFilterGroup;
        }

        private string OperatorStringFromComparison(Comparison comparison)
        {
            switch (comparison)
            {
                case Comparison.GreaterThan:
                    return ">";
                case Comparison.GreaterThanOrEqual:
                    return ">=";
                case Comparison.LessThan:
                    return "<";
                case Comparison.LessThanOrEqual:
                    return "<=";
                case Comparison.NotEqual:
                    return "<>";
                default:
                    return "=";
            }
        }

        private void ThrowIfFilteringNotInitialised()
        {
            if (Specification.Filters.Count == 0)
            {
                throw new InvalidOperationException(
                    "Filtering has not been initialised, please use a Where method before any And or Or method.");
            }
        }

        private void ThrowIfGroupingNotInitialised()
        {
            if (Specification.Groupings.Count == 0)
            {
                throw new InvalidOperationException(
                    "Grouping has not been initialised, pluase a GroupBy method before any Having method.");
            }
        }

        private void ThrowIfTableAlreadyJoined<T>(string @alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            if (FindTableSpecification<T>(@alias) != null)
            {
                throw new InvalidOperationException(
                    "The entity has already been joined into the query, you must use a unique alias, table name override or schema override to join it again.");
            }
        }

        private void ThrowIfTableNotSpecified<T>(string @alias = null)
        {
            if (FindTableSpecification<T>(@alias) == null)
            {
                throw new InvalidOperationException(
                    "A table specification for the entity type and alias must be set using From or one of the Join methods before filtering, sorting or grouping can be applied.");
            }
        }
    }
}