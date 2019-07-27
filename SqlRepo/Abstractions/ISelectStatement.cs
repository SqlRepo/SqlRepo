using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SqlRepo.Abstractions
{
    public interface ISelectStatement<TEntity> : ISqlStatement<IEnumerable<TEntity>>
        where TEntity: class, new()
    {
        ISelectStatement<TEntity> And(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectStatement<TEntity> And<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectStatement<TEntity> AndBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectStatement<TEntity> AndBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectStatement<TEntity> AndIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectStatement<TEntity> AndIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectStatement<TEntity> AndOn<TRight>(Expression<Func<TEntity, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null);

        ISelectStatement<TEntity> AndOn<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null);

        ISelectStatement<TEntity> Avg(Expression<Func<TEntity, object>> selector, string alias = null);

        ISelectStatement<TEntity> Avg<T>(Expression<Func<T, object>> selector, string alias = null);

        ISelectStatement<TEntity> Count(Expression<Func<TEntity, object>> selector, string alias = null);

        ISelectStatement<TEntity> Count<T>(Expression<Func<T, object>> selector, string alias = null);

        ISelectStatement<TEntity> CountAll();

        ISelectStatement<TEntity> EndNesting();

        ISelectStatement<TEntity> From(string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectStatement<TEntity> GroupBy<T>(Expression<Func<T, object>> selector,
            string alias = null,
            params Expression<Func<T, object>>[] additionalSelectors);

        ISelectStatement<TEntity> GroupBy(Expression<Func<TEntity, object>> selector,
            string alias = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectStatement<TEntity> HavingAvg<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectStatement<TEntity> HavingAvg(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectStatement<TEntity> HavingCount<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectStatement<TEntity> HavingCount(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectStatement<TEntity> HavingCountAll<T>(Comparison comparison, int @value);
        ISelectStatement<TEntity> HavingCountAll(Comparison comparison, int @value);

        ISelectStatement<TEntity> HavingMax<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectStatement<TEntity> HavingMax(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectStatement<TEntity> HavingMin<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectStatement<TEntity> HavingMin(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectStatement<TEntity> HavingSum<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectStatement<TEntity> HavingSum(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectStatement<TEntity> InnerJoin<TRight>(string alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectStatement<TEntity> LeftOuterJoin<TRight>(string @alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectStatement<TEntity> Max(Expression<Func<TEntity, object>> selector, string alias = null);

        ISelectStatement<TEntity> Max<T>(Expression<Func<T, object>> selector, string alias = null);

        ISelectStatement<TEntity> Min(Expression<Func<TEntity, object>> selector, string alias = null);

        ISelectStatement<TEntity> Min<T>(Expression<Func<T, object>> selector, string alias = null);

        ISelectStatement<TEntity> NestedAnd<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectStatement<TEntity> NestedAnd(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectStatement<TEntity> NestedAndBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectStatement<TEntity> NestedAndBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectStatement<TEntity> NestedAndIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectStatement<TEntity> NestedAndIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectStatement<TEntity> NestedOr<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectStatement<TEntity> NestedOr(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectStatement<TEntity> NestedOrBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectStatement<TEntity> NestedOrBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectStatement<TEntity> NestedOrIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectStatement<TEntity> NestedOrIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectStatement<TEntity> NoLocks();

        ISelectStatement<TEntity> On<TRight>(Expression<Func<TEntity, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null);

        ISelectStatement<TEntity> On<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null);

        ISelectStatement<TEntity> Or<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectStatement<TEntity> Or(Expression<Func<TEntity, bool>> selector, string @alias = null);

        ISelectStatement<TEntity> OrBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectStatement<TEntity> OrBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectStatement<TEntity> OrderBy<T>(Expression<Func<T, object>> selector,
            string alias = null,
            params Expression<Func<T, object>>[] additionalSelectors);

        ISelectStatement<TEntity> OrderBy(Expression<Func<TEntity, object>> selector,
            string alias = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectStatement<TEntity> OrderByDescending<T>(Expression<Func<T, object>> selector,
            string alias = null,
            params Expression<Func<T, object>>[] additionalSelectors);

        ISelectStatement<TEntity> OrderByDescending(Expression<Func<TEntity, object>> selector,
            string alias = null,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectStatement<TEntity> OrIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectStatement<TEntity> OrIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string @alias = null);

        ISelectStatement<TEntity> OrOn<TRight>(Expression<Func<TEntity, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null);

        ISelectStatement<TEntity> OrOn<TLeft, TRight>(Expression<Func<TLeft, TRight, bool>> expression,
            string leftTableAlias = null,
            string rightTableAlias = null);

        ISelectStatement<TEntity> Percent(bool useTopPercent = true);

        ISelectStatement<TEntity> RightOuterJoin<TLeft>(string @alias = null,
            string tableName = null,
            string tableSchema = null);

        ISelectStatement<TEntity> Select(Expression<Func<TEntity, object>> selector,
            string @alias,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectStatement<TEntity> Select<T>(Expression<Func<T, object>> selector,
            string @alias,
            params Expression<Func<T, object>>[] additionalSelectors);

        ISelectStatement<TEntity> Select(Expression<Func<TEntity, object>> selector,
            params Expression<Func<TEntity, object>>[] additionalSelectors);

        ISelectStatement<TEntity> Select<T>(Expression<Func<T, object>> selector,
            params Expression<Func<T, object>>[] additionalSelectors);

        ISelectStatement<TEntity> SelectAll(string alias = null);

        ISelectStatement<TEntity> SelectAll<T>(string alias = null);

        ISelectStatement<TEntity> Sum(Expression<Func<TEntity, object>> selector, string alias = null);

        ISelectStatement<TEntity> Sum<T>(Expression<Func<T, object>> selector, string alias = null);

        ISelectStatement<TEntity> Top(int rows);

        ISelectStatement<TEntity> UsingMappingProfile(IEntityMappingProfile mappingProfile);

        ISelectStatement<TEntity> Where<T>(Expression<Func<T, bool>> selector, string alias = null);

        ISelectStatement<TEntity> Where(Expression<Func<TEntity, bool>> selector, string alias = null);

        ISelectStatement<TEntity> WhereBetween<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectStatement<TEntity> WhereBetween<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember start,
            TMember end,
            string alias = null);

        ISelectStatement<TEntity> WhereIn<T, TMember>(Expression<Func<T, TMember>> selector,
            TMember[] values,
            string alias = null);

        ISelectStatement<TEntity> WhereIn<TMember>(Expression<Func<TEntity, TMember>> selector,
            TMember[] values,
            string alias = null);
    }
}