using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SqlRepo
{
    public interface ISelectCommand<TEntity> : ISqlCommand<IEnumerable<TEntity>>
        where TEntity: class, new()
    {
        ISelectCommand<TEntity> And<T>(Expression<Func<T, bool>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectCommand<TEntity> And(Expression<Func<TEntity, bool>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null);
        
        ISelectCommand<TEntity> AndIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectCommand<TEntity> EndNesting();

        ISelectCommand<TEntity> InnerJoin<TLeft, TRight>(string leftTableAlias = null,
            string rightTableAlias = null,
            string rightTableName = null,
            string rightTableSchema = null);

        ISelectCommand<TEntity> LeftOuterJoin<TLeft, TRight>(string leftTableAlias = null,
            string rightTableAlias = null,
            string rightTableName = null,
            string rightTableSchema = null);

        ISelectCommand<TEntity> NestedAnd<T>(Expression<Func<T, bool>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectCommand<TEntity> NestedOr<T>(Expression<Func<T, bool>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectCommand<TEntity> On<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null);

        ISelectCommand<TEntity> Or<T>(Expression<Func<T, bool>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectCommand<TEntity> OrderBy<T>(Expression<Func<T, object>> selector,
            params Expression<Func<T, object>>[] additionalSelectors);

        ISelectCommand<TEntity> OrderByDescending<T>(Expression<Func<T, object>> expression,
            params Expression<Func<T, object>>[] additionalSelectors);

        ISelectCommand<TEntity> OrIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectCommand<TEntity> RightOuterJoin<TLeft, TRight>(string leftTableAlias = null,
            string rightTableAlias = null,
            string rightTableName = null,
            string rightTableSchema = null);

        ISelectCommand<TEntity> Select(Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectCommand<TEntity> Select(string alias,
            Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectCommand<TEntity> SelectAll<T>(string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectCommand<TEntity> Top(int rows);
        ISelectCommand<TEntity> UsingTableName(string tableName);
        ISelectCommand<TEntity> UsingTableSchema(string tableSchema);

        ISelectCommand<TEntity> Where<T>(Expression<Func<T, bool>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectCommand<TEntity> Where(Expression<Func<TEntity, bool>> selector,
            string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectCommand<TEntity> WhereIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null,
            string tableName = null,
            string tableSchema = null);
    }
}