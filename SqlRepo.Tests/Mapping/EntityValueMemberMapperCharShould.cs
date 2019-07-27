using System;
using System.Data;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityValueMemberMapperCharShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }

        [Test]
        public void MapCharPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(CharPropertyName);
            var builder = this.AssumeBuilderIsInitialised(CharPropertyName);
            builder.MapFromColumnName(CharPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.CharProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapCharPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(CharPropertyName);
            var builder = this.AssumeBuilderIsInitialised(CharPropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.CharProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveCharPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(CharPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(CharPropertyName);
            builder.MapFromColumnName(CharPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.CharProperty.Should()
                .Be(default(char));
        }

        [Test]
        public void MapNullableCharPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableCharPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableCharPropertyName);
            builder.MapFromColumnName(NullableCharPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableCharProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableCharPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableCharPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableCharPropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableCharProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableCharPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableCharPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableCharPropertyName);
            builder.MapFromColumnName(NullableCharPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableCharProperty.Should()
                .Be(default(char?));
        }

        [Test]
        public void MapCharFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(CharFieldName);
            var builder = this.AssumeBuilderIsInitialised(CharFieldName);
            builder.MapFromColumnName(CharFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.CharField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapCharFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(CharFieldName);
            var builder = this.AssumeBuilderIsInitialised(CharFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.CharField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveCharFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(CharFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(CharFieldName);
            builder.MapFromColumnName(CharFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.CharField.Should()
                .Be(default(char));
        }

        [Test]
        public void MapNullableCharFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableCharFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableCharFieldName);
            builder.MapFromColumnName(NullableCharFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableCharField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableCharFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableCharFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableCharFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableCharField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableCharFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableCharFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableCharFieldName);
            builder.MapFromColumnName(NullableCharFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableCharField.Should()
                .Be(default(char?));
        }

        private const string CharFieldName = "CharField";
        private const string CharPropertyName = "CharProperty";
        private const string NullableCharFieldName = "NullableCharField";
        private const string NullableCharPropertyName = "NullableCharProperty";

        private MappingTestEntity entity;
        private const char DbReturnValue = 'a';

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
            dataRecord.GetChar(0)
                      .Returns(DbReturnValue);
            dataRecord.IsDBNull(0)
                      .Returns(returnsNull);
            return dataRecord;
        }
    }
}