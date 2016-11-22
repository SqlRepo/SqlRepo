using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SqlRepo.SqlServer
{
    public class SelectCommand<TEntity> : SqlCommand<TEntity, IEnumerable<TEntity>>, ISelectCommand<TEntity>
        where TEntity: class, new()
    {
        private const string CommandTemplate = "{0}\n{1}{2}{3};";
        private readonly IFromClauseBuilder fromClauseBuilder;
        private readonly IOrderByClauseBuilder orderByClauseBuilder;
        private readonly ISelectClauseBuilder selectClauseBuilder;
        private readonly IWhereClauseBuilder whereClauseBuilder;

        public SelectCommand(ICommandExecutor commandExecutor,
            IEntityMapper entityMapper,
            ISelectClauseBuilder selectClauseBuilder,
            IFromClauseBuilder fromClauseBuilder,
            IWhereClauseBuilder whereClauseBuilder,
            IOrderByClauseBuilder orderByClauseBuilder)
            : base(commandExecutor, entityMapper)
        {
            this.selectClauseBuilder = selectClauseBuilder;
            this.fromClauseBuilder = fromClauseBuilder;
            this.whereClauseBuilder = whereClauseBuilder;
            this.orderByClauseBuilder = orderByClauseBuilder;
        }

        public ISelectCommand<TEntity> And(Expression<Func<TEntity, bool>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            return this.And<TEntity>(selector, alias, tableName, tableSchema);
        }

        public ISelectCommand<TEntity> And<T>(Expression<Func<T, bool>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.whereClauseBuilder.And(selector, alias, tableName, tableName);
            return this;
        }

        public ISelectCommand<TEntity> AndIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.whereClauseBuilder.AndIn(selector, values, alias, tableName, tableSchema);
            return this;
        }

        public ISelectCommand<TEntity> EndNesting()
        {
            this.whereClauseBuilder.EndNesting();
            return this;
        }

        public ISelectCommand<TEntity> FromScratch()
        {
            this.selectClauseBuilder.FromScratch();
            this.fromClauseBuilder.FromScratch();
            this.whereClauseBuilder.FromScratch();
            this.orderByClauseBuilder.FromScratch();
            this.IsClean = true;
            return this;
        }

        public override IEnumerable<TEntity> Go(string connectionString = null)
        {
            if(string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = this.ConnectionString;
            }

            using(var reader = this.CommandExecutor.ExecuteReader(this.Sql()))
            {
                return this.EntityMapper.Map<TEntity>(reader);
            }
        }

        public ISelectCommand<TEntity> InnerJoin<TLeft, TRight>(string leftTableAlias = null,
            string rightTableAlias = null,
            string rightTableName = null,
            string rightTableSchema = null)
        {
            if(this.fromClauseBuilder.IsClean)
            {
                this.fromClauseBuilder.From<TEntity>(leftTableAlias);
            }

            this.fromClauseBuilder.InnerJoin<TLeft, TRight>(leftTableAlias,
                rightTableAlias,
                rightTableName,
                rightTableSchema);

            return this;
        }

        public ISelectCommand<TEntity> LeftOuterJoin<TLeft, TRight>(string leftTableAlias = null,
            string rightTableAlias = null,
            string rightTableName = null,
            string rightTableSchema = null)
        {
            if(this.fromClauseBuilder.IsClean)
            {
                this.fromClauseBuilder.From<TEntity>(leftTableAlias);
            }

            this.fromClauseBuilder.LeftOuterJoin<TLeft, TRight>(leftTableAlias,
                rightTableAlias,
                rightTableName,
                rightTableSchema);

            return this;
        }

        public ISelectCommand<TEntity> NestedAnd<T>(Expression<Func<T, bool>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.whereClauseBuilder.NestedAnd(selector, alias, tableName, tableSchema);
            return this;
        }

        public ISelectCommand<TEntity> NestedOr<T>(Expression<Func<T, bool>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.whereClauseBuilder.NestedOr(selector, alias, tableName, tableSchema);
            return this;
        }

        public ISelectCommand<TEntity> On<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null)
        {
            this.fromClauseBuilder.On(expression, leftTableAlias, rightTableAlias);
            return this;
        }

        public ISelectCommand<TEntity> Or<T>(Expression<Func<T, bool>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.whereClauseBuilder.Or(selector, alias, tableName, tableSchema);
            return this;
        }

        public ISelectCommand<TEntity> OrderBy<T>(Expression<Func<T, object>> selector,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            this.orderByClauseBuilder.By(selector, additionalSelectors);
            return this;
        }

        public ISelectCommand<TEntity> OrderByDescending<T>(Expression<Func<T, object>> expression,
            params Expression<Func<T, object>>[] additionalSelectors)
        {
            this.orderByClauseBuilder.ByDescending(expression, additionalSelectors);
            return this;
        }

        public ISelectCommand<TEntity> OrIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.whereClauseBuilder.OrIn(selector, values, alias, tableName, tableSchema);
            return this;
        }

        public ISelectCommand<TEntity> RightOuterJoin<TLeft, TRight>(string leftTableAlias = null,
            string rightTableAlias = null,
            string rightTableName = null,
            string rightTableSchema = null)
        {
            if(this.fromClauseBuilder.IsClean)
            {
                this.fromClauseBuilder.From<TEntity>(leftTableAlias);
            }

            this.fromClauseBuilder.RightOuterJoin<TLeft, TRight>(leftTableAlias,
                rightTableAlias,
                rightTableName,
                rightTableSchema);

            return this;
        }

        public ISelectCommand<TEntity> Select(string alias,
            Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            this.selectClauseBuilder.Select(alias, selector, additionalSelectors);
            return this;
        }

        public ISelectCommand<TEntity> Select(Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            this.selectClauseBuilder.Select(selector, additionalSelectors);
            return this;
        }

        public ISelectCommand<TEntity> SelectAll<T>(string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.selectClauseBuilder.SelectAll<T>(alias, tableName, tableSchema);
            return this;
        }

        public override string Sql()
        {
            var selectClause = this.selectClauseBuilder.Sql();
            if(this.fromClauseBuilder.IsClean)
            {
                this.fromClauseBuilder.From<TEntity>();
            }
            var fromClause = this.fromClauseBuilder.Sql();
            var whereClause = this.whereClauseBuilder.IsClean
                                  ? string.Empty
                                  : $"\n{this.whereClauseBuilder.Sql()}";
            var orderByClause = this.orderByClauseBuilder.IsClean
                                    ? string.Empty
                                    : $"\n{this.orderByClauseBuilder.Sql()}";
            return string.Format(CommandTemplate, selectClause, fromClause, whereClause, orderByClause);
        }

        public ISelectCommand<TEntity> Top(int rows)
        {
            this.selectClauseBuilder.Top(rows);
            return this;
        }

        public ISelectCommand<TEntity> UsingTableName(string tableName)
        {
            this.TableName = tableName;
            return this;
        }

        public ISelectCommand<TEntity> UsingTableSchema(string tableSchema)
        {
            this.TableSchema = tableSchema;
            return this;
        }

        public ISelectCommand<TEntity> Where<T>(Expression<Func<T, bool>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.whereClauseBuilder.Where(selector, alias, tableName, tableSchema);
            return this;
        }

        public ISelectCommand<TEntity> Where(Expression<Func<TEntity, bool>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            return this.Where<TEntity>(selector, alias, tableName, tableSchema);
        }

        public ISelectCommand<TEntity> WhereIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.whereClauseBuilder.WhereIn(selector, values, alias, tableName, tableSchema);
            return this;
        }
    }
}