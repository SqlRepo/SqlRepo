using System;
using FluentAssertions;
using NUnit.Framework;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class PropertySetterProviderShould
    {
        [TestCase(typeof(bool), typeof(BooleanPropertySetter))]
        [TestCase(typeof(bool?), typeof(BooleanPropertySetter))]
        [TestCase(typeof(byte), typeof(BytePropertySetter))]
        [TestCase(typeof(byte?), typeof(BytePropertySetter))]
        [TestCase(typeof(DateTime), typeof(DateTimePropertySetter))]
        [TestCase(typeof(DateTime?), typeof(DateTimePropertySetter))]
        [TestCase(typeof(decimal), typeof(DecimalPropertySetter))]
        [TestCase(typeof(decimal?), typeof(DecimalPropertySetter))]
        [TestCase(typeof(double), typeof(DoublePropertySetter))]
        [TestCase(typeof(double?), typeof(DoublePropertySetter))]
        [TestCase(typeof(float), typeof(FloatPropertySetter))]
        [TestCase(typeof(float?), typeof(FloatPropertySetter))]
        [TestCase(typeof(int), typeof(IntPropertySetter))]
        [TestCase(typeof(int?), typeof(IntPropertySetter))]
        [TestCase(typeof(long), typeof(LongPropertySetter))]
        [TestCase(typeof(long?), typeof(LongPropertySetter))]
        [TestCase(typeof(short), typeof(ShortPropertySetter))]
        [TestCase(typeof(short?), typeof(ShortPropertySetter))]
        [TestCase(typeof(string), typeof(StringPropertySetter))]
        [TestCase(typeof(DateTimeOffset), typeof(DefaultPropertySetter))]
        [TestCase(typeof(DateTimeOffset?), typeof(DefaultPropertySetter))]
        public void ProvideCorrectSetterForType(Type propertyType, Type expectedSetterType)
        {
            var actual = PropertySetterProvider.Get(propertyType);

            actual.GetType()
                  .Should()
                  .Be(expectedSetterType);
        }
    }
}