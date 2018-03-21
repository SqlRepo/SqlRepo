using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NSubstitute;
using SqlRepo.Abstractions;

namespace SqlRepo.Testing.NSubstitute
{
    public static class SelectStatementSubstituteSelectClauseExtensions
    {
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
        
        public static ISelectStatement<TEntity> ReceivedSelect<TEntity>(
            this ISelectStatement<TEntity> selectStatement,
            Expression<Func<TEntity, object>> expression,
            params Expression<Func<TEntity, object>>[] additionalExpressions)
            where TEntity: class, new()
        {
            if(additionalExpressions == null)
            {
                return selectStatement.Received()
                                      .Select(Arg.Is<Expression<Func<TEntity, object>>>(
                                          e => e.IsEqual(expression)));
            }

            return selectStatement.Received()
                                  .Select(Arg.Is<Expression<Func<TEntity, object>>>(
                                      e => e.IsEqual(expression)),
                                      Arg.Is<Expression<Func<TEntity, object>>[]>(e => e.AreAllEqual(additionalExpressions)));
        }
    }
}