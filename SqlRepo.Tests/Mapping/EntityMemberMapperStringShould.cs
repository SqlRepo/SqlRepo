using System;
using System.Data;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMemberMapperStringShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }

        [Test]
        public void MapStringPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(StringPropertyName);
            var builder = this.AssumeBuilderIsInitialised(StringPropertyName);
            builder.MapFromColumnName(StringPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.StringProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapStringPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(StringPropertyName);
            var builder = this.AssumeBuilderIsInitialised(StringPropertyName);
            builder.MapFromColumnName(StringPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.StringProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveStringPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(StringPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(StringPropertyName);
            builder.MapFromColumnName(StringPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.StringProperty.Should()
                .Be(default(string));
        }

        [Test]
        public void MapStringFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(StringFieldName);
            var builder = this.AssumeBuilderIsInitialised(StringFieldName);
            builder.MapFromColumnName(StringFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.StringField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapStringFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(StringFieldName);
            var builder = this.AssumeBuilderIsInitialised(StringFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.StringField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveStringFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(StringFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(StringFieldName);
            builder.MapFromColumnName(StringFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.StringField.Should()
                .Be(default(string));
        }

        private const string DbReturnValue = "something";
        private const string StringFieldName = "StringField";
        private const string StringPropertyName = "StringProperty";

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
            dataRecord.GetString(0)
                      .Returns(DbReturnValue);
            dataRecord.IsDBNull(0)
                      .Returns(returnsNull);
            return dataRecord;
        }
    }
}