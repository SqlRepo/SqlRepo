using System;
using System.Linq;

namespace SqlRepo.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsSimpleType(
            this Type type)
        {
            return
                type.IsPrimitive ||
                new[]
                {
                    typeof(string),
                    typeof(decimal)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object;
        }

        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static object GetDefaultForType(this Type type)
        {
            return type.IsValueType? Activator.CreateInstance(type): null;
        }
    }
}