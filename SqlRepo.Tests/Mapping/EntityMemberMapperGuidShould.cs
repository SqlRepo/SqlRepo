using System;
using System.Data;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMemberMapperGuidShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }

        [Test]
        public void MapGuidPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(GuidPropertyName);
            var builder = this.AssumeBuilderIsInitialised(GuidPropertyName);
            builder.MapFromColumnName(GuidPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.GuidProperty.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void MapGuidPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(GuidPropertyName);
            var builder = this.AssumeBuilderIsInitialised(GuidPropertyName);
            builder.MapFromColumnName(GuidPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.GuidProperty.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void LeaveGuidPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(GuidPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(GuidPropertyName);
            builder.MapFromColumnName(GuidPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.GuidProperty.Should()
                .Be(default(Guid));
        }

        [Test]
        public void MapNullableGuidPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableGuidPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableGuidPropertyName);
            builder.MapFromColumnName(NullableGuidPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableGuidProperty.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void MapNullableGuidPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableGuidPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableGuidPropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableGuidProperty.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void LeaveNullableGuidPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableGuidPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableGuidPropertyName);
            builder.MapFromColumnName(NullableGuidPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableGuidProperty.Should()
                .Be(default(Guid?));
        }

        [Test]
        public void MapGuidFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(GuidFieldName);
            var builder = this.AssumeBuilderIsInitialised(GuidFieldName);
            builder.MapFromColumnName(GuidFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.GuidField.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void MapGuidFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(GuidFieldName);
            var builder = this.AssumeBuilderIsInitialised(GuidFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.GuidField.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void LeaveGuidFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(GuidFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(GuidFieldName);
            builder.MapFromColumnName(GuidFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.GuidField.Should()
                .Be(default(Guid));
        }

        [Test]
        public void MapNullableGuidFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableGuidFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableGuidFieldName);
            builder.MapFromColumnName(NullableGuidFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableGuidField.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void MapNullableGuidFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableGuidFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableGuidFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableGuidField.Should()
                .Be(this.dbReturnValue);
        }

        [Test]
        public void LeaveNullableGuidFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableGuidFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableGuidFieldName);
            builder.MapFromColumnName(NullableGuidFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableGuidField.Should()
                .Be(default(Guid?));
        }

        private const string GuidFieldName = "GuidField";
        private const string GuidPropertyName = "GuidProperty";
        private const string NullableGuidFieldName = "NullableGuidField";
        private const string NullableGuidPropertyName = "NullableGuidProperty";

        private readonly Guid dbReturnValue = Guid.NewGuid();

        private MappingTestEntity entity;

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
            dataRecord.GetGuid(0)
                      .Returns(this.dbReturnValue);
            dataRecord.IsDBNull(0)
                      .Returns(returnsNull);
            return dataRecord;
        }
    }
}