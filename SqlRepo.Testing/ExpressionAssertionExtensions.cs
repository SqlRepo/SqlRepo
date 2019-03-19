using System;
using System.Linq.Expressions;

namespace SqlRepo.Testing
{
    public static class ExpressionAssertionExtensions
    {
        public static bool HasMemberName<TEntity, TMember>(this Expression<Func<TEntity, TMember>> expression,
            string expected)
        {
            return expression.GetMemberName() == expected;
        }

        public static bool HasOperator<TEntity, TMember>(this Expression<Func<TEntity, TMember>> expression,
            string @operator)
        {
            return expression.GetOperator() == @operator;
        }

        public static bool HasValue<TEntity, TMember>(this Expression<Func<TEntity, TMember>> expression,
            object expected)
        {
            return expression.GetExpressionValue() == expected;
        }

        public static bool IsComparisonWith<TEntity, TMember>(
            this Expression<Func<TEntity, TMember>> expression,
            string member,
            string @operator,
            string @value)
        {
            return expression.HasMemberName(member) && expression.HasOperator(@operator)
                                                    && expression.HasValue(@value);
        }
    }
}