using System;
using System.Linq.Expressions;
using FluentAssertions;
using NUnit.Framework;

namespace SqlRepo.Testing.Tests
{
    [TestFixture]
    public class ExpressionAssertionExtensionsShould
    {
        [Test]
        public void MatchEqualBooleanExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 3;
            expression.AreEqual(e => e.Id == 3)
                      .Should()
                      .BeTrue();
        }

        [Test]
        public void NotMatchUnequalBooleanExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 3;
            expression.AreEqual(e => e.Id == 4)
                      .Should()
                      .BeFalse();
        }
    }
}