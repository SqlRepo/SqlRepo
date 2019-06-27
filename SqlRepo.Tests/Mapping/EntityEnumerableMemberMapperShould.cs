using System;
using NUnit.Framework;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityEnumerableMemberMapperShould
    {
        private MappingTestEntity entity;

        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }
    }
}
