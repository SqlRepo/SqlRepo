using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
            foreach(var property in entity.GetType()
                                          .GetProperties()
                                          .Where(p => p.CanWrite))
            {
                this.AddColumnSelection<TEntity>(alias, tableName, tableSchema, property.Name);
            }
            this.IsClean = false;
            return this;
        }

        public ISelectClauseBuilder FromScratch()
        {
            this.selections.Clear();
            this.IsClean = true;
            return this;
        }

        public ISelectClauseBuilder Select<TEntity>(Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return this.Select(this.ActiveAlias,
                this.TableNameFromType<TEntity>(),
                DefaultSchema,
                selector,
                additionalSelectors);
        }

        public ISelectClauseBuilder Select<TEntity>(string alias,
            Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return this.Select(alias,
                this.TableNameFromType<TEntity>(),
                DefaultSchema,
                selector,
                additionalSelectors);
        }

        public ISelectClauseBuilder Select<TEntity>(string alias,
            string tableName,
            Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return this.Select(alias, tableName, DefaultSchema, selector, additionalSelectors);
        }

        public ISelectClauseBuilder Select<TEntity>(string alias,
            string tableName,
            string tableSchema,
            Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            this.AddColumnSelection<TEntity>(alias, tableName, tableSchema, this.GetMemberName(selector));
            foreach(var additionalSelector in additionalSelectors)
            {
                this.AddColumnSelection<TEntity>(alias,
                    tableName,
                    tableSchema,
                    this.GetMemberName(additionalSelector));
            }

            this.IsClean = false;
            return this;
        }

        public ISelectClauseBuilder SelectAll<TEntity>(string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.AddColumnSelection<TEntity>(alias, tableName, tableSchema, "*");
            return this;
        }

        public override string Sql()
        {
            var selection = string.IsNullOrWhiteSpace(this.ActiveAlias)? "*": $"[{this.ActiveAlias}].*";
                
            if(this.selections.Any())
            {
                selection = string.Join(", ", this.selections);
            }

             return string.Format(ClauseTemplate,
                    this.topRows.HasValue ? $"TOP {this.topRows.Value} ": string.Empty,
                    selection);
        }

        public ISelectClauseBuilder Top(int rows)
        {
           this.topRows = rows;
            return this;
        }

        public ISelectClauseBuilder UsingAlias(string alias)
        {
            this.ActiveAlias = alias;
            return this;
        }

        private void AddColumnSelection<TEntity>(string alias,
            string tableName,
            string tableSchema,
            string name)
        {
            if(string.IsNullOrWhiteSpace(alias))
            {
                alias = this.ActiveAlias;
            }

            if(string.IsNullOrWhiteSpace(tableName))
            {
                tableName = this.TableNameFromType<TEntity>();
            }

            if(string.IsNullOrWhiteSpace(tableSchema))
            {
                tableSchema = DefaultSchema;
            }

            this.selections.Add(new ColumnSelection
                                {
                                    Alias = alias,
                                    Table = tableName,
                                    Schema = tableSchema,
                                    Name = name
                                });
        }
    }
}