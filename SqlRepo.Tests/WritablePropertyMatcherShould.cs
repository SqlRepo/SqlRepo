using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class WritablePropertyMatcherShould
    {
        [SetUp]
        public void SetUp()
        {
            this.matcher = new WritablePropertyMatcher();
        }

        [Test]
        [TestCase(typeof(string), true)]
        [TestCase(typeof(Enum), true)]
        [TestCase(typeof(string), true)]
        [TestCase(typeof(char), true)]
        [TestCase(typeof(Guid), true)]
        [TestCase(typeof(bool), true)]
        [TestCase(typeof(byte), true)]
        [TestCase(typeof(short), true)]
        [TestCase(typeof(int), true)]
        [TestCase(typeof(long), true)]
        [TestCase(typeof(float), true)]
        [TestCase(typeof(double), true)]
        [TestCase(typeof(decimal), true)]
        [TestCase(typeof(sbyte), true)]
        [TestCase(typeof(ushort), true)]
        [TestCase(typeof(uint), true)]
        [TestCase(typeof(ulong), true)]
        [TestCase(typeof(DateTime), true)]
        [TestCase(typeof(DateTimeOffset), true)]
        [TestCase(typeof(TimeSpan), true)]
        [TestCase(typeof(TestEntity), false)]
        [TestCase(typeof(ICollection<TestEntity>), false)]
        public void ReturnCorrectResultForType(Type type, bool expectedResult)
        {
            this.matcher.Test(type)
                .Should()
                .Be(expectedResult);
        }

        private WritablePropertyMatcher matcher;
    }
}