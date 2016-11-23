using System;
using System.Linq;
using System.Linq.Expressions;

namespace SqlRepo.SqlServer
{
    public class WhereClauseBuilder : ClauseBuilder, IWhereClauseBuilder
    {
        private WhereClauseGroup currentGroup;
        private WhereClauseGroup rootGroup;

        public IWhereClauseBuilder And<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            ThrowIfNotInitialised();
            return AddConditionToCurrentGroup(expression,
                LogicalOperator.And,
                alias,
                tableName,
                tableSchema);
        }

        public IWhereClauseBuilder AndBetween<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            AddBetweenConditionToCurrentGroup(selector,
                start,
                end,
                LogicalOperator.And,
                alias,
                tableName,
                tableSchema);
            return this;
        }

        public IWhereClauseBuilder AndIn<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            ThrowIfNotInitialised();

            AddInConditionToCurrentGroup(selector,
                values,
                LogicalOperator.And,
                alias,
                tableName,
                tableSchema);

            return this;
        }

        public IWhereClauseBuilder EndNesting()
        {
            if (currentGroup != null && currentGroup.Parent != null)
            {
                currentGroup = currentGroup.Parent;
            }
            return this;
        }

        public IWhereClauseBuilder NestedAnd<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            return AddNestedGroupToCurrentGroup(expression,
                WhereClauseGroupType.And,
                alias,
                tableName,
                tableSchema);
        }

        public IWhereClauseBuilder NestedOr<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            return AddNestedGroupToCurrentGroup(expression,
                WhereClauseGroupType.Or,
                alias,
                tableName,
                tableSchema);
        }

        public IWhereClauseBuilder Or<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            return AddConditionToCurrentGroup(expression,
                LogicalOperator.Or,
                alias,
                tableName,
                tableSchema);
        }

        public IWhereClauseBuilder OrBetween<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            AddBetweenConditionToCurrentGroup(selector,
                start,
                end,
                LogicalOperator.Or,
                alias,
                tableName,
                tableSchema);
            return this;
        }

        public IWhereClauseBuilder OrIn<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            ThrowIfNotInitialised();

            AddInConditionToCurrentGroup(selector,
                values,
                LogicalOperator.Or,
                alias,
                tableName,
                tableSchema);

            return this;
        }

        public override string Sql()
        {
            return rootGroup?.ToString() ?? string.Empty;
        }

        public IWhereClauseBuilder Where<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            Initialise();
            AddConditionToCurrentGroup(expression, LogicalOperator.NotSet, alias, tableName, tableSchema);
            IsClean = false;
            return this;
        }

        public IWhereClauseBuilder WhereBetween<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            Initialise();
            AddBetweenConditionToCurrentGroup(selector,
                start,
                end,
                LogicalOperator.NotSet,
                alias,
                tableName,
                tableSchema);
            return this;
        }

        public IWhereClauseBuilder WhereIn<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            Initialise();
            AddInConditionToCurrentGroup(selector,
                values,
                LogicalOperator.NotSet,
                alias,
                tableName,
                tableSchema);
            return this;
        }

        private void AddBetweenConditionToCurrentGroup<TEntity, TMember>(
            Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            LogicalOperator locigalOperator,
            string alias,
            string tableName,
            string tableSchema)
        {
            currentGroup.Conditions.Add(new WhereClauseCondition
            {
                Alias = alias,
                LocigalOperator = locigalOperator,
                LeftTable =
                    string.IsNullOrWhiteSpace(tableName)
                        ? TableNameFromType<TEntity>()
                        : tableName,
                LeftSchema =
                    string.IsNullOrWhiteSpace(tableSchema)
                        ? DefaultSchema
                        : tableSchema,
                Left = GetMemberName(ConvertExpression(selector)),
                Operator = ">=",
                Right = FormatValue(start)
            });
            currentGroup.Conditions.Add(new WhereClauseCondition
            {
                Alias = alias,
                LocigalOperator = LogicalOperator.And,
                LeftTable =
                    string.IsNullOrWhiteSpace(tableName)
                        ? TableNameFromType<TEntity>()
                        : tableName,
                LeftSchema =
                    string.IsNullOrWhiteSpace(tableSchema)
                        ? DefaultSchema
                        : tableSchema,
                Left = GetMemberName(ConvertExpression(selector)),
                Operator = "<=",
                Right = FormatValue(end)
            });
            IsClean = false;
        }

        private IWhereClauseBuilder AddConditionToCurrentGroup<TEntity>(
            Expression<Func<TEntity, bool>> expression,
            LogicalOperator locigalOperator,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            ThrowIfNotInitialised();
            IsClean = false;

            var operatorString = GetOperator(expression);
            var @value = FormatValue(GetExpressionValue(expression));
            var actualOperator = GetActualOperator(operatorString, @value);
            currentGroup.Conditions.Add(new WhereClauseCondition
            {
                Alias = alias,
                LocigalOperator = locigalOperator,
                LeftTable =
                    string.IsNullOrWhiteSpace(tableName)
                        ? TableNameFromType<TEntity>()
                        : tableName,
                LeftSchema =
                    string.IsNullOrWhiteSpace(tableSchema)
                        ? ClauseBuilder.DefaultSchema
                        : tableSchema,
                Left = GetMemberName(expression),
                Operator = actualOperator,
                Right = @value
            });
            return this;
        }

        private void AddInConditionToCurrentGroup<TEntity, TMember>(
            Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            LogicalOperator locigalOperator,
            string alias,
            string tableName,
            string tableSchema)
        {
            if (values != null && values.Any())
            {
                currentGroup.Conditions.Add(new WhereClauseCondition
                {
                    Alias = alias,
                    LocigalOperator = locigalOperator,
                    LeftTable =
                        string.IsNullOrWhiteSpace(tableName)
                            ? TableNameFromType<TEntity>()
                            : tableName,
                    LeftSchema =
                        string.IsNullOrWhiteSpace(tableSchema)
                            ? ClauseBuilder.DefaultSchema
                            : tableSchema,
                    Left =
                        GetMemberName(ConvertExpression(selector)),
                    Operator = "IN",
                    Right =
                        "("
                        + string.Join(", ",
                            values.Select(v => FormatValue(v))) + ")"
                });
                IsClean = false;
            }
        }

        private IWhereClauseBuilder AddNestedGroupToCurrentGroup<TEntity>(
            Expression<Func<TEntity, bool>> expression,
            WhereClauseGroupType groupType,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            ThrowIfNotInitialised();
            var newGroup = new WhereClauseGroup
            {
                GroupType = groupType,
                Parent = currentGroup
            };
            currentGroup.Groups.Add(newGroup);
            currentGroup = newGroup;
            return AddConditionToCurrentGroup(expression,
                LogicalOperator.NotSet,
                alias,
                tableName,
                tableSchema);
        }

        private string GetActualOperator(string operatorString, string @value)
        {
            return @value != "NULL" ? operatorString : (operatorString == "=" ? "IS" : "IS NOT");
        }

        private void Initialise()
        {
            rootGroup = new WhereClauseGroup
            {
                GroupType = WhereClauseGroupType.Where
            };
            currentGroup = rootGroup;
        }

        private void ThrowIfNotInitialised()
        {
            if (rootGroup == null)
            {
                throw new InvalidOperationException(
                    "Where must be used before any additional conditions can be applied.");
            }
        }
    }
}