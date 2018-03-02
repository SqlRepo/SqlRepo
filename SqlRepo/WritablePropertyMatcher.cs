using System;
using System.Linq;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class WritablePropertyMatcher : IWritablePropertyMatcher
    {
        private readonly Type[] additionalTypes;

        public WritablePropertyMatcher()
        {
            var types = new[]
            {
                typeof (Enum),
                typeof (string),
                typeof (bool),
                typeof (byte),
                typeof (short),
                typeof (int),
                typeof (long),
                typeof (float),
                typeof (double),
                typeof (decimal),
                typeof (sbyte),
                typeof (ushort),
                typeof (uint),
                typeof (ulong),
                typeof (DateTime),
                typeof (DateTimeOffset),
                typeof (TimeSpan)
            };

            var nullTypes = from t in types
                where t.GetTypeInfo().IsValueType
                select typeof (Nullable<>).MakeGenericType(t);

            additionalTypes = types.Concat(nullTypes)
                .ToArray();
        }

        public bool Test(Type type)
        {
            if (type.GetTypeInfo().IsValueType || additionalTypes.Any(x => x.IsAssignableFrom(type)))
            {
                return true;
            }

            var nut = Nullable.GetUnderlyingType(type);
            return nut != null && nut.GetTypeInfo().IsEnum;
        }
    }
}