using System;
using System.Collections.Generic;
using System.Linq;
using SqlRepo.Abstractions;

namespace SqlRepo.ValueMemberSetters
{
    public static class ValueMemberSetterProvider
    {
        private static readonly IValueMemberSetter DefaultValueMemberSetter = new DefaultValueMemberSetter();
        private static readonly IEnumerable<IValueMemberSetter> propertySetters =
            new IValueMemberSetter[]
            {
                new BooleanValueMemberSetter(),
                new ByteValueMemberSetter(),
                new CharValueMemberSetter(),
                new DateTimeValueMemberSetter(),
                new DecimalValueMemberSetter(),
                new DoubleValueMemberSetter(),
                new FloatValueMemberSetter(),
                new GuidValueMemberSetter(),
                new IntValueMemberSetter(),
                new LongValueMemberSetter(),
                new ShortValueMemberSetter(),
                new StringValueMemberSetter()
            };

        public static IValueMemberSetter Get(Type type)
        {
            return propertySetters.FirstOrDefault(ps => ps.CanSet(type)) ?? DefaultValueMemberSetter;
        }
    }
}