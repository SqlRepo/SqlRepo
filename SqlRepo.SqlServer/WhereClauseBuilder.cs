using System;
using System.Linq;
using System.Linq.Expressions;

namespace SqlRepo.SqlServer
{
    public class WhereClauseBuilder : ClauseBuilder, IWhereClauseBuilder
    {
        private WhereClauseGroup currentGroup;
        private WhereClauseGroup rootGroup;
        public string ActiveAlias { get; private set; }

        public IWhereClauseBuilder And<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.ThrowIfNotInitialised();
            return this.AddConditionToCurrentGroup(expression, LogicalOperator.And, alias, tableName, tableSchema);
        }

        public IWhereClauseBuilder AndIn<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.ThrowIfNotInitialised();
            this.AddInConditionToCurrentGroup(selector, values, LogicalOperator.And, alias, tableName, tableSchema);

            return this;
        }

        public IWhereClauseBuilder EndNesting()
        {
            if(this.currentGroup != null && this.currentGroup.Parent != null)
            {
                this.currentGroup = this.currentGroup.Parent;
            }
            return this;
        }

        public IWhereClauseBuilder FromScratch()
        {
            this.rootGroup = null;
            this.currentGroup = null;
            this.IsClean = true;
            return this;
        }

        public IWhereClauseBuilder NestedAnd<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            return this.AddNestedGroupToCurrentGroup(expression, WhereClauseGroupType.And, alias, tableName, tableSchema);
        }

        public IWhereClauseBuilder NestedOr<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            return this.AddNestedGroupToCurrentGroup(expression, WhereClauseGroupType.Or, alias, tableName, tableSchema);
        }

        public IWhereClauseBuilder Or<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            return this.AddConditionToCurrentGroup(expression, LogicalOperator.Or, alias, tableName, tableSchema);
        }

        public IWhereClauseBuilder OrIn<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.ThrowIfNotInitialised();

            this.AddInConditionToCurrentGroup(selector, values, LogicalOperator.Or, alias, tableName, tableSchema);

            return this;
        }

        public override string Sql()
        {
            return this.rootGroup?.ToString() ?? string.Empty;
        }

        public IWhereClauseBuilder UsingAlias(string alias)
        {
            this.ActiveAlias = alias;
            return this;
        }

        public IWhereClauseBuilder Where<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.Initialise();
            this.AddConditionToCurrentGroup(expression, LogicalOperator.NotSet, alias, tableName, tableSchema);
            this.IsClean = false;
            return this;
        }

        public IWhereClauseBuilder WhereIn<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.Initialise();

            this.AddInConditionToCurrentGroup(selector, values, LogicalOperator.NotSet, alias, tableName, tableSchema);

            return this;
        }

        private IWhereClauseBuilder AddConditionToCurrentGroup<TEntity>(Expression<Func<TEntity, bool>> expression,
            LogicalOperator locigalOperator,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.ThrowIfNotInitialised();

            var operatorString = this.GetOperator(expression);
            var @value = this.FormatValue(this.GetExpressionValue(expression));
            var actualOperator = this.GetActualOperator(operatorString, @value);
            this.currentGroup.Conditions.Add(new WhereClauseCondition
                                             {
                                                 Alias = string.IsNullOrWhiteSpace(alias)? this.ActiveAlias: alias,
                                                 LocigalOperator = locigalOperator,
                                                 LeftTable =
                                                     string.IsNullOrWhiteSpace(tableName)
                                                         ? this.TableNameFromType<TEntity>()
                                                         : tableName,
                                                 LeftSchema =
                                                     string.IsNullOrWhiteSpace(tableSchema)? DefaultSchema: tableSchema,
                                                 Left = this.GetMemberName(expression),
                                                 Operator = actualOperator,
                                                 Right = @value
                                             });
            return this;
        }

        private void AddInConditionToCurrentGroup<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            LogicalOperator locigalOperator,
            string alias,
            string tableName,
            string tableSchema)
        {
            if(values != null && values.Any())
            {
                this.currentGroup.Conditions.Add(new WhereClauseCondition
                                                 {
                                                     Alias = string.IsNullOrWhiteSpace(alias)? this.ActiveAlias: alias,
                                                     LocigalOperator = locigalOperator,
                                                     LeftTable =
                                                         string.IsNullOrWhiteSpace(tableName)
                                                             ? this.TableNameFromType<TEntity>()
                                                             : tableName,
                                                     LeftSchema =
                                                         string.IsNullOrWhiteSpace(tableSchema)
                                                             ? DefaultSchema
                                                             : tableSchema,
                                                     Left = this.GetMemberName(this.ConvertExpression(selector)),
                                                     Operator = "IN",
                                                     Right =
                                                         "("
                                                         + string.Join(", ", values.Select(v => this.FormatValue(v)))
                                                         + ")"
                                                 });
            }
        }

        private IWhereClauseBuilder AddNestedGroupToCurrentGroup<TEntity>(Expression<Func<TEntity, bool>> expression,
            WhereClauseGroupType groupType,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.ThrowIfNotInitialised();
            var newGroup = new WhereClauseGroup
                           {
                               GroupType = groupType,
                               Parent = this.currentGroup
                           };
            this.currentGroup.Groups.Add(newGroup);
            this.currentGroup = newGroup;
            return this.AddConditionToCurrentGroup(expression, LogicalOperator.NotSet, alias, tableName, tableSchema);
        }

        private string GetActualOperator(string operatorString, string @value)
        {
            return @value != "NULL"? operatorString: (operatorString == "="? "IS": "IS NOT");
        }

        private void Initialise()
        {
            this.rootGroup = new WhereClauseGroup
                             {
                                 GroupType = WhereClauseGroupType.Where
                             };
            this.currentGroup = this.rootGroup;
        }

        private void ThrowIfNotInitialised()
        {
            if(this.rootGroup == null)
            {
                throw new InvalidOperationException(
                    "Where must be used before any additional conditions can be applied.");
            }
        }
    }
}