using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NSubstitute;

namespace SqlRepo.Testing.NSubstitute
{
    public static class SelectCommandSubstituteExtensions
    {
        public static ISelectCommand<TEntity> DidNotReceiveAndEquals<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.DidNotReceive()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)),
                                    alias);
        }

        public static ISelectCommand<TEntity> DidNotReceiveAndEquals<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.DidNotReceiveAndEquals(property, compareTo, alias);
        }

        public static ISelectCommand<TEntity> DidNotReceiveWhereEquals<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.DidNotReceiveWhereEquals(property, compareTo, alias);
        }

        public static ISelectCommand<TEntity> DidNotReceiveWhereEquals<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.DidNotReceive()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedAndBetween<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember start,
            TMember end,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .AndBetween<TMember>(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                                    start,
                                    end,
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedAndEquals<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedAndEquals(property, compareTo, alias);
        }

        public static ISelectCommand<TEntity> ReceivedAndEquals<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedAndGreaterThan<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, ">", value)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedAndGreaterThan<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedAndGreaterThan(property, compareTo, alias);
        }

        public static ISelectCommand<TEntity> ReceivedAndIn<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember[] values,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .AndIn<TEntity, TMember>(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                                    Arg.Is<TMember[]>(e => AssertMatch(values, e)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedAndLessThan<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "<", value)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedAndLessThan<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedAndLessThan(property, compareTo, alias);
        }

        public static ISelectCommand<TEntity> ReceivedAndLessThanOrEqualTo<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "<=", value)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedAndLessThanOrEqualTo<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedAndLessThanOrEqualTo(property, compareTo, alias);
        }

        public static ISelectCommand<TEntity> ReceivedGroupBy<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .GroupBy(
                                    Arg.Is<Expression<Func<TEntity, object>>>(e => e.HasMemberName(property)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedInnerJoin<TEntity, TRight>(
            this ISelectCommand<TEntity> selectCommand,
            string rightTableAlias = null,
            string rightTableName = null,
            string rightTableSchema = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .InnerJoin<TRight>(rightTableAlias, rightTableName, rightTableSchema);
        }

        public static ISelectCommand<TEntity> ReceivedOrBetween<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember start,
            TMember end,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .OrBetween<TMember>(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                                    start,
                                    end,
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedOrderBy<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .OrderBy(
                                    Arg.Is<Expression<Func<TEntity, object>>>(e => e.HasMemberName(property)));
        }

        public static ISelectCommand<TEntity> ReceivedOrderBy<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            string alias) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .OrderBy(
                                    Arg.Is<Expression<Func<TEntity, object>>>(e => e.HasMemberName(property)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedOrderByDescending<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .OrderByDescending(
                                    Arg.Is<Expression<Func<TEntity, object>>>(e => e.HasMemberName(property)));
        }

        public static ISelectCommand<TEntity> ReceivedOrEquals<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .Or<TEntity>(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedOrEquals<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedOrEquals(property, compareTo, alias);
        }

        public static ISelectCommand<TEntity> ReceivedSelect<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .Select(
                                    Arg.Is<Expression<Func<TEntity, object>>>(e => e.HasMemberName(property)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedSelectAll<TEntity>(
            this ISelectCommand<TEntity> selectCommand) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .SelectAll();
        }

        public static ISelectCommand<TEntity> ReceivedSum<TEntity>(this ISelectCommand<TEntity> selectCommand,
            string property,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .Sum(
                                    Arg.Is<Expression<Func<TEntity, object>>>(e => e.HasMemberName(property)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedWhereBetween<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember start,
            TMember end,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .WhereBetween<TMember>(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                                    start,
                                    end,
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedWhereEquals<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedWhereEquals<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedWhereEquals(property, compareTo, alias);
        }

        public static ISelectCommand<TEntity> ReceivedWhereGreaterThan<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, ">", value)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedWhereGreaterThan<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedWhereGreaterThan(property, compareTo, alias);
        }

        public static ISelectCommand<TEntity> ReceivedWhereGreaterThanOrEqualTo<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, ">=", value)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedWhereGreaterThanOrEqualTo<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
        {
            var compareTo = value.ToString();
            return selectCommand.ReceivedWhereGreaterThanOrEqualTo(property, compareTo, alias);
        }

        public static ISelectCommand<TEntity> ReceivedWhereIn<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember[] values,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .WhereIn<TMember>(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                                    Arg.Is<TMember[]>(e => AssertMatch(values, e)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedWhereLessThan<TEntity>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return selectCommand.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "<", value)),
                                    alias);
        }

        public static ISelectCommand<TEntity> ReceivedWhereLessThan<TEntity, TMember>(
            this ISelectCommand<TEntity> selectCommand,
            string property,
            TMember value,
            string alias = null) where TEntity: class, new()
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