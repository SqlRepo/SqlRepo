using System;
using System.Linq.Expressions;

namespace SqlRepo.SqlServer.Abstractions
{
    public interface ISelectClauseBuilder : IClauseBuilder
    {
        ISelectClauseBuilder Avg<TEntity>(Expression<Func<TEntity, object>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectClauseBuilder Count<TEntity>(Expression<Func<TEntity, object>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectClauseBuilder CountAll();

        ISelectClauseBuilder For<TEntity>(TEntity entity,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectClauseBuilder Max<TEntity>(Expression<Func<TEntity, object>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectClauseBuilder Min<TEntity>(Expression<Func<TEntity, object>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectClauseBuilder Select<TEntity>(Expression<Func<TEntity, object>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectClauseBuilder SelectAll<TEntity>(string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectClauseBuilder Sum<TEntity>(Expression<Func<TEntity, object>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectClauseBuilder Top(int rows);
    }
}