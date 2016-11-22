using System;
using System.Linq.Expressions;

namespace SqlRepo.SqlServer
{
    public interface ISelectClauseBuilder : IClauseBuilder
    {
        string ActiveAlias { get; }

        ISelectClauseBuilder For<TEntity>(TEntity entity,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectClauseBuilder FromScratch();

        ISelectClauseBuilder Select<TEntity>(Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectClauseBuilder Select<TEntity>(string alias,
            Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectClauseBuilder Select<TEntity>(string alias,
            string tableName,
            Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectClauseBuilder Select<TEntity>(string alias,
            string tableName,
            string tableSchema,
            Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectClauseBuilder SelectAll<TEntity>(string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectClauseBuilder Top(int rows);

        ISelectClauseBuilder UsingAlias(string alias);
    }
}