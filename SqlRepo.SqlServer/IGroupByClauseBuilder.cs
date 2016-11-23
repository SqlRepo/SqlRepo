using System;
using System.Linq.Expressions;

namespace SqlRepo.SqlServer
{
    public interface IGroupByClauseBuilder : IClauseBuilder
    {
        IGroupByClauseBuilder By<TEntity>(Expression<Func<TEntity, object>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        IGroupByClauseBuilder HavingCount<TEntity>(Expression<Func<TEntity, object>> selector,
            Comparison comparison,
            int @value,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        IGroupByClauseBuilder HavingCountAll<TEntity>(Comparison comparison,
            int @value);

        IGroupByClauseBuilder HavingAvg<TEntity>(Expression<Func<TEntity, object>> selector,
            Comparison comparison,
            int value,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        IGroupByClauseBuilder HavingMax<TEntity>(Expression<Func<TEntity, object>> selector,
            Comparison comparison,
            int value,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        IGroupByClauseBuilder HavingMin<TEntity>(Expression<Func<TEntity, object>> selector,
            Comparison comparison,
            int value,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        IGroupByClauseBuilder HavingSum<TEntity>(Expression<Func<TEntity, object>> selector,
            Comparison comparison,
            int value,
            string alias = null,
            string tableName = null,
            string tableSchema = null);
    }
}