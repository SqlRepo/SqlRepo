using System;
using System.Data;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMemberMapperDateTimeOffsetShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }

        [Test]
        public void MapDateTimeOffsetPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DateTimeOffsetPropertyName);
            var builder = this.AssumeBuilderIsInitialised(DateTimeOffsetPropertyName);
            builder.MapFromColumnName(DateTimeOffsetPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DateTimeOffsetProperty.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void MapDateTimeOffsetPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DateTimeOffsetPropertyName);
            var builder = this.AssumeBuilderIsInitialised(DateTimeOffsetPropertyName);
            builder.MapFromColumnName(DateTimeOffsetPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DateTimeOffsetProperty.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void LeaveDateTimeOffsetPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DateTimeOffsetPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(DateTimeOffsetPropertyName);
            builder.MapFromColumnName(DateTimeOffsetPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DateTimeOffsetProperty.Should()
                .Be(default(DateTimeOffset));
        }

        [Test]
        public void MapNullableDateTimeOffsetPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDateTimeOffsetPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableDateTimeOffsetPropertyName);
            builder.MapFromColumnName(NullableDateTimeOffsetPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDateTimeOffsetProperty.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void MapNullableDateTimeOffsetPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDateTimeOffsetPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableDateTimeOffsetPropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDateTimeOffsetProperty.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void LeaveNullableDateTimeOffsetPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDateTimeOffsetPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableDateTimeOffsetPropertyName);
            builder.MapFromColumnName(NullableDateTimeOffsetPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDateTimeOffsetProperty.Should()
                .Be(default(DateTimeOffset?));
        }

        [Test]
        public void MapDateTimeOffsetFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DateTimeOffsetFieldName);
            var builder = this.AssumeBuilderIsInitialised(DateTimeOffsetFieldName);
            builder.MapFromColumnName(DateTimeOffsetFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DateTimeOffsetField.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void MapDateTimeOffsetFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DateTimeOffsetFieldName);
            var builder = this.AssumeBuilderIsInitialised(DateTimeOffsetFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DateTimeOffsetField.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void LeaveDateTimeOffsetFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DateTimeOffsetFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(DateTimeOffsetFieldName);
            builder.MapFromColumnName(DateTimeOffsetFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DateTimeOffsetField.Should()
                .Be(default(DateTimeOffset));
        }

        [Test]
        public void MapNullableDateTimeOffsetFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDateTimeOffsetFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableDateTimeOffsetFieldName);
            builder.MapFromColumnName(NullableDateTimeOffsetFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDateTimeOffsetField.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void MapNullableDateTimeOffsetFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDateTimeOffsetFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableDateTimeOffsetFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDateTimeOffsetField.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void LeaveNullableDateTimeOffsetFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDateTimeOffsetFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableDateTimeOffsetFieldName);
            builder.MapFromColumnName(NullableDateTimeOffsetFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDateTimeOffsetField.Should()
                .Be(default(DateTimeOffset?));
        }
        private readonly DateTimeOffset dbReturnValue = DateTimeOffset.UtcNow;
        private const string DateTimeOffsetFieldName = "DateTimeOffsetField";
        private const string DateTimeOffsetPropertyName = "DateTimeOffsetProperty";
        private const string NullableDateTimeOffsetFieldName = "NullableDateTimeOffsetField";
        private const string NullableDateTimeOffsetPropertyName = "NullableDateTimeOffsetProperty";

        private MappingTestEntity entity;

        private EntityMemberMapperBuilder AssumeBuilderIsInitialised(string name)
        {
            var memberInfo = typeof(MappingTestEntity).GetMember(name)[0];
            var builder = new EntityMemberMapperBuilder(memberInfo);
            return builder;
        }

        private IDataRecord AssumeDataRecordIsInitialised(string name, bool returnsNull = false)
        {
            var dataRecord = Substitute.For<IDataRecord>();
            dataRecord.FieldCount.Returns(1);
            dataRecord.GetName(0)
                      .Returns(name);
            dataRecord.GetValue(0)
                      .Returns(this.dbReturnValue);
            dataRecord.IsDBNull(0)
                      .Returns(returnsNull);
            return dataRecord;
        }
    }
}