using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class OrderByClauseBuilder : ClauseBuilder, IOrderByClauseBuilder
    {
        private const string ClauseTemplate = "ORDER BY {0}";
        private readonly IList<OrderBySpecification> orderBySpecifications = new List<OrderBySpecification>();

        public string ActiveAlias { get; private set; }

        public IOrderByClauseBuilder By<TEntity>(Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return this.By(this.ActiveAlias,
                this.TableNameFromType<TEntity>(),
                DefaultSchema,
                selector,
                additionalSelectors);
        }

        public IOrderByClauseBuilder By<TEntity>(string alias,
            string tableName,
            Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return this.By(alias, tableName, DefaultSchema, selector, additionalSelectors);
        }

        public IOrderByClauseBuilder By<TEntity>(string alias,
            string tableName,
            string tableSchema,
            Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            this.AddOrderBySpecification<TEntity>(alias, tableName, tableSchema, this.GetMemberName(selector));
            foreach(var additionalSelector in additionalSelectors)
            {
                this.AddOrderBySpecification<TEntity>(alias,
                    tableName,
                    tableSchema,
                    this.GetMemberName(additionalSelector));
            }
            this.IsClean = false;
            return this;
        }

        public IOrderByClauseBuilder ByDescending<TEntity>(Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return this.ByDescending(this.ActiveAlias,
                this.TableNameFromType<TEntity>(),
                DefaultSchema,
                selector,
                additionalSelectors);
        }

        public IOrderByClauseBuilder ByDescending<TEntity>(string alias,
            Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            return this.ByDescending(alias,
                this.TableNameFromType<TEntity>(),
                DefaultSchema,
                selector,
                additionalSelectors);
        }

        public IOrderByClauseBuilder ByDescending<TEntity>(string alias,
            string tableName,
            string tableSchema,
            Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            this.AddOrderBySpecification<TEntity>(alias,
                tableName,
                tableSchema,
                this.GetMemberName(selector),
                OrderByDirection.Descending);
            foreach(var additionalSelector in additionalSelectors)
            {
                this.AddOrderBySpecification<TEntity>(alias,
                    tableName,
                    tableSchema,
                    this.GetMemberName(additionalSelector),
                    OrderByDirection.Descending);
            }
            this.IsClean = false;
            return this;
        }

        public IOrderByClauseBuilder FromScratch()
        {
            this.orderBySpecifications.Clear();
            this.IsClean = true;
            return this;
        }

        public override string Sql()
        {
            return this.orderBySpecifications.Any()
                       ? string.Format(ClauseTemplate, string.Join(", ", this.orderBySpecifications))
                       : string.Empty;
        }

        public IOrderByClauseBuilder UsingAlias(string alias)
        {
            this.ActiveAlias = alias;
            return this;
        }

        private void AddOrderBySpecification<TEntity>(string alias,
            string tableName,
            string tableSchema,
            string name,
            OrderByDirection direction = OrderByDirection.Ascending)
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

            this.orderBySpecifications.Add(new OrderBySpecification
                                           {
                                               Alias = alias,
                                               Table = tableName,
                                               Schema = tableSchema,
                                               Name = name,
                                               Direction = direction
                                           });
        }
    }
}