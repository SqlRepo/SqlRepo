using System;
using System.Data;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMemberMapperDecimalShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }

        [Test]
        public void MapDecimalPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DecimalPropertyName);
            var builder = this.AssumeBuilderIsInitialised(DecimalPropertyName);
            builder.MapFromColumnName(DecimalPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DecimalProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapDecimalPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DecimalPropertyName);
            var builder = this.AssumeBuilderIsInitialised(DecimalPropertyName);
            builder.MapFromColumnName(DecimalPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DecimalProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveDecimalPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DecimalPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(DecimalPropertyName);
            builder.MapFromColumnName(DecimalPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DecimalProperty.Should()
                .Be(default(decimal));
        }

        [Test]
        public void MapNullableDecimalPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDecimalPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableDecimalPropertyName);
            builder.MapFromColumnName(NullableDecimalPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDecimalProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableDecimalPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDecimalPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableDecimalPropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDecimalProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableDecimalPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDecimalPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableDecimalPropertyName);
            builder.MapFromColumnName(NullableDecimalPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDecimalProperty.Should()
                .Be(default(decimal?));
        }

        [Test]
        public void MapDecimalFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DecimalFieldName);
            var builder = this.AssumeBuilderIsInitialised(DecimalFieldName);
            builder.MapFromColumnName(DecimalFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DecimalField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapDecimalFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DecimalFieldName);
            var builder = this.AssumeBuilderIsInitialised(DecimalFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DecimalField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveDecimalFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(DecimalFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(DecimalFieldName);
            builder.MapFromColumnName(DecimalFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.DecimalField.Should()
                .Be(default(decimal));
        }

        [Test]
        public void MapNullableDecimalFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDecimalFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableDecimalFieldName);
            builder.MapFromColumnName(NullableDecimalFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDecimalField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableDecimalFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDecimalFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableDecimalFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDecimalField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableDecimalFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableDecimalFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableDecimalFieldName);
            builder.MapFromColumnName(NullableDecimalFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableDecimalField.Should()
                .Be(default(decimal?));
        }

        private const decimal DbReturnValue = 1;
        private const string DecimalFieldName = "DecimalField";
        private const string DecimalPropertyName = "DecimalProperty";
        private const string NullableDecimalFieldName = "NullableDecimalField";
        private const string NullableDecimalPropertyName = "NullableDecimalProperty";

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
            dataRecord.GetDecimal(0)
                      .Returns(DbReturnValue);
            dataRecord.IsDBNull(0)
                      .Returns(returnsNull);
            return dataRecord;
        }
    }
}