using System;
using NUnit.Framework;

namespace SqlRepo.Testing
{
    [TestFixture]
    public abstract class TestBase
    {
        protected const string OtherValue = "other";

        protected void AssumeTestEntityIsInitialised()
        {
            this.Entity = new TestEntity
                          {
                              ByteProperty = 1,
                              StringProperty = "My String",
                              IntProperty = 1,
                              DoubleProperty = 2.01,
                              DateTimeProperty = DateTime.UtcNow,
                              DateTimeOffsetProperty = DateTimeOffset.UtcNow,
                              DecimalProperty = 3.02M,
                              NullableDateTimeProperty = DateTime.UtcNow,
                              NullableDateTimeOffsetProperty = DateTimeOffset.UtcNow,
                              GuidProperty = Guid.NewGuid(),
                              ShortProperty = 2,
                              SingleProperty = 1.01F,
                              TestEnumProperty = TestEnum.One
                          };
        }

        protected TestEntity Entity { get; private set; }
    }
}