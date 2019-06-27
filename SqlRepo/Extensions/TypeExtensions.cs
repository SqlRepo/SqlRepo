using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SqlRepo
{
    public static class TypeExtensions
    {
        public static object GetDefaultForType(this Type type)
        {
            return type.IsValueType? Activator.CreateInstance(type): null;
        }

        public static IEnumerable<MemberInfo> GetPropertyAndFieldMembers(this Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            return type.GetFields(bindingFlags)
                       .Cast<MemberInfo>()
                       .Concat(type.GetProperties(bindingFlags));
        }

        public static bool IsMappableType(this Type type)
        {
            return type.IsSimpleType() || type.IsEnum || type.IsNullable()
                   || new[] {typeof(DateTime), typeof(DateTimeOffset), typeof(Guid)}.Contains(type);
        }

        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static bool IsSimpleType(this Type type)
        {
            return type.IsPrimitive || new[] {typeof(string), typeof(decimal)}.Contains(type)
                                    || Convert.GetTypeCode(type) != TypeCode.Object;
        }
    }
}