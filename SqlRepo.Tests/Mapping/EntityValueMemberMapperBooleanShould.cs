using System;
using System.Data;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityValueMemberMapperBooleanShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }

        [Test]
        public void MapBooleanPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(BooleanPropertyName);
            var builder = this.AssumeBuilderIsInitialised(BooleanPropertyName);
            builder.MapFromColumnName(BooleanPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.BooleanProperty.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void MapBooleanPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(BooleanPropertyName);
            var builder = this.AssumeBuilderIsInitialised(BooleanPropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.BooleanProperty.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void LeaveBooleanPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(BooleanPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(BooleanPropertyName);
            builder.MapFromColumnName(BooleanPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.BooleanProperty.Should()
                .Be(default(bool));
        }

        [Test]
        public void MapNullableBooleanPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableBooleanPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableBooleanPropertyName);
            builder.MapFromColumnName(NullableBooleanPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableBooleanProperty.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void MapNullableBooleanPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableBooleanPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableBooleanPropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableBooleanProperty.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void LeaveNullableBooleanPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableBooleanPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableBooleanPropertyName);
            builder.MapFromColumnName(NullableBooleanPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableBooleanProperty.Should()
                .Be(default(bool?));
        }

        [Test]
        public void MapBooleanFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(BooleanFieldName);
            var builder = this.AssumeBuilderIsInitialised(BooleanFieldName);
            builder.MapFromColumnName(BooleanFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.BooleanField.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void MapBooleanFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(BooleanFieldName);
            var builder = this.AssumeBuilderIsInitialised(BooleanFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.BooleanField.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void LeaveBooleanFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(BooleanFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(BooleanFieldName);
            builder.MapFromColumnName(BooleanFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.BooleanField.Should()
                .Be(default(bool));
        }

        [Test]
        public void MapNullableBooleanFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableBooleanFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableBooleanFieldName);
            builder.MapFromColumnName(NullableBooleanFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableBooleanField.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void MapNullableBooleanFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableBooleanFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableBooleanFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableBooleanField.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void LeaveNullableBooleanFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableBooleanFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableBooleanFieldName);
            builder.MapFromColumnName(NullableBooleanFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableBooleanField.Should()
                .Be(default(bool?));
        }

        private const string BooleanFieldName = "BooleanField";
        private const string BooleanPropertyName = "BooleanProperty";
        private const string NullableBooleanFieldName = "NullableBooleanField";
        private const string NullableBooleanPropertyName = "NullableBooleanProperty";

        private MappingTestEntity entity;
        private bool dbReturnValue;

        private EntityValueMemberMapperBuilder AssumeBuilderIsInitialised(string Name)
        {
            var memberInfo = typeof(MappingTestEntity).GetMember(Name)[0];
            var builder = new EntityValueMemberMapperBuilder(memberInfo);
            return builder;
        }

        private IDataRecord AssumeDataRecordIsInitialised(string Name, bool returnsNull = false)
        {
            var dataRecord = Substitute.For<IDataRecord>();
            dataRecord.FieldCount.Returns(1);
            dataRecord.GetName(0)
                      .Returns(Name);
            this.dbReturnValue = true;
            dataRecord.GetBoolean(0)
                      .Returns(this.dbReturnValue);
            dataRecord.IsDBNull(0)
                      .Returns(returnsNull);
            return dataRecord;
        }
    }
}