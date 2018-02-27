using System;
using System.Linq.Expressions;
using NSubstitute;

namespace SqlRepo.Testing.FluentAssertions
{
    public static class UpdateCommandSubstituteExtensions
    {
        public static IUpdateCommand<TEntity> DidNotReceiveWhereEquals<TEntity>(
            this IUpdateCommand<TEntity> updateCommand,
            string property,
            string value) where TEntity: class, new()
        {
            return updateCommand.DidNotReceive()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)));
        }

        public static IUpdateCommand<TEntity> ReceivedOrEquals<TEntity>(
            this IUpdateCommand<TEntity> updateCommand,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return updateCommand.Received()
                                .Or(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)));
        }

        public static IUpdateCommand<TEntity> ReceivedSet<TEntity, TMember>(
            this IUpdateCommand<TEntity> command,
            string property,
            TMember value) where TEntity: class, new()
        {
            return command.Received()
                          .Set(Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                              value);
        }

        public static IUpdateCommand<TEntity> ReceivedSet<TEntity, TMember>(
            this IUpdateCommand<TEntity> command,
            string property,
            TMember value, int expectedCalls) where TEntity: class, new()
        {
            return command.Received(expectedCalls)
                          .Set(Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                              value);
        }

        public static IUpdateCommand<TEntity> ReceivedWhereEquals<TEntity>(
            this IUpdateCommand<TEntity> updateCommand,
            string property,
            string value) where TEntity: class, new()
        {
            return updateCommand.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)));
        }

        public static IUpdateCommand<TEntity> ReceivedAndEquals<TEntity>(this IUpdateCommand<TEntity> updateCommand,
            string property,
            string value) where TEntity: class, new()
        {
            return updateCommand.Received()
                                .And(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)));
        }
    }
}