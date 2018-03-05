using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo.SqlServer
{
    public abstract class ClauseBuilder : IClauseBuilder
    {
        public const string DefaultSchema = "dbo";

        protected ClauseBuilder()
        {
            this.IsClean = true;
        }

        public bool IsClean { get; set; }
        public abstract string Sql();

        protected Expression<Func<T, object>> ConvertExpression<T, TMember>(
            Expression<Func<T, TMember>> selector)
        {
            Expression converted = Expression.Convert(selector.Body, typeof(object));
            return Expression.Lambda<Func<T, object>>(converted, selector.Parameters);
        }

        protected string FormatValue(object @value)
        {
            if(@value is string || @value is Guid)
            {
                var newString = this.ReplaceSingleQuoteWithDoubleQuote($"{@value}");
                return $"'{newString}'";
            }

            if(@value is DateTime)
            {
                var dateTime = (DateTime)@value;
                return $"'{dateTime.ToString(FormatString.DateTime)}'";
            }

            if(@value is DateTimeOffset)
            {
                var dateTime = (DateTimeOffset)@value;
                return $"'{dateTime.ToString(FormatString.DateTimeOffset)}'";
            }

            if(@value is Enum)
            {
                return Convert.ToInt32(@value)
                              .ToString();
            }

            if(!(@value is bool))
            {
                return @value?.ToString() ?? "NULL";
            }
            var boolValue = (bool)@value;
            return boolValue? "1": "0";
        }

        protected object GetExpressionValue(Expression expression)
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
            }

            var memberExpression = expression as MemberExpression;
            if(memberExpression != null)
            {
                var fieldsOfObj = memberExpression.Expression as ConstantExpression;
                var propertyInfo = memberExpression.Member as PropertyInfo;

                if (propertyInfo != null && !propertyInfo.PropertyType.IsSimpleType())
                    return propertyInfo.GetValue(fieldsOfObj, null);

                var @value = this.GetExpressionValue(memberExpression.Expression);
                return this.ResolveValue((dynamic)memberExpression.Member, @value);
            }

            throw new ArgumentException("Expected constant expression");
        }

        protected string GetExpressionValue(MethodCallExpression callExpression)
        {
            const string template = "{0}{1}{2}";
            var value = (string)this.GetExpressionValue(callExpression.Arguments.First());
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

        protected MemberExpression GetMemberExpression(Expression expression)
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

        protected string GetMemberName<T, TMember>(Expression<Func<T, TMember>> selector)
        {
            return this.GetMemberName(this.GetMemberExpression(selector.Body));
        }

        protected string GetMemberName(Expression expression)
        {
            var member = this.GetMemberExpression(expression);
            return member.Member.Name;
        }

        protected string GetOperator<T, TMember>(Expression<Func<T, TMember>> expression)
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

        protected string OperatorString(ExpressionType @operator)
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

        protected string TableNameFromType<TEntity>()
        {
            return typeof(TEntity).Name;
        }

        private string OperatorString(string methodName)
        {
            switch(methodName)
            {
                default:
                    return "LIKE";
            }
        }

        private string ReplaceSingleQuoteWithDoubleQuote(string originalString)
        {
            var newString = originalString;

            for(var i = 0; i < newString.Length; i++)
            {
                if(newString[i] != '\'')
                {
                    continue;
                }

                if(i == 0 || (newString[i - 1] != '\''))
                {
                    if(i == newString.Length - 1 || newString[i + 1] != '\'')
                    {
                        newString = newString.Insert(i, "'");
                    }
                }
            }
            return newString;
        }

        private object ResolveMethodCall(MethodCallExpression callExpression)
        {
            var arguments = callExpression.Arguments.Select(this.GetExpressionValue)
                                          .ToArray();
            var obj = callExpression.Object != null
                          ? this.GetExpressionValue(callExpression.Object)
                          : arguments.First();

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