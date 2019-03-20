using System;
using System.Data;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMemberMapperByteShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }

        [Test]
        public void MapBytePropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(BytePropertyName);
            var builder = this.AssumeBuilderIsInitialised(BytePropertyName);
            builder.MapFromColumnName(BytePropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.ByteProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapBytePropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(BytePropertyName);
            var builder = this.AssumeBuilderIsInitialised(BytePropertyName);
            builder.MapFromColumnName(BytePropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.ByteProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveBytePropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(BytePropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(BytePropertyName);
            builder.MapFromColumnName(BytePropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.ByteProperty.Should()
                .Be(default(byte));
        }

        [Test]
        public void MapNullableBytePropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableBytePropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableBytePropertyName);
            builder.MapFromColumnName(NullableBytePropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableByteProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableBytePropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableBytePropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableBytePropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableByteProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableBytePropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableBytePropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableBytePropertyName);
            builder.MapFromColumnName(NullableBytePropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableByteProperty.Should()
                .Be(default(byte?));
        }

        [Test]
        public void MapByteFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(ByteFieldName);
            var builder = this.AssumeBuilderIsInitialised(ByteFieldName);
            builder.MapFromColumnName(ByteFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.ByteField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapByteFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(ByteFieldName);
            var builder = this.AssumeBuilderIsInitialised(ByteFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.ByteField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveByteFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(ByteFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(ByteFieldName);
            builder.MapFromColumnName(ByteFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.ByteField.Should()
                .Be(default(byte));
        }

        [Test]
        public void MapNullableByteFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableByteFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableByteFieldName);
            builder.MapFromColumnName(NullableByteFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableByteField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableByteFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableByteFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableByteFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableByteField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableByteFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableByteFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableByteFieldName);
            builder.MapFromColumnName(NullableByteFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableByteField.Should()
                .Be(default(byte?));
        }

        private const string ByteFieldName = "ByteField";
        private const string BytePropertyName = "ByteProperty";
        private const string NullableByteFieldName = "NullableByteField";
        private const string NullableBytePropertyName = "NullableByteProperty";

        private MappingTestEntity entity;
        private const byte DbReturnValue = 1;

        private EntityMemberMapperBuilder AssumeBuilderIsInitialised(string Name)
        {
            var memberInfo = typeof(MappingTestEntity).GetMember(Name)[0];
            var builder = new EntityMemberMapperBuilder(memberInfo);
            return builder;
        }

        private IDataRecord AssumeDataRecordIsInitialised(string Name, bool returnsNull = false)
        {
            var dataRecord = Substitute.For<IDataRecord>();
            dataRecord.FieldCount.Returns(1);
            dataRecord.GetName(0)
                      .Returns(Name);
            dataRecord.GetByte(0)
                      .Returns(DbReturnValue);
            dataRecord.IsDBNull(0)
                      .Returns(returnsNull);
            return dataRecord;
        }
    }
}