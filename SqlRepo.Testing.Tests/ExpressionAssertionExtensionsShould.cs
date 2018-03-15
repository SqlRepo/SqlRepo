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
        public void MatchTheSameEqualityExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 3;
            expression.AreEqual(e => e.Id == 3)
                      .Should()
                      .BeTrue();
        }

        [Test]
        public void NotMatchDifferentEqualityExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id == 3;
            expression.AreEqual(e => e.Id == 4)
                      .Should()
                      .BeFalse();
        }

        [Test]
        public void MatchTheSameInequalityExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id != 3;
            expression.AreEqual(e => e.Id != 3)
                      .Should()
                      .BeTrue();
        }

        [Test]
        public void NotMatchDifferentInequalityExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id != 3;
            expression.AreEqual(e => e.Id != 4)
                      .Should()
                      .BeFalse();
        }

        [Test]
        public void MatchTheSameGreaterThanExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id > 3;
            expression.AreEqual(e => e.Id > 3)
                      .Should()
                      .BeTrue();
        }

        [Test]
        public void NotMatchDifferentGreaterThanExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id > 3;
            expression.AreEqual(e => e.Id > 4)
                      .Should()
                      .BeFalse();
        }

        [Test]
        public void MatchTheSameGreaterThanOrEqualExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id >= 3;
            expression.AreEqual(e => e.Id >= 3)
                      .Should()
                      .BeTrue();
        }

        [Test]
        public void NotMatchDifferentGreaterThanOrEqualExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id >= 3;
            expression.AreEqual(e => e.Id >= 4)
                      .Should()
                      .BeFalse();
        }

        [Test]
        public void MatchTheSameLessThanExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id < 3;
            expression.AreEqual(e => e.Id < 3)
                      .Should()
                      .BeTrue();
        }

        [Test]
        public void NotMatchDifferentLessThanExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id < 3;
            expression.AreEqual(e => e.Id < 4)
                      .Should()
                      .BeFalse();
        }

        [Test]
        public void MatchTheSameLessThanOrEqualExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id <= 3;
            expression.AreEqual(e => e.Id <= 3)
                      .Should()
                      .BeTrue();
        }

        [Test]
        public void NotMatchDifferentLessThanOrEqualExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.Id <= 3;
            expression.AreEqual(e => e.Id <= 4)
                      .Should()
                      .BeFalse();
        }

        [Test]
        public void MatchSameBooleanMemberExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.BooleanProperty;
            expression.AreEqual(e => e.BooleanProperty2)
                      .Should()
                      .BeFalse();
        }

        [Test]
        public void NotMatchDifferentBooleanMemberExpressions()
        {
            Expression<Func<TestEntity, bool>> expression = e => e.BooleanProperty;
            expression.AreEqual(e => e.BooleanProperty2)
                      .Should()
                      .BeFalse();
        }
    }
}