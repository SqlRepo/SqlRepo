using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SqlRepo.SqlServer;

namespace SqlRepo.Testing
{
    public class ExpressionHelper
    {
   
        public Expression<Func<T, object>> ConvertExpression<T, TMember>(Expression<Func<T, TMember>> selector)
        {
            Expression converted = Expression.Convert(selector.Body, typeof(object));
            return Expression.Lambda<Func<T, object>>(converted, selector.Parameters);
        }

        public object GetExpressionValue(Expression expression)
        {
            var constantExpression = expression as ConstantExpression;
            if(constantExpression != null)
            {
                return constantExpression.Value;
            }
            
            var unaryExpression = expression as UnaryExpression;
            if(unaryExpression != null)
            {
                return this.GetExpressionValue(unaryExpression.Operand);
            }

            var lambdaExpression = expression as LambdaExpression;
            if(lambdaExpression != null)
            {
                var binaryExpression = lambdaExpression.Body as BinaryExpression;
                if(binaryExpression != null)
                {
                    return this.GetExpressionValue(binaryExpression.Right);
                }

                var callExpression = lambdaExpression.Body as MethodCallExpression;
                if(callExpression != null)
                {
                    return this.GetExpressionValue(callExpression);
                }

                var bodyExpression = lambdaExpression.Body as MemberExpression;
                if(bodyExpression != null)
                {
                    return this.ResolveValue((dynamic)bodyExpression.Member, null);
                }

                var bodyUnaryExpression = lambdaExpression.Body as UnaryExpression;
                if(bodyUnaryExpression != null)
                {
                    
                }
            }

            var memberExpression = expression as MemberExpression;
            if(memberExpression != null)
            {
                var @value = this.GetExpressionValue(memberExpression.Expression);
                return this.ResolveValue((dynamic)memberExpression.Member, @value);
            }

            throw new ArgumentException("Expected constant expression");
        }

        public string GetExpressionValue(MethodCallExpression callExpression)
        {
            const string Template = "{0}{1}{2}";
            var value = (string)this.GetExpressionValue(callExpression.Arguments.First());
            switch(callExpression.Method.Name)
            {
                case MethodName.EndsWith:
                    return string.Format(Template, "%", value, string.Empty);
                case MethodName.StartsWith:
                    return string.Format(Template, string.Empty, value, "%");
                default:
                    return string.Format(Template, "%", value, "%");
            }
        }

        public MemberExpression GetMemberExpression(Expression expression)
        {
            var memberExpression = expression as MemberExpression;
            if(memberExpression != null)
            {
                return memberExpression;
            }

            var binaryExpression = expression as BinaryExpression;
            if(binaryExpression != null)
            {
                return this.GetMemberExpression(binaryExpression.Left);
            }

            var unaryExpression = expression as UnaryExpression;
            if(unaryExpression != null)
            {
                return this.GetMemberExpression(unaryExpression.Operand);
            }

            var callExpression = expression as MethodCallExpression;
            if(callExpression != null)
            {
                return this.GetMemberExpression(callExpression.Object);
            }

            throw new ArgumentException("Member expression expected");
        }

        public string GetMemberName<T, TMember>(Expression<Func<T, TMember>> selector)
        {
            return this.GetMemberName(this.GetMemberExpression(selector.Body));
        }

        public string GetMemberName(Expression expression)
        {
            var member = this.GetMemberExpression(expression);
            return member.Member.Name;
        }

        public string GetOperator<T, TMember>(Expression<Func<T, TMember>> expression)
        {
            var lambdaExpression = expression as LambdaExpression;

            if(lambdaExpression != null)
            {
                var binaryExpression = lambdaExpression.Body as BinaryExpression;
                if(binaryExpression != null)
                {
                    return this.OperatorString(binaryExpression.NodeType);
                }

                var callExpression = lambdaExpression.Body as MethodCallExpression;
                if(callExpression != null)
                {
                    return this.OperatorString(callExpression.Method.Name);
                }
            }

            return "=";
        }

        public string OperatorString(ExpressionType @operator)
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

        private string OperatorString(string methodName)
        {
            switch(methodName)
            {
                default:
                    return "LIKE";
            }
        }

        private object ResolveMethodCall(MethodCallExpression callExpression)
        {
            var arguments = callExpression.Arguments.Select(this.GetExpressionValue)
                                          .ToArray();
            var obj = callExpression.Object != null? this.GetExpressionValue(callExpression.Object): arguments.First();

            return callExpression.Method.Invoke(obj, arguments);
        }

        private object ResolveValue(PropertyInfo property, object obj)
        {
            return property.GetValue(obj, null);
        }

        private object ResolveValue(FieldInfo field, object obj)
        {
            return field.GetValue(obj);
        }
    }
}