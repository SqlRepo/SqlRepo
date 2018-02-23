using System;
using System.Linq.Expressions;
using NSubstitute;

namespace SqlRepo.Testing.FluentAssertions
{
    public static class InsertCommandSubstituteExtensions
    {
        public static IInsertCommand<TEntity> ReceivedWith<TEntity, TMember>(
            this IInsertCommand<TEntity> command,
            string property,
            TMember value) where TEntity: class, new()
        {
            return command.Received()
                          .With(Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                              value);
        }

        public static IInsertCommand<TEntity> ReceivedWith<TEntity, TMember>(
            this IInsertCommand<TEntity> command,
            string property,
            TMember value,
            int expectedCalls) where TEntity: class, new()
        {
            return command.Received(expectedCalls)
                          .With(Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                              value);
        }
    }
}