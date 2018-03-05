using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NSubstitute;
using SqlRepo.Abstractions;

namespace SqlRepo.Testing.NSubstitute
{
    public static class SelectStatementSubstituteExtensions
    {
        public static ISelectStatement<TEntity> DidNotReceiveAndEquals<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.DidNotReceive()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> DidNotReceiveAndEquals<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectStatement.DidNotReceiveAndEquals(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> DidNotReceiveWhereEquals<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectStatement.DidNotReceiveWhereEquals(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> DidNotReceiveWhereEquals<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.DidNotReceive()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndBetween<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember start,
            TMember end,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .AndBetween<TMember>(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                                    start,
                                    end,
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndEquals<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectStatement.ReceivedAndEquals(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndEquals<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndGreaterThan<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, ">", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndGreaterThan<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectStatement.ReceivedAndGreaterThan(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndIn<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember[] values,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .AndIn<TEntity, TMember>(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                                    Arg.Is<TMember[]>(e => AssertMatch(values, e)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndLessThan<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "<", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndLessThan<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectStatement.ReceivedAndLessThan(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndLessThanOrEqualTo<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "<=", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndLessThanOrEqualTo<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectStatement.ReceivedAndLessThanOrEqualTo(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedGroupBy<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .GroupBy(
                                    Arg.Is<Expression<Func<TEntity, object>>>(e => e.HasMemberName(property)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedInnerJoin<TEntity, TRight>(
            this ISelectStatement<TEntity> selectStatement,
            string rightTableAlias = null,
            string rightTableName = null,
            string rightTableSchema = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .InnerJoin<TRight>(rightTableAlias, rightTableName, rightTableSchema);
        }

        public static ISelectStatement<TEntity> ReceivedOrBetween<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember start,
            TMember end,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .OrBetween<TMember>(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                                    start,
                                    end,
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedOrderBy<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .OrderBy(
                                    Arg.Is<Expression<Func<TEntity, object>>>(e => e.HasMemberName(property)));
        }

        public static ISelectStatement<TEntity> ReceivedOrderBy<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            string alias) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .OrderBy(
                                    Arg.Is<Expression<Func<TEntity, object>>>(e => e.HasMemberName(property)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedOrderByDescending<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .OrderByDescending(
                                    Arg.Is<Expression<Func<TEntity, object>>>(e => e.HasMemberName(property)));
        }

        public static ISelectStatement<TEntity> ReceivedOrEquals<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .Or<TEntity>(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedOrEquals<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectStatement.ReceivedOrEquals(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedSelect<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            string alias) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .Select(
                                    Arg.Is<Expression<Func<TEntity, object>>>(e => e.HasMemberName(property)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedSelect<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property)
            where TEntity: class, new()
        {
            return selectStatement.Received()
                                  .Select(Arg.Is<Expression<Func<TEntity, object>>>(
                                          e => e.HasMemberName(property)));
        }

        public static ISelectStatement<TEntity> ReceivedSelectAll<TEntity>(
            this ISelectStatement<TEntity> selectStatement) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .SelectAll();
        }

        public static ISelectStatement<TEntity> ReceivedSum<TEntity>(this ISelectStatement<TEntity> selectStatement,
            string property,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .Sum(
                                    Arg.Is<Expression<Func<TEntity, object>>>(e => e.HasMemberName(property)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereBetween<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember start,
            TMember end,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .WhereBetween<TMember>(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                                    start,
                                    end,
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereEquals<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereEquals<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectStatement.ReceivedWhereEquals(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereGreaterThan<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, ">", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereGreaterThan<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectStatement.ReceivedWhereGreaterThan(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereGreaterThanOrEqualTo<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, ">=", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereGreaterThanOrEqualTo<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectStatement.ReceivedWhereGreaterThanOrEqualTo(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereIn<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember[] values,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .WhereIn<TMember>(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                                    Arg.Is<TMember[]>(e => AssertMatch(values, e)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereLessThan<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "<", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereLessThan<TEntity, TMember>(
            this ISelectStatement<TEntity> selectStatement,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            return selectStatement.ReceivedWhereLessThan(property, value.ToString(), alias);
        }

        private static bool AssertMatch<T>(IReadOnlyCollection<T> expected, IReadOnlyList<T> actual)
        {
            if(expected.Count != actual.Count)
            {
                return false;
            }

            return !expected.Where((t, i) => !t.Equals(actual[i]))
                            .Any();
        }
    }
}