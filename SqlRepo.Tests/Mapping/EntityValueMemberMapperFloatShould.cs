using System;
using System.Data;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityValueMemberMapperFloatShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }

        [Test]
        public void MapFloatPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(FloatPropertyName);
            var builder = this.AssumeBuilderIsInitialised(FloatPropertyName);
            builder.MapFromColumnName(FloatPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.FloatProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapFloatPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(FloatPropertyName);
            var builder = this.AssumeBuilderIsInitialised(FloatPropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.FloatProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveFloatPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(FloatPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(FloatPropertyName);
            builder.MapFromColumnName(FloatPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.FloatProperty.Should()
                .Be(default(float));
        }

        [Test]
        public void MapNullableFloatPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableFloatPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableFloatPropertyName);
            builder.MapFromColumnName(NullableFloatPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableFloatProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableFloatPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableFloatPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableFloatPropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableFloatProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableFloatPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableFloatPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableFloatPropertyName);
            builder.MapFromColumnName(NullableFloatPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableFloatProperty.Should()
                .Be(default(float?));
        }

        [Test]
        public void MapFloatFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(FloatFieldName);
            var builder = this.AssumeBuilderIsInitialised(FloatFieldName);
            builder.MapFromColumnName(FloatFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.FloatField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapFloatFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(FloatFieldName);
            var builder = this.AssumeBuilderIsInitialised(FloatFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.FloatField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveFloatFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(FloatFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(FloatFieldName);
            builder.MapFromColumnName(FloatFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.FloatField.Should()
                .Be(default(float));
        }

        [Test]
        public void MapNullableFloatFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableFloatFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableFloatFieldName);
            builder.MapFromColumnName(NullableFloatFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableFloatField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableFloatFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableFloatFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableFloatFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableFloatField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableFloatFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableFloatFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableFloatFieldName);
            builder.MapFromColumnName(NullableFloatFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableFloatField.Should()
                .Be(default(float?));
        }

        private const float DbReturnValue = 1;
        private const string FloatFieldName = "FloatField";
        private const string FloatPropertyName = "FloatProperty";
        private const string NullableFloatFieldName = "NullableFloatField";
        private const string NullableFloatPropertyName = "NullableFloatProperty";

        private MappingTestEntity entity;

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
            dataRecord.GetFloat(0)
                      .Returns(DbReturnValue);
            dataRecord.IsDBNull(0)
                      .Returns(returnsNull);
            return dataRecord;
        }
    }
}