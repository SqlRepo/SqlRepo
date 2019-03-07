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
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string value,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.DidNotReceive()
                                .And(Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> DidNotReceiveAndEquals<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null)
            where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.DidNotReceiveAndEquals(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> DidNotReceiveWhereEquals<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null)
            where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.DidNotReceiveWhereEquals(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> DidNotReceiveWhereEquals<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string value,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.DidNotReceive()
                                .Where(Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndBetween<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember start,
            TMember end,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .AndBetween<TMember>(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(
                                        e => e.HasMemberName(property)),
                                    start,
                                    end,
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndEquals<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null)
            where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedAndEquals(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndEquals<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string value,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .And(Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndGreaterThan<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string value,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .And(Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, ">", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndGreaterThan<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null)
            where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedAndGreaterThan(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndIn<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember[] values,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .AndIn<TEntity, TMember>(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(
                                        e => e.HasMemberName(property)),
                                    Arg.Is<TMember[]>(e => AssertMatch(values, e)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndLessThan<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string value,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .And(Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "<", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndLessThan<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null)
            where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedAndLessThan(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndLessThanOrEqualTo<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string value,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .And(Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "<=", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedAndLessThanOrEqualTo<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null)
            where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedAndLessThanOrEqualTo(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedGroupBy<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .GroupBy(Arg.Is<Expression<Func<TEntity, object>>>(
                                        e => e.HasMemberName(property)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedInnerJoin<TEntity, TRight>(
            this ISelectStatement<TEntity> selectCommand,
            string rightTableAlias = null,
            string rightTableName = null,
            string rightTableSchema = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .InnerJoin<TRight>(rightTableAlias, rightTableName, rightTableSchema);
        }

        public static ISelectStatement<TEntity> ReceivedOrBetween<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember start,
            TMember end,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .OrBetween<TMember>(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(
                                        e => e.HasMemberName(property)),
                                    start,
                                    end,
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedOrderBy<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .OrderBy(Arg.Is<Expression<Func<TEntity, object>>>(
                                    e => e.HasMemberName(property)));
        }

        public static ISelectStatement<TEntity> ReceivedOrderBy<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string alias)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .OrderBy(Arg.Is<Expression<Func<TEntity, object>>>(
                                        e => e.HasMemberName(property)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedOrderByDescending<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .OrderByDescending(
                                    Arg.Is<Expression<Func<TEntity, object>>>(
                                        e => e.HasMemberName(property)));
        }

        public static ISelectStatement<TEntity> ReceivedOrEquals<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string value,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .Or<TEntity>(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedOrEquals<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null)
            where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedOrEquals(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedSelect<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .Select(Arg.Is<Expression<Func<TEntity, object>>>(
                                        e => e.HasMemberName(property)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedSelectAll<TEntity>(
            this ISelectStatement<TEntity> selectCommand)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .SelectAll();
        }

        public static ISelectStatement<TEntity> ReceivedSum<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .Sum(Arg.Is<Expression<Func<TEntity, object>>>(
                                        e => e.HasMemberName(property)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereBetween<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember start,
            TMember end,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .WhereBetween<TMember>(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(
                                        e => e.HasMemberName(property)),
                                    start,
                                    end,
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereEquals<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string value,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .Where(Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereEquals<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null)
            where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedWhereEquals(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereGreaterThan<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string value,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .Where(Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, ">", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereGreaterThan<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null)
            where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedWhereGreaterThan(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereGreaterThanOrEqualTo<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string value,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .Where(Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, ">=", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereGreaterThanOrEqualTo<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null)
            where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedWhereGreaterThanOrEqualTo(property, compareTo, alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereIn<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember[] values,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .WhereIn<TMember>(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(
                                        e => e.HasMemberName(property)),
                                    Arg.Is<TMember[]>(e => AssertMatch(values, e)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereLessThan<TEntity>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            string value,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.Received()
                                .Where(Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "<", value)),
                                    alias);
        }

        public static ISelectStatement<TEntity> ReceivedWhereLessThan<TEntity, TMember>(
            this ISelectStatement<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null)
            where TEntity: class, new()
        {
            return selectCommand.ReceivedWhereLessThan(property, value.ToString(), alias);
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