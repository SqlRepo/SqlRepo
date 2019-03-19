using System;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMappingProfileProviderShould
    {
        [SetUp]
        public void SetUp()
        {
            this.target = new EntityMappingProfileProvider();
        }

        [Test]
        public void GetAddedMappingProfile()
        {
            var profile = new EntityMappingProfile<Person>();

            this.target.Add(profile);

            this.target.Get<Person>().Should().Be(profile);
        }

        [Test]
        public void ReturnNullIfNoProfileForType()
        {
            this.target.Get<Person>().Should().BeNull();
        }

        [Test]
        public void ReturnProfileForType()
        {
            var profile = new EntityMappingProfile<Person>();

            this.target.Add(profile);

            this.target.Get(typeof(Person))
                .Should()
                .Be(profile);
        }

        [Test]
        public void SupportAddingProfilesFromAssembly()
        {
            this.target.AddFromAssembly(this.GetType()
                                            .Assembly);

            var personProfile = this.target.Get<Person>();
            personProfile
                .Should()
                .NotBeNull("Person profile was not added");

            personProfile.Should()
                         .BeAssignableTo<PersonEntityMappingProfile>(
                             "Person profile was not of expected type");

            var locationProfile = this.target.Get<Location>();
            locationProfile.Should()
                         .NotBeNull("Location profile was not added");

            locationProfile.Should()
                         .BeAssignableTo<LocationEntityMappingProfile>(
                             "Location profile was not of expected type");

        }

        private IEntityMappingProfileProvider target;
    }
}