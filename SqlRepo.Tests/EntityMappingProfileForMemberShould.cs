using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMappingProfileForMemberShould
    {
        [SetUp]
        public void SetUp()
        {
            this.target = new EntityMappingProfile<Person>();
        }

        [Test]
        public void ReturnCorrectTargetType()
        {
            this.target.TargetType.Should().Be(typeof(Person));
        }

        [Test]
        public void InvokeDelegateToConfigureBuilder()
        {
            IEntityMemberMapperBuilderConfig config = null;

            this.target.ForMember(t => t.Id, c => config = c);

            config.Should()
                   .NotBeNull("Builder config was not set as expected");
            config.Should()
                   .BeOfType<EntityMemberMapperBuilder>("Builder config was not of expected type");
        }

        [Test]
        public void PopulateMappingsCollection()
        {
            var memberInfo = typeof(Person).GetMember("Id").First();

            this.target.ForMember(e => e.Id, o => { });

            var actual = this.target.GetMapper(memberInfo);

            actual.Should()
                  .NotBeNull("Actual mapper was not found");
            actual.Should()
                  .BeOfType<EntityMemberMapper>("Actual mapper was not of expected type");
        }

        [Test]
        public void ReturnSelfForFluentAccess()
        {
            var actual = this.target.ForMember(e => e.Id, o => { });

            actual.Should()
                  .Be(this.target);
        }

        [Test]
        public void ThrowExceptionIfExpressionIsNotMemberExpression()
        {
            this.target.Invoking(t => t.ForMember(e => e.Children.LongCount(), o => { }))
                .Should()
                .Throw<ArgumentException>()
                .WithMessage("Expected valid member expression e.g e => e.Id");
        }

        private IEntityMappingProfile<Person> target;
    }
}