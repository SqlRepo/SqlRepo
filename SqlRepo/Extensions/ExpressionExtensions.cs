using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SqlRepo
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, object>> ConvertExpression<T, TMember>(this Expression<Func<T, TMember>> selector)
        {
            Expression converted = Expression.Convert(selector.Body, typeof(object));
            return Expression.Lambda<Func<T, object>>(converted, selector.Parameters);
        }

        public static object GetExpressionValue(this Expression expression)
        {
            if(expression is ConstantExpression constantExpression)
            {
                return constantExpression.Value;
            }

            if(expression is UnaryExpression unaryExpression)
            {
                return unaryExpression.Operand.GetExpressionValue();
            }

            if(expression is LambdaExpression lambdaExpression)
            {
                if(lambdaExpression.Body is BinaryExpression binaryExpression)
                {
                    return binaryExpression.Right.GetExpressionValue();
                }

                if(lambdaExpression.Body is MethodCallExpression callExpression)
                {
                    return callExpression.GetExpressionValue();
                }
            }

            if(expression is MemberExpression memberExpression)
            {
                var @value = memberExpression.Expression.GetExpressionValue();
                return memberExpression.Member.ResolveValue(@value);
            }

            throw new ArgumentException("Expected constant expression");
        }

        public static string GetExpressionValue(this MethodCallExpression callExpression)
        {
            const string template = "{0}{1}{2}";
            var value = (string)callExpression.Arguments.First().GetExpressionValue();
            switch(callExpression.Method.Name)
            {
                case MethodName.EndsWith:
                    return string.Format(template, "%", value, string.Empty);
                case MethodName.StartsWith:
                    return string.Format(template, string.Empty, value, "%");
                default:
                    return string.Format(template, "%", value, "%");
            }
        }

        public static MemberExpression GetMemberExpression(this Expression expression)
        {
            if(expression is MemberExpression memberExpression)
            {
                return memberExpression;
            }

            if(expression is LambdaExpression lambdaExpression)
            {
                return lambdaExpression.Body.GetMemberExpression();
            }

            if(expression is BinaryExpression binaryExpression)
            {
                return binaryExpression.Left.GetMemberExpression();
            }

            if(expression is UnaryExpression unaryExpression)
            {
                return unaryExpression.Operand.GetMemberExpression();
            }

            if(expression is MethodCallExpression callExpression)
            {
                return callExpression.Object.GetMemberExpression();
            }

            throw new ArgumentException("Expected valid member expression e.g e => e.Id");
        }

        public static string GetMemberName<T, TMember>(this Expression<Func<T, TMember>> selector)
        {
            return selector.Body.GetMemberName();
        }

        public static string GetMemberName(this Expression expression)
        {
            var member = expression.GetMemberExpression();
            return member.Member.Name;
        }

        public static string GetOperator<T, TMember>(this Expression<Func<T, TMember>> expression)
        {
            if(expression is LambdaExpression lambdaExpression)
            {
                if(lambdaExpression.Body is BinaryExpression binaryExpression)
                {
                    return binaryExpression.NodeType.ToOperatorString();
                }

                if(lambdaExpression.Body is MethodCallExpression callExpression)
                {
                    return callExpression.ToOperatorString();
                }
            }

            return "=";
        }

        public static string ToOperatorString(this ExpressionType @operator)
        {
            switch(@operator)
            {
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.Not:
                case ExpressionType.NotEqual:
                    return "<>";
                default:
                    return "=";
            }
        }

        public static IEnumerable<MemberExpression> GetMembers(this MemberExpression expression)
        {
            while(expression != null)
            {
                yield return expression;
                expression = expression.Expression as MemberExpression;
            }
        }

        public static IEnumerable<MemberInfo> GetMemberInfos(this MemberExpression expression)
        {
            return expression.GetMembers()
                             .Select(e => e.Member);
        }

        private static string ToOperatorString(this MethodCallExpression expression)
        {
            switch(expression.Method.Name)
            {
                default:
                    return "LIKE";
            }
        }

        private static object ResolveMethodCall(this MethodCallExpression callExpression)
        {
            var arguments = callExpression.Arguments.Select(a => a.GetExpressionValue())
                                          .ToArray();
            var obj = callExpression.Object != null? callExpression.Object.GetExpressionValue(): arguments.First();

            return callExpression.Method.Invoke(obj, arguments);
        }

        private static object ResolveValue(this MemberInfo member, object obj)
        {
            switch(member.MemberType)
            {
                case MemberTypes.Property:
                 var propertyInfo = member as PropertyInfo;
                 return propertyInfo.ResolveValue(obj);
                case MemberTypes.Field:
                    var fieldInfo = member as FieldInfo;
                    return fieldInfo.ResolveValue(obj);
                default:
                    throw new ArgumentException("The memberInfo specified did not reference a field or property");
            }
        }
        
        private static object ResolveValue(this PropertyInfo property, object obj)
        {
            return property.GetValue(obj, null);
        }

        private static object ResolveValue(this FieldInfo field, object obj)
        {
            return field.GetValue(obj);
        }
    }
}