using System;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Abstractions;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMappingProfileForValueTypeMemberShould: EntityMappingProfileTestBase
    {
        [Test]
        public void InvokeDelegateToConfigureBuilder()
        {
            IEntityValueMemberMapperBuilderConfig config = null;

            this.target.ForMember(t => t.Id, c => config = c);

            config.Should()
                   .NotBeNull("Builder config was not set as expected");
            config.Should()
                   .BeOfType<EntityValueMemberMapperBuilder>("Builder config was not of expected type");
        }

        [Test]
        public void ReturnSelfForFluentAccess()
        {
            var actual = this.target.ForMember(e => e.Id, o => { });

            actual.Should()
                  .Be(this.target);
        }
    }
}