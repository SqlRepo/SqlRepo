using System;
using System.Data;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMemberMapperShortShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }

        [Test]
        public void MapShortPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(ShortPropertyName);
            var builder = this.AssumeBuilderIsInitialised(ShortPropertyName);
            builder.MapFromColumnName(ShortPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.ShortProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapShortPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(ShortPropertyName);
            var builder = this.AssumeBuilderIsInitialised(ShortPropertyName);
            builder.MapFromColumnName(ShortPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.ShortProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveShortPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(ShortPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(ShortPropertyName);
            builder.MapFromColumnName(ShortPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.ShortProperty.Should()
                .Be(default(short));
        }

        [Test]
        public void MapNullableShortPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableShortPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableShortPropertyName);
            builder.MapFromColumnName(NullableShortPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableShortProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableShortPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableShortPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableShortPropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableShortProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableShortPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableShortPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableShortPropertyName);
            builder.MapFromColumnName(NullableShortPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableShortProperty.Should()
                .Be(default(short?));
        }

        [Test]
        public void MapShortFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(ShortFieldName);
            var builder = this.AssumeBuilderIsInitialised(ShortFieldName);
            builder.MapFromColumnName(ShortFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.ShortField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapShortFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(ShortFieldName);
            var builder = this.AssumeBuilderIsInitialised(ShortFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.ShortField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveShortFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(ShortFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(ShortFieldName);
            builder.MapFromColumnName(ShortFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.ShortField.Should()
                .Be(default(short));
        }

        [Test]
        public void MapNullableShortFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableShortFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableShortFieldName);
            builder.MapFromColumnName(NullableShortFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableShortField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableShortFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableShortFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableShortFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableShortField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableShortFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableShortFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableShortFieldName);
            builder.MapFromColumnName(NullableShortFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableShortField.Should()
                .Be(default(short?));
        }

        private const short DbReturnValue = 1;
        private const string ShortFieldName = "ShortField";
        private const string ShortPropertyName = "ShortProperty";
        private const string NullableShortFieldName = "NullableShortField";
        private const string NullableShortPropertyName = "NullableShortProperty";

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
            dataRecord.GetInt16(0)
                      .Returns(DbReturnValue);
            dataRecord.IsDBNull(0)
                      .Returns(returnsNull);
            return dataRecord;
        }
    }
}