using System;
using System.Collections;
using System.Reflection;

namespace SqlRepo
{
    public static class ReflectionExtensions
    {
        public static Type GetUnderlyingType(this MemberInfo member)
        {
            switch(member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException(
                        "Input MemberInfo must be of type EventInfo, FieldInfo, MethodInfo, or PropertyInfo");
            }
        }

        public static bool IsEnumerable(this MemberInfo member)
        {
            return typeof(IEnumerable).IsAssignableFrom(member.GetUnderlyingType());
        }
    }
}