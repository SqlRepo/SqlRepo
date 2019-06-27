using System;
using System.Data;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityValueMemberMapperDoubleShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }

        [Test]
        public void MapDoublePropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DoublePropertyName);
            var builder = this.AssumeBuilderIsInitialised(DoublePropertyName);
            builder.MapFromColumnName(DoublePropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DoubleProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapDoublePropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DoublePropertyName);
            var builder = this.AssumeBuilderIsInitialised(DoublePropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DoubleProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveDoublePropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DoublePropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(DoublePropertyName);
            builder.MapFromColumnName(DoublePropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DoubleProperty.Should()
                .Be(default(double));
        }

        [Test]
        public void MapNullableDoublePropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDoublePropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableDoublePropertyName);
            builder.MapFromColumnName(NullableDoublePropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDoubleProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableDoublePropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDoublePropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableDoublePropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDoubleProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableDoublePropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDoublePropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableDoublePropertyName);
            builder.MapFromColumnName(NullableDoublePropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDoubleProperty.Should()
                .Be(default(double?));
        }

        [Test]
        public void MapDoubleFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DoubleFieldName);
            var builder = this.AssumeBuilderIsInitialised(DoubleFieldName);
            builder.MapFromColumnName(DoubleFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DoubleField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapDoubleFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DoubleFieldName);
            var builder = this.AssumeBuilderIsInitialised(DoubleFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DoubleField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveDoubleFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DoubleFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(DoubleFieldName);
            builder.MapFromColumnName(DoubleFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DoubleField.Should()
                .Be(default(double));
        }

        [Test]
        public void MapNullableDoubleFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDoubleFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableDoubleFieldName);
            builder.MapFromColumnName(NullableDoubleFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDoubleField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableDoubleFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDoubleFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableDoubleFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDoubleField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableDoubleFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDoubleFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableDoubleFieldName);
            builder.MapFromColumnName(NullableDoubleFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDoubleField.Should()
                .Be(default(double?));
        }

        private const double DbReturnValue = 1;
        private const string DoubleFieldName = "DoubleField";
        private const string DoublePropertyName = "DoubleProperty";
        private const string NullableDoubleFieldName = "NullableDoubleField";
        private const string NullableDoublePropertyName = "NullableDoubleProperty";

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
            dataRecord.GetDouble(0)
                      .Returns(DbReturnValue);
            dataRecord.IsDBNull(0)
                      .Returns(returnsNull);
            return dataRecord;
        }
    }
}