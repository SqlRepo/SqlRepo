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

        public static void SetValue(this MemberInfo member, object target, object value)
        {
            switch(member.MemberType)
            {
                case MemberTypes.Field:
                    ((FieldInfo)member).SetValue(target, value);
                    break;
                case MemberTypes.Property:
                    ((PropertyInfo)member).SetValue(target, value, null);
                    break;
                default:
                    throw new ArgumentException("MemberInfo must be of type FieldInfo or PropertyInfo",
                        nameof(member));
            }
        }

        public static object GetValue(this MemberInfo member, object target)
        {
            switch(member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)member).GetValue(target);
                case MemberTypes.Property:
                    return ((PropertyInfo)member).GetValue(target, null);
                default:
                    throw new ArgumentException("MemberInfo must be of type FieldInfo or PropertyInfo",
                        nameof(member));
            }
        }

        public static object CreateInstance(this Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}