using System;
using System.Data;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityValueMemberMapperDateTimeShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }

        [Test]
        public void MapDateTimePropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DateTimePropertyName);
            var builder = this.AssumeBuilderIsInitialised(DateTimePropertyName);
            builder.MapFromColumnName(DateTimePropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DateTimeProperty.Should()
                .Be(this.returnDateTime);
        }

        [Test]
        public void MapDateTimePropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DateTimePropertyName);
            var builder = this.AssumeBuilderIsInitialised(DateTimePropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DateTimeProperty.Should()
                .Be(this.returnDateTime);
        }

        [Test]
        public void LeaveDateTimePropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DateTimePropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(DateTimePropertyName);
            builder.MapFromColumnName(DateTimePropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DateTimeProperty.Should()
                .Be(default(DateTime));
        }

        [Test]
        public void MapNullableDateTimePropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDateTimePropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableDateTimePropertyName);
            builder.MapFromColumnName(NullableDateTimePropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDateTimeProperty.Should()
                .Be(this.returnDateTime);
        }

        [Test]
        public void MapNullableDateTimePropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDateTimePropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableDateTimePropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDateTimeProperty.Should()
                .Be(this.returnDateTime);
        }

        [Test]
        public void LeaveNullableDateTimePropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDateTimePropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableDateTimePropertyName);
            builder.MapFromColumnName(NullableDateTimePropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDateTimeProperty.Should()
                .Be(default(DateTime?));
        }

        [Test]
        public void MapDateTimeFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DateTimeFieldName);
            var builder = this.AssumeBuilderIsInitialised(DateTimeFieldName);
            builder.MapFromColumnName(DateTimeFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DateTimeField.Should()
                .Be(this.returnDateTime);
        }

        [Test]
        public void MapDateTimeFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DateTimeFieldName);
            var builder = this.AssumeBuilderIsInitialised(DateTimeFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DateTimeField.Should()
                .Be(this.returnDateTime);
        }

        [Test]
        public void LeaveDateTimeFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DateTimeFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(DateTimeFieldName);
            builder.MapFromColumnName(DateTimeFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DateTimeField.Should()
                .Be(default(DateTime));
        }

        [Test]
        public void MapNullableDateTimeFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDateTimeFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableDateTimeFieldName);
            builder.MapFromColumnName(NullableDateTimeFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDateTimeField.Should()
                .Be(this.returnDateTime);
        }

        [Test]
        public void MapNullableDateTimeFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDateTimeFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableDateTimeFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDateTimeField.Should()
                .Be(this.returnDateTime);
        }

        [Test]
        public void LeaveNullableDateTimeFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDateTimeFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableDateTimeFieldName);
            builder.MapFromColumnName(NullableDateTimeFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDateTimeField.Should()
                .Be(default(DateTime?));
        }

        private const string DateTimeFieldName = "DateTimeField";
        private const string DateTimePropertyName = "DateTimeProperty";
        private const string NullableDateTimeFieldName = "NullableDateTimeField";
        private const string NullableDateTimePropertyName = "NullableDateTimeProperty";

        private MappingTestEntity entity;
        private DateTime returnDateTime;

        private EntityValueMemberMapperBuilder AssumeBuilderIsInitialised(string Name)
        {
            var memberInfo = typeof(MappingTestEntity).GetMember(Name)[0];
            var builder = new EntityValueMemberMapperBuilder(memberInfo);
            return builder;
        }

        private IDataRecord AssumeDataRecordIsInitialised(string Name, bool returnsNull = false)
        {
            this.returnDateTime = DateTime.Now;
            var dataRecord = Substitute.For<IDataRecord>();
            dataRecord.FieldCount.Returns(1);
            dataRecord.GetName(0)
                      .Returns(Name);
            dataRecord.GetDateTime(0)
                      .Returns(this.returnDateTime);
            dataRecord.IsDBNull(0)
                      .Returns(returnsNull);
            return dataRecord;
        }
    }
}