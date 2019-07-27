using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public abstract class EntityMappingProfileTestBase
    {
        [SetUp]
        public void SetUp()
        {
            this.target = new EntityMappingProfile<Person>();
        }

        [Test]
        public void ReturnCorrectTargetType()
        {
            this.target.TargetType.Should()
                .Be(typeof(Person));
        }

        [Test]
        public void ThrowExceptionIfExpressionIsNotMemberExpression()
        {
            this.target.Invoking(t => t.ForMember(e => e.Children.LongCount(), o => { }))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("Expected valid member expression e.g e => e.Id");
        }

        protected IEntityMappingProfile<Person> target;
    }
}