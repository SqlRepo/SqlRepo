using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SqlRepo.SqlServer
{
    public class FromClauseBuilder : ClauseBuilder, IFromClauseBuilder
    {
        private readonly IList<TableSpecification> tableSpecifications = new List<TableSpecification>();
        private TableSpecification currentTableSpecification;

        public IFromClauseBuilder From<TEntity>(string tableAlias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.AddTableSpecification<TEntity>(TableSpecificationType.From, tableName, tableSchema, tableAlias);
            this.IsClean = false;
            return this;
        }

        public IFromClauseBuilder FromScratch()
        {
            this.tableSpecifications.Clear();
            this.IsClean = true;
            return this;
        }

        public IFromClauseBuilder InnerJoin<TLeft, TRight>(string leftTableAlias = null,
            string rightTableAlias = null,
            string rightTableName = null,
            string rightTableSchema = null)
        {
            this.AddTableSpecification<TRight>(TableSpecificationType.InnerJoin,
                rightTableName,
                rightTableSchema,
                rightTableAlias,
                typeof(TLeft),
                leftTableAlias);
            return this;
        }

        public IFromClauseBuilder LeftOuterJoin<TLeft, TRight>(string leftTableAlias = null,
            string rightTableAlias = null,
            string rightTableName = null,
            string rightTableSchema = null)
        {
            this.AddTableSpecification<TRight>(TableSpecificationType.LeftOuterJoin,
                rightTableName,
                rightTableSchema,
                rightTableAlias,
                typeof(TLeft),
                leftTableAlias);
            return this;
        }

        public IFromClauseBuilder RightOuterJoin<TLeft, TRight>(string leftTableAlias = null,
            string rightTableAlias = null,
            string rightTableName = null,
            string rightTableSchema = null)
        {
            this.AddTableSpecification<TRight>(TableSpecificationType.RighOuterJoin,
                rightTableName,
                rightTableSchema,
                rightTableAlias,
                typeof(TLeft),
                leftTableAlias);
            return this;
        }

        public IFromClauseBuilder On<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null)
        {
            this.currentTableSpecification.Conditions.Add(this.GetCondition(LogicalOperator.NotSet, expression));
            return this;
        }

        public override string Sql()
        {
            return string.Join("\n", this.tableSpecifications);
        }

        private void AddTableSpecification<TEntity>(string specificationType,
            string rightTableName,
            string rightTableSchema,
            string rightTableAlias,
            Type leftTableType = null,
            string leftTableAlias = null)
        {
            if(string.IsNullOrWhiteSpace(rightTableName))
            {
                rightTableName = typeof(TEntity).Name;
            }

            if(string.IsNullOrWhiteSpace(rightTableSchema))
            {
                rightTableSchema = "dbo";
            }

            this.currentTableSpecification = new TableSpecification
                                             {
                                                 SpecificationType = specificationType,
                                                 RightSchema = rightTableSchema,
                                                 RightTable = rightTableName,
                                                 RightAlias = rightTableAlias,
                                                 RightType = typeof(TEntity)
                                             };

            if(leftTableType != null)
            {
                var leftTableSpecification =
                    this.tableSpecifications.FirstOrDefault(
                        s =>
                        s.RightType == leftTableType
                        && (string.IsNullOrWhiteSpace(s.RightAlias) || s.RightAlias == leftTableAlias));
                if(leftTableSpecification != null)
                {
                    this.currentTableSpecification.LeftSchema = leftTableSpecification.RightSchema;
                    this.currentTableSpecification.LeftTable = leftTableSpecification.RightTable;
                    this.currentTableSpecification.LeftAlias = leftTableSpecification.RightAlias;
                    this.currentTableSpecification.LeftType = leftTableSpecification.RightType;
                }
            }

            this.tableSpecifications.Add(this.currentTableSpecification);
        }

        private JoinCondition GetCondition<TLeft, TRight>(LogicalOperator logicalOperator,
            Expression<Func<TLeft, TRight, bool>> expression)
        {
            var binaryExpression = expression.Body as BinaryExpression;
            return binaryExpression != null
                       ? new JoinCondition
                         {
                             LogicalOperator = logicalOperator,
                             LeftTableAlias = this.currentTableSpecification.LeftAlias,
                             LeftTableSchema = this.currentTableSpecification.LeftSchema,
                             LeftTableName = this.currentTableSpecification.LeftTable,
                             LeftIdentifier = this.GetMemberName(binaryExpression.Left),
                             RightTableAlias = this.currentTableSpecification.RightAlias,
                             RightTableSchema = this.currentTableSpecification.RightSchema,
                             RightTableName = this.currentTableSpecification.RightTable,
                             RightIdentifier = this.GetMemberName(binaryExpression.Right),
                             Operator = this.OperatorString(binaryExpression.NodeType)
                         }
                       : default(JoinCondition);
        }
    }
}