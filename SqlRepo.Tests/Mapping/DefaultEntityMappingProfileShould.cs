using System;
using System.Data;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class DefaultEntityMappingProfileShould
    {
        [SetUp]
        public void SetUp()
        {
            this.AssumeDataRecordIsInitialised();
            this.target = new DefaultEntityMappingProfile(typeof(TestEntity));
        }

        [Test]
        public void MapUsingColumnNameStrategy()
        {
            this.target.Map(this.entity, this.dataRecord);
            this.entity.DateTimeOffsetProperty.Should()
                .Be(this.dateTimeOffsetExpectedValue, "DateTimeOffsetProperty was not set as expected");
            this.entity.DateTimeProperty.Should()
                .Be(this.dateTimeExpectedValue, "DateTimeProperty was nto set as expected");
            this.entity.DoubleProperty.Should()
                .Be(this.doubleExpectedValue, "DoubleProperty was not set as expected");
            this.entity.IntProperty.Should()
                .Be(this.intExpectedValue, "IntProperty was not set as expected");
            this.entity.StringProperty.Should()
                .Be(this.stringExpectedValue, "StringProperty was not set as expected");
            this.entity.TestEnumProperty.Should()
                .Be(this.testEnumExpectedValue, "TestEnumProperty was not set as expected");
            this.entity.GuidProperty.Should()
                .Be(this.guidExpectedValue, "GuidProperty was not set as expected");

        }

        [Test]
        public void AlwaysIndicateDataRecordAndEntityDoNotMatch
            ()
        {
            var result = this.target.DataRecordMatchesEntity(this.entity, this.dataRecord);
            result.Should().BeFalse();
        }

        private IDataRecord dataRecord;
        private TestEntity entity;
        private IEntityMappingProfile target;
        private DateTimeOffset dateTimeOffsetExpectedValue;
        private DateTime dateTimeExpectedValue;
        private int doubleExpectedValue;
        private int intExpectedValue;
        private string stringExpectedValue;
        private TestEnum testEnumExpectedValue;
        private Guid guidExpectedValue;

        private void AssumeDataRecordIsInitialised()
        {
            this.entity = new TestEntity();
            this.dateTimeOffsetExpectedValue = DateTimeOffset.Now;
            this.dateTimeExpectedValue = DateTime.Now;
            this.doubleExpectedValue = 100;
            this.intExpectedValue = 5;
            this.stringExpectedValue = "something";
            this.testEnumExpectedValue = TestEnum.One;
            this.guidExpectedValue = Guid.NewGuid();
            this.dataRecord = DataRecordMockBuilder.CreateNew()
                                                   .WithDateTimeOffsetColumn(
                                                       nameof(this.entity.DateTimeOffsetProperty),
                                                       0,
                                                       this.dateTimeOffsetExpectedValue)
                                                   .WithDateTimeColumn(nameof(this.entity.DateTimeProperty),
                                                       1,
                                                       this.dateTimeExpectedValue)
                                                   .WithDoubleColumn(nameof(this.entity.DoubleProperty),
                                                       2,
                                                       this.doubleExpectedValue)
                                                   .WithIntColumn(nameof(this.entity.IntProperty), 3, this.intExpectedValue)
                                                   .WithStringColumn(nameof(this.entity.StringProperty),
                                                       4,
                                                       this.stringExpectedValue)
                                                   .WithIntColumn(nameof(this.entity.TestEnumProperty),
                                                       5,
                                                       (int)this.testEnumExpectedValue)
                                                   .WithGuidColumn(nameof(this.entity.GuidProperty),
                                                       6,
                                                       this.guidExpectedValue)
                                                   .Build();
        }
    }
}