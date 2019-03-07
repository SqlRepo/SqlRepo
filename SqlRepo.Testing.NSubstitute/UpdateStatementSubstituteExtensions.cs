using System;
using System.Linq.Expressions;
using NSubstitute;
using SqlRepo.Abstractions;

namespace SqlRepo.Testing.NSubstitute
{
    public static class UpdateStatementSubstituteExtensions
    {
        public static IUpdateStatement<TEntity> DidNotReceiveWhereEquals<TEntity>(
            this IUpdateStatement<TEntity> updateCommand,
            string property,
            string value) where TEntity: class, new()
        {
            return updateCommand.DidNotReceive()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)));
        }

        public static IUpdateStatement<TEntity> ReceivedOrEquals<TEntity>(
            this IUpdateStatement<TEntity> updateCommand,
            string property,
            string value,
            string alias = null) where TEntity: class, new()
        {
            return updateCommand.Received()
                                .Or(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)));
        }

        public static IUpdateStatement<TEntity> ReceivedSet<TEntity, TMember>(
            this IUpdateStatement<TEntity> command,
            string property,
            TMember value) where TEntity: class, new()
        {
            return command.Received()
                          .Set(Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                              value);
        }

        public static IUpdateStatement<TEntity> ReceivedSet<TEntity, TMember>(
            this IUpdateStatement<TEntity> command,
            string property,
            TMember value, int expectedCalls) where TEntity: class, new()
        {
            return command.Received(expectedCalls)
                          .Set(Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                              value);
        }

        public static IUpdateStatement<TEntity> ReceivedWhereEquals<TEntity>(
            this IUpdateStatement<TEntity> updateCommand,
            string property,
            string value) where TEntity: class, new()
        {
            return updateCommand.Received()
                                .Where(
                                    Arg.Is<Expression<Func<TEntity, bool>>>(
                                        e => e.IsComparisonWith(property, "=", value)));
        }

        public static IUpdateStatement<TEntity> ReceivedAndEquals<TEntity>(this IUpdateStatement<TEntity> updateCommand,
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