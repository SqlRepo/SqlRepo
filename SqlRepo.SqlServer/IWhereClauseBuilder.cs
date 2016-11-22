using System;
using System.Linq.Expressions;

namespace SqlRepo.SqlServer
{
    public interface IWhereClauseBuilder : IClauseBuilder
    {
        string ActiveAlias { get; }

        IWhereClauseBuilder And<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        IWhereClauseBuilder AndIn<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        IWhereClauseBuilder EndNesting();
        IWhereClauseBuilder FromScratch();

        IWhereClauseBuilder NestedAnd<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        IWhereClauseBuilder NestedOr<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        IWhereClauseBuilder Or<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        IWhereClauseBuilder OrIn<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        IWhereClauseBuilder UsingAlias(string alias);

        IWhereClauseBuilder Where<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        IWhereClauseBuilder WhereIn<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null);
    }
}