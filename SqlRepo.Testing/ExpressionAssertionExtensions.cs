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

        public static bool AreEqual<TEntity>(this Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, bool>> compareTo)
        {
            if(expression.Equals(compareTo) || ReferenceEquals(expression, compareTo))
            {
                return true;
            }

            if(compareTo == null)
            {
                return false;
            }

            var memberNameMatches = helper.GetMemberName(expression) == helper.GetMemberName(compareTo);
            var operatorMatches = helper.GetOperator(expression) == helper.GetOperator(compareTo);
            var valueMatches = helper.GetExpressionValue(expression).Equals(helper.GetExpressionValue(compareTo));

            return memberNameMatches && operatorMatches && valueMatches;
        }
    }
}