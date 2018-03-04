using System;
using System.Linq;
using System.Linq.Expressions;
using NSubstitute;
using SqlRepo.Abstractions;

namespace SqlRepo.Testing.NSubstitute
{
    public static class DeleteStatementSubstituteExtensions
    {
        public static IDeleteStatement<TEntity> DidNotReceiveWhereEquals<TEntity>(
            this IDeleteStatement<TEntity> deleteStatement,
            string property,
            string value) where TEntity: class, new()
        {
            return deleteStatement.DidNotReceive()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)));
        }

        public static IDeleteStatement<TEntity> ReceivedAndEquals<TEntity>(this IDeleteStatement<TEntity> deleteStatement,
            string property,
            string value) where TEntity: class, new()
        {
            return deleteStatement.Received()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)));
        }

        public static IDeleteStatement<TEntity> ReceivedAndGreaterThan<TEntity>(this IDeleteStatement<TEntity> deleteStatement,
            string property,
            string value) where TEntity : class, new()
        {
            return deleteStatement.Received()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, ">", value)));
        }

        public static IDeleteStatement<TEntity> ReceivedAndLessThan<TEntity>(this IDeleteStatement<TEntity> deleteStatement,
            string property,
            string value) where TEntity : class, new()
        {
            return deleteStatement.Received()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "<", value)));
        }

        public static IDeleteStatement<TEntity> ReceivedOrEquals<TEntity>(
            this IDeleteStatement<TEntity> deleteStatement,
            string property,
            string value) where TEntity: class, new()
        {
            return deleteStatement.Received()
                                .Or(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)));
        }

        public static IDeleteStatement<TEntity> ReceivedWhereEquals<TEntity>(
            this IDeleteStatement<TEntity> deleteStatement,
            string property,
            string value) where TEntity: class, new()
        {
            return deleteStatement.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)));
        }

        public static IDeleteStatement<TEntity> ReceivedWhereGreaterThan<TEntity>(
            this IDeleteStatement<TEntity> deleteStatement,
            string property,
            string value) where TEntity: class, new()
        {
            return deleteStatement.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, ">", value)));
        }

        public static IDeleteStatement<TEntity> ReceivedWhereLessThan<TEntity>(
            this IDeleteStatement<TEntity> deleteStatement,
            string property,
            string value) where TEntity: class, new()
        {
            return deleteStatement.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "<", value)));
        }

        public static IDeleteStatement<TEntity> ReceivedWhereIn<TEntity, TMember>(
            this IDeleteStatement<TEntity> deleteStatement,
            string property,
            params TMember[] values) where TEntity: class, new()
        {
            return deleteStatement.Received()
                                .WhereIn(
                                    Arg.Is<Expression<Func<TEntity, TMember>>>(
                                        e => e.HasMemberName(property)), Arg.Is<TMember[]>(e => AssertMatch(values, e)));
        }

        private static bool AssertMatch<T>(T[] expected, T[] actual)
        {
            if(expected.Length != actual.Length)
            {
                return false;
            }

            return !expected.Where((t, i) => !t.Equals(actual[i]))
                            .Any();
        }
    }
}