using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SqlRepo
{
    public interface ISelectCommand<TEntity> : ISqlCommand<IEnumerable<TEntity>>
        where TEntity: class, new()
    {

        ISelectCommand<TEntity> And(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectCommand<TEntity> And<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectCommand<TEntity> AndBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectCommand<TEntity> AndBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectCommand<TEntity> AndIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectCommand<TEntity> AndIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectCommand<TEntity> AndOn<TRight>(Expression<Func<TEntity, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null);

        ISelectCommand<TEntity> AndOn<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null);

        ISelectCommand<TEntity> Avg(Expression<Func<TEntity, object>> selector, string alias = null);

        ISelectCommand<TEntity> Avg<T>(Expression<Func<T, object>> selector, string alias = null);

        ISelectCommand<TEntity> Count(Expression<Func<TEntity, object>> selector, string alias = null);

        ISelectCommand<TEntity> Count<T>(Expression<Func<T, object>> selector, string alias = null);

        ISelectCommand<TEntity> CountAll();

        ISelectCommand<TEntity> EndNesting();

        ISelectCommand<TEntity> From(string alias = null, string tableName = null, string tableSchema = null);

        ISelectCommand<TEntity> GroupBy<T>(Expression<Func<T, object>> selector,
            string alias = null,
            params Expression<Func<T, object>>[] additionalSelectors);

        ISelectCommand<TEntity> GroupBy(Expression<Func<TEntity, object>> selector,
            string alias = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectCommand<TEntity> HavingAvg<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectCommand<TEntity> HavingAvg(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectCommand<TEntity> HavingCount<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectCommand<TEntity> HavingCount(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectCommand<TEntity> HavingCountAll<T>(Comparison comparison, int @value);
        ISelectCommand<TEntity> HavingCountAll(Comparison comparison, int @value);

        ISelectCommand<TEntity> HavingMax<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectCommand<TEntity> HavingMax(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectCommand<TEntity> HavingMin<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectCommand<TEntity> HavingMin(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectCommand<TEntity> HavingSum<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectCommand<TEntity> HavingSum(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectCommand<TEntity> InnerJoin<TRight>(string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectCommand<TEntity> LeftOuterJoin<TRight>(string @alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectCommand<TEntity> Max(Expression<Func<TEntity, object>> selector, string alias = null);

        ISelectCommand<TEntity> Max<T>(Expression<Func<T, object>> selector, string alias = null);

        ISelectCommand<TEntity> Min(Expression<Func<TEntity, object>> selector, string alias = null);

        ISelectCommand<TEntity> Min<T>(Expression<Func<T, object>> selector, string alias = null);

        ISelectCommand<TEntity> NestedAnd<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectCommand<TEntity> NestedAnd(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectCommand<TEntity> NestedAndBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectCommand<TEntity> NestedAndBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectCommand<TEntity> NestedAndIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectCommand<TEntity> NestedAndIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectCommand<TEntity> NestedOr<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectCommand<TEntity> NestedOr(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectCommand<TEntity> NestedOrBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectCommand<TEntity> NestedOrBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectCommand<TEntity> NestedOrIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectCommand<TEntity> NestedOrIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectCommand<TEntity> NoLocks();

        ISelectCommand<TEntity> On<TRight>(Expression<Func<TEntity, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null);

        ISelectCommand<TEntity> On<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null);

        ISelectCommand<TEntity> Or<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectCommand<TEntity> Or(Expression<Func<TEntity, bool>> selector, string @alias = null);

        ISelectCommand<TEntity> OrBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectCommand<TEntity> OrBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectCommand<TEntity> OrderBy<T>(Expression<Func<T, object>> selector,
            string alias = null,
            params Expression<Func<T, object>>[] additionalSelectors);

        ISelectCommand<TEntity> OrderBy(Expression<Func<TEntity, object>> selector,
            string alias = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectCommand<TEntity> OrderByDescending<T>(Expression<Func<T, object>> selector,
            string alias = null,
            params Expression<Func<T, object>>[] additionalSelectors);

        ISelectCommand<TEntity> OrderByDescending(Expression<Func<TEntity, object>> selector,
            string alias = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectCommand<TEntity> OrIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectCommand<TEntity> OrIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string @alias = null);

        ISelectCommand<TEntity> OrOn<TRight>(Expression<Func<TEntity, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null);

        ISelectCommand<TEntity> OrOn<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null);

        ISelectCommand<TEntity> Percent(bool useTopPercent = true);

        ISelectCommand<TEntity> RightOuterJoin<TLeft>(string @alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectCommand<TEntity> Select(Expression<Func<TEntity, object>> selector,
            string alias = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectCommand<TEntity> Select<T>(Expression<Func<T, object>> selector,
            string alias = null,
            params Expression<Func<T, object>>[] additionalSelectors);

        ISelectCommand<TEntity> SelectAll(string alias = null);

        ISelectCommand<TEntity> SelectAll<T>(string alias = null);

        ISelectCommand<TEntity> Sum(Expression<Func<TEntity, object>> selector, string alias = null);

        ISelectCommand<TEntity> Sum<T>(Expression<Func<T, object>> selector, string alias = null);

        ISelectCommand<TEntity> Top(int rows);

        ISelectCommand<TEntity> Where<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectCommand<TEntity> Where(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectCommand<TEntity> WhereBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectCommand<TEntity> WhereBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectCommand<TEntity> WhereIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectCommand<TEntity> WhereIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null);
    }
}