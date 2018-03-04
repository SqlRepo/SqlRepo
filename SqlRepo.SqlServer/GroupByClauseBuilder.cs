using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer
{
    public class GroupByClauseBuilder : ClauseBuilder, IGroupByClauseBuilder
    {
        private const string ClauseTemplate = "GROUP BY {0}";
        private const string HavlingClauseTemplate = "\nHAVING {0}";
        private readonly IList<GroupBySpecification> groupBySpecifications = new List<GroupBySpecification>();
        private readonly IList<HavingSpecification> havingSpecifications = new List<HavingSpecification>();

        public IGroupByClauseBuilder By<TEntity>(Expression<Func<TEntity, object>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors)
        {
            this.AddGroupBySpecification<TEntity>(alias, tableName, tableSchema, this.GetMemberName(selector));
            foreach(var additionalSelector in additionalSelectors)
            {
                this.AddGroupBySpecification<TEntity>(alias,
                    tableName,
                    tableSchema,
                    this.GetMemberName(additionalSelector));
            }
            this.IsClean = false;
            return this;
        }

        public IGroupByClauseBuilder HavingAvg<TEntity>(Expression<Func<TEntity, object>> selector,
            Comparison comparison,
            int value,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.AddHavingSpecification<TEntity>(alias,
                tableName,
                tableSchema,
                this.GetMemberName(selector),
                Aggregation.Avg,
                comparison,
                value);
            return this;
        }

        public IGroupByClauseBuilder HavingCount<TEntity>(Expression<Func<TEntity, object>> selector,
            Comparison comparison,
            int value,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.AddHavingSpecification<TEntity>(alias,
                tableName,
                tableSchema,
                this.GetMemberName(selector),
                Aggregation.Count,
                comparison,
                value);
            return this;
        }

        public IGroupByClauseBuilder HavingCountAll<TEntity>(Comparison comparison, int value)
        {
            this.AddHavingSpecification<TEntity>(null, null, null, "*", Aggregation.Count, comparison, value);
            return this;
        }

        public IGroupByClauseBuilder HavingMax<TEntity>(Expression<Func<TEntity, object>> selector,
            Comparison comparison,
            int value,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.AddHavingSpecification<TEntity>(alias,
                tableName,
                tableSchema,
                this.GetMemberName(selector),
                Aggregation.Max,
                comparison,
                value);
            return this;
        }

        public IGroupByClauseBuilder HavingMin<TEntity>(Expression<Func<TEntity, object>> selector,
            Comparison comparison,
            int value,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.AddHavingSpecification<TEntity>(alias,
                tableName,
                tableSchema,
                this.GetMemberName(selector),
                Aggregation.Min,
                comparison,
                value);
            return this;
        }

        public IGroupByClauseBuilder HavingSum<TEntity>(Expression<Func<TEntity, object>> selector,
            Comparison comparison,
            int value,
            string alias = null,
            string tableName = null,
            string tableSchema = null)
        {
            this.AddHavingSpecification<TEntity>(alias,
                tableName,
                tableSchema,
                this.GetMemberName(selector),
                Aggregation.Sum,
                comparison,
                value);
            return this;
        }

        public override string Sql()
        {
            if(!this.groupBySpecifications.Any())
            {
                return string.Empty;
            }

            var result = string.Format(ClauseTemplate, string.Join(", ", this.groupBySpecifications));

            if(this.havingSpecifications.Any())
            {
                result += string.Format(HavlingClauseTemplate, string.Join(", ", this.havingSpecifications));
            }
            return result;
        }

        private void AddGroupBySpecification<TEntity>(string alias,
            string tableName,
            string tableSchema,
            string name)
        {
            if(string.IsNullOrWhiteSpace(tableName))
            {
                tableName = this.TableNameFromType<TEntity>();
            }

            if(string.IsNullOrWhiteSpace(tableSchema))
            {
                tableSchema = DefaultSchema;
            }

            this.groupBySpecifications.Add(new GroupBySpecification
                                           {
                                               Alias = alias,
                                               Table = tableName,
                                               Schema = tableSchema,
                                               Name = name
                                           });
        }

        private void AddHavingSpecification<TEntity>(string alias,
            string tableName,
            string tableSchema,
            string name,
            Aggregation aggregation,
            Comparison comparison,
            object @value)
        {
            if(string.IsNullOrWhiteSpace(tableName))
            {
                tableName = this.TableNameFromType<TEntity>();
            }

            if(string.IsNullOrWhiteSpace(tableSchema))
            {
                tableSchema = DefaultSchema;
            }

            this.havingSpecifications.Add(new HavingSpecification
                                          {
                                              Aggregation = aggregation,
                                              Alias = alias,
                                              Table = tableName,
                                              Schema = tableSchema,
                                              Name = name,
                                              Comparison = comparison,
                                              Value = @value
                                          });
        }
    }
}