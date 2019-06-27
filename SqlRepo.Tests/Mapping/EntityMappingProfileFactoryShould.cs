using System;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMappingProfileFactoryShould
    {
        [SetUp]
        public void SetUp()
        {
            this.target = new EntityMappingProfileFactory();
        }

        [Test]
        public void CreateInstance()
        {
            var result = this.target.Create<Person>();

            result.Should()
                  .BeAssignableTo<IEntityMappingProfile<Person>>();
            result.Should()
                  .BeOfType<EntityMappingProfile<Person>>();
        }

        private IEntityMappingProfileFactory target;
    }
}