using System;
using System.Linq.Expressions;

namespace SqlRepo.Testing
{
    public static class ExpressionAssertionExtensions
    {
        private static readonly ExpressionHelper helper = new ExpressionHelper();

        public static bool HasMemberName<TEntity, TMember>(this Expression<Func<TEntity, TMember>> expression,
            string expected)
        {
            return helper.GetMemberName(expression) == expected;
        }

        public static bool HasValue<TEntity, TMember>(this Expression<Func<TEntity, TMember>> expression,
            object expected)
        {
            return helper.GetExpressionValue(expression) == expected;
        }
        
        public static bool IsComparisonWith<TEntity, TMember>(this Expression<Func<TEntity, TMember>> expression,
            string member, string @operator, string @value)
        {
            var memberName = helper.GetMemberName(expression);
            var actualOperator = helper.GetOperator(expression);
            var expressionValue = helper.GetExpressionValue(expression);
            return memberName == member && actualOperator == @operator && expressionValue.ToString() == @value;
        }
    }
}