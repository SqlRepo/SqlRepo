using System;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMappingProfileForReferenceTypeMemberShould : EntityMappingProfileTestBase
    {
        [Test]
        public void ReturnSelfForFluentAccessOnForMemberWithProfile()
        {
            var mappingProfile = new EntityMappingProfile<Person>();

            var actual = this.target.ForMember(e => e.Location, mappingProfile);

            actual.Should()
                  .Be(this.target);
        }

        [Test]
        public void ReturnSelfForFluentAccessOnForMemberWithProfileBuilderAction()
        {
            var actual =
                this.target.ForMember<Location>(e => e.Location, (IEntityMappingProfile<Location> p) => { });

            actual.Should()
                  .Be(this.target);
        }
    }
}