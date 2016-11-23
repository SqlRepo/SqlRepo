using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SqlRepo.SqlServer
{
    public class SelectClauseBuilder : ClauseBuilder, ISelectClauseBuilder
    {
        private const string ClauseTemplate = "SELECT {0}{1}";
        private readonly IList<ColumnSelection> selections = new List<ColumnSelection>();
        private int? topRows;
        public string ActiveAlias { get; private set; }

        public ISelectClauseBuilder For<TEntity>(TEntity entity,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            foreach (var property in entity.GetType()
                .GetProperties()
                .Where(p => p.CanWrite))
            {
                AddColumnSelection<TEntity>(alias, tableName, tableSchema, property.Name);
            }
            IsClean = false;
            return this;
        }

        public ISelectClauseBuilder FromScratch()
        {
            selections.Clear();
            IsClean = true;
            return this;
        }

        public ISelectClauseBuilder Select<TEntity>(Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return Select(ActiveAlias,
                TableNameFromType<TEntity>(),
                DefaultSchema,
                selector,
                additionalSelectors);
        }

        public ISelectClauseBuilder Select<TEntity>(string alias,
            Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return Select(alias,
                TableNameFromType<TEntity>(),
                DefaultSchema,
                selector,
                additionalSelectors);
        }

        public ISelectClauseBuilder Select<TEntity>(string alias,
            string tableName,
            Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return Select(alias, tableName, DefaultSchema, selector, additionalSelectors);
        }

        public ISelectClauseBuilder Select<TEntity>(string alias,
            string tableName,
            string tableSchema,
            Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            AddColumnSelection<TEntity>(alias, tableName, tableSchema, GetMemberName(selector));
            foreach (var additionalSelector in additionalSelectors)
            {
                AddColumnSelection<TEntity>(alias,
                    tableName,
                    tableSchema,
                    GetMemberName(additionalSelector));
            }

            IsClean = false;
            return this;
        }

        public ISelectClauseBuilder SelectAll<TEntity>(string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            AddColumnSelection<TEntity>(alias, tableName, tableSchema, "*");
            return this;
        }

        public override string Sql()
        {
            var selection = string.IsNullOrWhiteSpace(ActiveAlias) ? "*" : $"[{ActiveAlias}].*";

            if (selections.Any())
            {
                selection = string.Join(", ", selections);
            }

            return string.Format(ClauseTemplate,
                topRows.HasValue ? $"TOP {topRows.Value} " : string.Empty,
                selection);
        }

        public ISelectClauseBuilder Top(int rows)
        {
            topRows = rows;
            return this;
        }

        public ISelectClauseBuilder UsingAlias(string alias)
        {
            ActiveAlias = alias;
            return this;
        }

        private void AddColumnSelection<TEntity>(string alias,
            string tableName,
            string tableSchema,
            string name)
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                alias = ActiveAlias;
            }

            if (string.IsNullOrWhiteSpace(tableName))
            {
                tableName = TableNameFromType<TEntity>();
            }

            if (string.IsNullOrWhiteSpace(tableSchema))
            {
                tableSchema = DefaultSchema;
            }

            selections.Add(new ColumnSelection
            {
                Alias = alias,
                Table = tableName,
                Schema = tableSchema,
                Name = name
            });
        }
    }
}