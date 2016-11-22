using System;
using FluentAssertions;
using NUnit.Framework;

namespace SqlRepo.Model.Tests
{
    [TestFixture]
    public class EntityShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new EntityFake();
        }

        [Test]
        public void DefaultToBeingTransient()
        {
            this.entity.IsTransient()
                .Should()
                .BeTrue();
        }

        [Test]
        public void FailEqualityTestWhenBothInstancesAreTransient()
        {
            var entity2 = new EntityFake();
            this.entity.Equals(entity2)
                .Should()
                .BeFalse();
        }

        [Test]
        public void FailEqualityTestWhenBothInstancesAreTransientUsingOperator()
        {
            var entity2 = new EntityFake();
            var result = this.entity == entity2;
            result.Should()
                  .BeFalse();
        }

        [Test]
        public void FailEqualityTestWhenBothInstancesHaveDifferentId()
        {
            this.entity.Id = 1;
            var entity2 = new EntityFake
                          {
                              Id = 2
                          };
            this.entity.Equals(entity2)
                .Should()
                .BeFalse();
        }

        [Test]
        public void FailEqualityTestWhenBothInstancesHaveDifferentIdUsingOperation()
        {
            this.entity.Id = 1;
            var entity2 = new EntityFake
                          {
                              Id = 2
                          };
            var result = this.entity == entity2;
            result.Should()
                  .BeFalse();
        }

        [Test]
        public void FailEqualityTestWhenComparingAgainstNull()
        {
            this.entity.Id = 1;
            this.entity.Equals(null)
                .Should()
                .BeFalse();
        }

        [Test]
        public void FailEqualityTestWhenComparingAgainstNullUsingOperation()
        {
            this.entity.Id = 1;
            var result = this.entity == null;
            result.Should()
                  .BeFalse();
        }

        [Test]
        public void NotBeTransientOnceIdSet()
        {
            this.entity.Id = 1;
            this.entity.IsTransient()
                .Should()
                .BeFalse();
        }

        [Test]
        public void PassEqualityTestWhenBothInstancesHaveSameId()
        {
            this.entity.Id = 1;
            var entity2 = new EntityFake
                          {
                              Id = 1
                          };
            this.entity.Equals(entity2)
                .Should()
                .BeTrue();
        }

        [Test]
        public void PassEqualityTestWhenBothInstancesHaveSameIdUsingOperator()
        {
            this.entity.Id = 1;
            var entity2 = new EntityFake
                          {
                              Id = 1
                          };
            var result = this.entity == entity2;
            result.Should()
                  .BeTrue();
        }

        [Test]
        public void FailNotEqualTestWhenBothInstancesHaveSameIdUsingOperator()
        {
            this.entity.Id = 1;
            var entity2 = new EntityFake
                          {
                              Id = 1
                          };
            var result = this.entity != entity2;
            result.Should()
                  .BeFalse();
        }

        [Test]
        public void ReturnDifferentHashCodesForTransientObjects()
        {
            var entity2 = new EntityFake();
            this.entity.GetHashCode()
                .Should()
                .NotBe(entity2.GetHashCode());
        }

        [Test]
        public void ReturnDifferentHashCodesForTwoEntitiesWithDifferentIds()
        {
            this.entity.Id = 1;
            var entity2 = new EntityFake
                          {
                              Id = 2
                          };
            this.entity.GetHashCode()
                .Should()
                .NotBe(entity2.GetHashCode());
        }

        [Test]
        public void ReturnSameHashCodesForTwoEntitiesWithSameId()
        {
            this.entity.Id = 1;
            var entity2 = new EntityFake
                          {
                              Id = 1
                          };
            this.entity.GetHashCode()
                .Should()
                .Be(entity2.GetHashCode());
        }

        [Test]
        public void ReturnSameHashCodeOnRepeatedCallsForTransientObject()
        {
            this.entity.GetHashCode()
                .Should()
                .Be(this.entity.GetHashCode());
        }

        private EntityFake entity;
    }
}