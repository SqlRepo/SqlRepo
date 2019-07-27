using System;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.ValueMemberSetters;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class ValueMemberSettersProviderShould
    {
        [TestCase(typeof(bool), typeof(BooleanValueMemberSetter))]
        [TestCase(typeof(bool?), typeof(BooleanValueMemberSetter))]
        [TestCase(typeof(byte), typeof(ByteValueMemberSetter))]
        [TestCase(typeof(byte?), typeof(ByteValueMemberSetter))]
        [TestCase(typeof(DateTime), typeof(DateTimeValueMemberSetter))]
        [TestCase(typeof(DateTime?), typeof(DateTimeValueMemberSetter))]
        [TestCase(typeof(decimal), typeof(DecimalValueMemberSetter))]
        [TestCase(typeof(decimal?), typeof(DecimalValueMemberSetter))]
        [TestCase(typeof(double), typeof(DoubleValueMemberSetter))]
        [TestCase(typeof(double?), typeof(DoubleValueMemberSetter))]
        [TestCase(typeof(float), typeof(FloatValueMemberSetter))]
        [TestCase(typeof(float?), typeof(FloatValueMemberSetter))]
        [TestCase(typeof(int), typeof(IntValueMemberSetter))]
        [TestCase(typeof(int?), typeof(IntValueMemberSetter))]
        [TestCase(typeof(long), typeof(LongValueMemberSetter))]
        [TestCase(typeof(long?), typeof(LongValueMemberSetter))]
        [TestCase(typeof(short), typeof(ShortValueMemberSetter))]
        [TestCase(typeof(short?), typeof(ShortValueMemberSetter))]
        [TestCase(typeof(string), typeof(StringValueMemberSetter))]
        [TestCase(typeof(DateTimeOffset), typeof(DefaultValueMemberSetter))]
        [TestCase(typeof(DateTimeOffset?), typeof(DefaultValueMemberSetter))]
        public void ProvideCorrectSetterForType(Type propertyType, Type expectedSetterType)
        {
            var actual = ValueMemberSetterProvider.Get(propertyType);

            actual.GetType()
                  .Should()
                  .Be(expectedSetterType);
        }
    }
}