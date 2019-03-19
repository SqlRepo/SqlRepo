using System;
using System.Collections.Generic;
using System.Linq;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public static class PropertySetterProvider
    {
        private static readonly IPropertySetter defaultPropertySetter = new DefaultPropertySetter();
        private static readonly IEnumerable<IPropertySetter> propertySetters =
            new IPropertySetter[]
            {
                new BooleanPropertySetter(),
                new BytePropertySetter(),
                new DateTimePropertySetter(),
                new DecimalPropertySetter(),
                new DoublePropertySetter(),
                new FloatPropertySetter(),
                new IntPropertySetter(),
                new LongPropertySetter(),
                new ShortPropertySetter(),
                new StringPropertySetter()
            };

        public static IPropertySetter Get(Type type)
        {
            return propertySetters.FirstOrDefault(ps => ps.CanSet(type)) ?? defaultPropertySetter;
        }
    }
}