using System;
using System.Linq;

namespace SqlRepo
{
    public class WritablePropertyMatcher : IWritablePropertyMatcher
    {
        private readonly Type[] additionalTypes;

        public WritablePropertyMatcher()
        {
            var types = new[]
                        {
                            typeof(Enum),
                            typeof(string),
                            typeof(bool),
                            typeof(byte),
                            typeof(short),
                            typeof(int),
                            typeof(long),
                            typeof(float),
                            typeof(double),
                            typeof(decimal),
                            typeof(sbyte),
                            typeof(ushort),
                            typeof(uint),
                            typeof(ulong),
                            typeof(DateTime),
                            typeof(DateTimeOffset),
                            typeof(TimeSpan),
                        };

            var nullTypes = from t in types where t.IsValueType select typeof(Nullable<>).MakeGenericType(t);

            this.additionalTypes = types.Concat(nullTypes)
                             .ToArray();
        }

        public bool Test(Type type)
        {
            if(type.IsValueType || this.additionalTypes.Any(x => x.IsAssignableFrom(type)))
            {
                return true;
            }

            var nut = Nullable.GetUnderlyingType(type);
            return nut != null && nut.IsEnum;
        }
    }
}