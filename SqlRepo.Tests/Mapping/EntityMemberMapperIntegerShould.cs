using System;
using System.Data;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMemberMapperIntegerShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }

        [Test]
        public void MapIntegerPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(IntPropertyName);
            var builder = this.AssumeBuilderIsInitialised(IntPropertyName);
            builder.MapFromColumnName(IntPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.IntProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapIntegerPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(IntPropertyName);
            var builder = this.AssumeBuilderIsInitialised(IntPropertyName);
            builder.MapFromColumnName(IntPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.IntProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveIntegerPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(IntPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(IntPropertyName);
            builder.MapFromColumnName(IntPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.IntProperty.Should()
                .Be(default(int));
        }

        [Test]
        public void MapNullableIntegerPropertyByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableIntPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableIntPropertyName);
            builder.MapFromColumnName(NullableIntPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableIntProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableIntegerPropertyByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableIntPropertyName);
            var builder = this.AssumeBuilderIsInitialised(NullableIntPropertyName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableIntProperty.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableIntegerPropertyAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableIntPropertyName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableIntPropertyName);
            builder.MapFromColumnName(NullableIntPropertyName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableIntProperty.Should()
                .Be(default(int?));
        }

        [Test]
        public void MapIntegerFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(IntFieldName);
            var builder = this.AssumeBuilderIsInitialised(IntFieldName);
            builder.MapFromColumnName(IntFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.IntField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapIntegerFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(IntFieldName);
            var builder = this.AssumeBuilderIsInitialised(IntFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.IntField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveIntegerFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(IntFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(IntFieldName);
            builder.MapFromColumnName(IntFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.IntField.Should()
                .Be(default(int));
        }

        [Test]
        public void MapNullableIntegerFieldByColumnNameCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableIntFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableIntFieldName);
            builder.MapFromColumnName(NullableIntFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableIntField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void MapNullableIntegerFieldByColumnIndexCorrectly()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableIntFieldName);
            var builder = this.AssumeBuilderIsInitialised(NullableIntFieldName);
            builder.MapFromIndex(0);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableIntField.Should()
                .Be(DbReturnValue);
        }

        [Test]
        public void LeaveNullableIntegerFieldAsDefaultValueWhenDatabaseValueIsNull()
        {
            var dataRecord = this.AssumeDataRecordIsInitialised(NullableIntFieldName, true);
            var builder = this.AssumeBuilderIsInitialised(NullableIntFieldName);
            builder.MapFromColumnName(NullableIntFieldName);
            var target = builder.Build();

            target.Map(this.entity, dataRecord);

            this.entity.NullableIntField.Should()
                .Be(default(int?));
        }

        private const int DbReturnValue = 1;
        private const string IntFieldName = "IntField";
        private const string IntPropertyName = "IntProperty";
        private const string NullableIntFieldName = "NullableIntField";
        private const string NullableIntPropertyName = "NullableIntProperty";

        private MappingTestEntity entity;

        private EntityMemberMapperBuilder<MappingTestEntity, int> AssumeBuilderIsInitialised(string Name)
        {
            var memberInfo = typeof(MappingTestEntity).GetMember(Name)[0];
            var builder = new EntityMemberMapperBuilder<MappingTestEntity, int>(memberInfo);
            return builder;
        }

        private IDataRecord AssumeDataRecordIsInitialised(string Name, bool returnsNull = false)
        {
            var dataRecord = Substitute.For<IDataRecord>();
            dataRecord.FieldCount.Returns(1);
            dataRecord.GetName(0)
                      .Returns(Name);
            dataRecord.GetInt32(0)
                      .Returns(DbReturnValue);
            dataRecord.IsDBNull(0)
                      .Returns(returnsNull);
            return dataRecord;
        }
    }
}