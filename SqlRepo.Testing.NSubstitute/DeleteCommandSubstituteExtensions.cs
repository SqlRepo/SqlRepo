using System;
using System.Linq;
using System.Linq.Expressions;
using NSubstitute;

namespace SqlRepo.Testing.NSubstitute
{
    public static class DeleteCommandSubstituteExtensions
    {
        public static IDeleteCommand<TEntity> DidNotReceiveWhereEquals<TEntity>(
            this IDeleteCommand<TEntity> deleteCommand,
            string property,
            string value) where TEntity: class, new()
        {
            return deleteCommand.DidNotReceive()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)));
        }

        public static IDeleteCommand<TEntity> ReceivedAndEquals<TEntity>(this IDeleteCommand<TEntity> deleteCommand,
            string property,
            string value) where TEntity: class, new()
        {
            return deleteCommand.Received()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)));
        }

        public static IDeleteCommand<TEntity> ReceivedAndGreaterThan<TEntity>(this IDeleteCommand<TEntity> deleteCommand,
            string property,
            string value) where TEntity : class, new()
        {
            return deleteCommand.Received()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, ">", value)));
        }

        public static IDeleteCommand<TEntity> ReceivedAndLessThan<TEntity>(this IDeleteCommand<TEntity> deleteCommand,
            string property,
            string value) where TEntity : class, new()
        {
            return deleteCommand.Received()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "<", value)));
        }

        public static IDeleteCommand<TEntity> ReceivedOrEquals<TEntity>(
            this IDeleteCommand<TEntity> deleteCommand,
            string property,
            string value) where TEntity: class, new()
        {
            return deleteCommand.Received()
                                .Or(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)));
        }

        public static IDeleteCommand<TEntity> ReceivedWhereEquals<TEntity>(
            this IDeleteCommand<TEntity> deleteCommand,
            string property,
            string value) where TEntity: class, new()
        {
            return deleteCommand.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)));
        }

        public static IDeleteCommand<TEntity> ReceivedWhereGreaterThan<TEntity>(
            this IDeleteCommand<TEntity> deleteCommand,
            string property,
            string value) where TEntity: class, new()
        {
            return deleteCommand.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, ">", value)));
        }

        public static IDeleteCommand<TEntity> ReceivedWhereLessThan<TEntity>(
            this IDeleteCommand<TEntity> deleteCommand,
            string property,
            string value) where TEntity: class, new()
        {
            return deleteCommand.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "<", value)));
        }

        public static IDeleteCommand<TEntity> ReceivedWhereIn<TEntity, TMember>(
            this IDeleteCommand<TEntity> deleteCommand,
            string property,
            params TMember[] values) where TEntity: class, new()
        {
            return deleteCommand.Received()
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