using System;
using System.Linq.Expressions;
using NSubstitute;

namespace SqlRepo.Testing.NSubstitute
{
    public static class InsertStatementSubstituteExtensions
    {
        public static IInsertStatement<TEntity> ReceivedWith<TEntity, TMember>(
            this IInsertStatement<TEntity> command,
            string property,
            TMember value) where TEntity: class, new()
        {
            return command.Received()
                          .With(Arg.Is<Expression<Func<TEntity, TMember>>>(e => e.HasMemberName(property)),
                              value);
        }

        public static IInsertStatement<TEntity> ReceivedWith<TEntity, TMember>(
            this IInsertStatement<TEntity> command,
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