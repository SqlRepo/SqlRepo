using System;
using System.Data;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMemberMapperLongShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }

        [Test]
        public void MapLongPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(LongPropertyName);
            var builder = this.AssumeBuilderIsInitialised(LongPropertyName);
            builder.MapFromColumnName(LongPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.LongProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapLongPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(LongPropertyName);
            var builder = this.AssumeBuilderIsInitialised(LongPropertyName);
            builder.MapFromColumnName(LongPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.LongProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveLongPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(LongPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(LongPropertyName);
            builder.MapFromColumnName(LongPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.LongProperty.Should()
                .Be(default(long));
        }

        [Test]
        public void MapNullableLongPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableLongPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableLongPropertyName);
            builder.MapFromColumnName(NullableLongPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableLongProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableLongPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableLongPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableLongPropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableLongProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableLongPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableLongPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableLongPropertyName);
            builder.MapFromColumnName(NullableLongPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableLongProperty.Should()
                .Be(default(long?));
        }

        [Test]
        public void MapLongFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(LongFieldName);
            var builder = this.AssumeBuilderIsInitialised(LongFieldName);
            builder.MapFromColumnName(LongFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.LongField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapLongFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(LongFieldName);
            var builder = this.AssumeBuilderIsInitialised(LongFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.LongField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveLongFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(LongFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(LongFieldName);
            builder.MapFromColumnName(LongFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.LongField.Should()
                .Be(default(long));
        }

        [Test]
        public void MapNullableLongFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableLongFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableLongFieldName);
            builder.MapFromColumnName(NullableLongFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableLongField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableLongFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableLongFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableLongFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableLongField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableLongFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableLongFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableLongFieldName);
            builder.MapFromColumnName(NullableLongFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableLongField.Should()
                .Be(default(long?));
        }

        private const long DbReturnValue = 1;
        private const string LongFieldName = "LongField";
        private const string LongPropertyName = "LongProperty";
        private const string NullableLongFieldName = "NullableLongField";
        private const string NullableLongPropertyName = "NullableLongProperty";

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
            dataRecord.GetInt64(0)
                      .Returns(DbReturnValue);
            dataRecord.IsDBNull(0)
                      .Returns(returnsNull);
            return dataRecord;
        }
    }
}