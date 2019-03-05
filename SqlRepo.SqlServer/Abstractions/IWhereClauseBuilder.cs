using System;
using System.Linq.Expressions;
using SqlRepo.Abstractions;

namespace SqlRepo.SqlServer.Abstractions
{
    public interface IWhereClauseBuilder : IClauseBuilder
    {
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

        IWhereClauseBuilder Where<TEntity>(Expression<Func<TEntity, bool>> expression,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        IWhereClauseBuilder WhereIn<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        IWhereClauseBuilder WhereBetween<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        IWhereClauseBuilder AndBetween<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        IWhereClauseBuilder OrBetween<TEntity, TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null,
            string tableName = null,
            string tableSchema = null);
    }
}