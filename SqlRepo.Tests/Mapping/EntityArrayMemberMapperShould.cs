using System;
using System.Data;
using System.Linq;
using FluentAssertions;
using System.Reflection;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Testing;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityArrayMemberMapperShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }

        [Test]
        public void MapPropertyCorrectlyWhenMemberNotInitialised()
        {
            this.AssumeDataRecordsAreInitialised();
            this.AssumeMemberInfoIsInitialised("ArrayProperty");

            this.AssumeColumnNameMappingProfileIsInitialised();
            this.AssumeArrayMemberMapperIsInitialised();

            this.mapper.Map(this.entity, this.dataRecord1);

            this.entity.ArrayProperty.Should()
                .NotBeNullOrEmpty();
            this.entity.ArrayProperty.Should()
                .BeAssignableTo<InnerEntity[]>();
            this.entity.ArrayProperty.Count()
                .Should()
                .Be(1);

            var item = this.entity.ArrayProperty.First();
            item.Id.Should()
                .Be(IdColumnValue1);
            item.TestEntityId.Should()
                .Be(TestEntityIdValue);
            item.StringProperty.Should()
                .Be(StringPropertyValue);
            item.IntProperty.Should()
                .Be(IntPropertyValue);
        }

        [Test]
        public void AddItemToExistingArray()
        {
            this.AssumeDataRecordsAreInitialised();
            this.AssumeMemberInfoIsInitialised("ArrayProperty");

            this.AssumeColumnNameMappingProfileIsInitialised();
            this.AssumeArrayMemberMapperIsInitialised();

            this.entity.ArrayProperty = new InnerEntity[0];

            this.mapper.Map(this.entity, this.dataRecord1);

            this.entity.ArrayProperty.Should()
                .NotBeNullOrEmpty();
            this.entity.ArrayProperty.Count()
                .Should()
                .Be(1);

            var item = this.entity.ArrayProperty.First();
            item.Id.Should()
                .Be(IdColumnValue1);
            item.TestEntityId.Should()
                .Be(TestEntityIdValue);
            item.StringProperty.Should()
                .Be(StringPropertyValue);
            item.IntProperty.Should()
                .Be(IntPropertyValue);
        }

        [Test]
        public void MapPropertyCorrectlyAndAddItemWhenMemberNotInitialised()
        {
            this.AssumeDataRecordsAreInitialised();
            this.AssumeMemberInfoIsInitialised("ArrayProperty");

            this.AssumeColumnNameMappingProfileIsInitialised();
            this.AssumeArrayMemberMapperIsInitialised();

            this.mapper.Map(this.entity, this.dataRecord1);
            this.mapper.Map(this.entity, this.dataRecord2);

            this.entity.ArrayProperty.Should()
                .NotBeNullOrEmpty();
            this.entity.ArrayProperty.Should()
                .BeAssignableTo<InnerEntity[]>();
            this.entity.ArrayProperty.Count()
                .Should()
                .Be(2);

            var first = this.entity.ArrayProperty.First();
            first.Id.Should()
                 .Be(IdColumnValue1);
            first.TestEntityId.Should()
                 .Be(TestEntityIdValue);
            first.StringProperty.Should()
                 .Be(StringPropertyValue);
            first.IntProperty.Should()
                 .Be(IntPropertyValue);

            var last = this.entity.ArrayProperty.Last();
            last.Id.Should()
                .Be(IdColumnValue2);
            last.TestEntityId.Should()
                .Be(TestEntityIdValue);
            last.StringProperty.Should()
                .Be(StringPropertyValue);
            last.IntProperty.Should()
                .Be(IntPropertyValue);
        }

        [Test]
        public void MapFieldCorrectlyAndAddItemWhenMemberNotInitialised()
        {
            this.AssumeDataRecordsAreInitialised();
            this.AssumeMemberInfoIsInitialised("EnumerableField");

            this.AssumeColumnNameMappingProfileIsInitialised();
            this.AssumeArrayMemberMapperIsInitialised();

            this.mapper.Map(this.entity, this.dataRecord1);
            this.mapper.Map(this.entity, this.dataRecord2);

            this.entity.EnumerableField.Should()
                .NotBeNullOrEmpty();
            this.entity.EnumerableField.Should()
                .BeAssignableTo<InnerEntity[]>();
            this.entity.EnumerableField.Count()
                .Should()
                .Be(2);

            var first = this.entity.EnumerableField.First();
            first.Id.Should()
                 .Be(IdColumnValue1);
            first.TestEntityId.Should()
                 .Be(TestEntityIdValue);
            first.StringProperty.Should()
                 .Be(StringPropertyValue);
            first.IntProperty.Should()
                 .Be(IntPropertyValue);

            var last = this.entity.EnumerableField.Last();
            last.Id.Should()
                .Be(IdColumnValue2);
            last.TestEntityId.Should()
                .Be(TestEntityIdValue);
            last.StringProperty.Should()
                .Be(StringPropertyValue);
            last.IntProperty.Should()
                .Be(IntPropertyValue);
        }

        private const int IdColumnValue1 = 1;
        private const int IdColumnValue2 = 2;
        private const string InnerEntityIdColumnName = "InnerEntity_Id";
        private const string InnerEntityIntPropertyColumnName = "InnerEntity_IntProperty";
        private const string InnerEntityStringPropertyColumnName = "InnerEntity_StringProperty";
        private const string InnerEntityTestEntityIdColumnName = "InnerEntity_TestEntityId";
        private const int IntPropertyValue = 3;
        private const string StringPropertyValue = "StringProperty";
        private const int TestEntityIdValue = 2;
        private IDataRecord dataRecord1;
        private IDataRecord dataRecord2;
        private MappingTestEntity entity;
        private IEntityMappingProfile<InnerEntity> itemMappingProfile;
        private EntityArrayMemberMapper<InnerEntity> mapper;
        private MemberInfo memberInfo;

        private void AssumeArrayMemberMapperIsInitialised()
        {
            this.mapper = new EntityArrayMemberMapper<InnerEntity>(this.memberInfo, this.itemMappingProfile);
        }

        private void AssumeColumnNameMappingProfileIsInitialised()
        {
            this.itemMappingProfile = new EntityMappingProfile<InnerEntity>();
            this.itemMappingProfile.ForMember(e => e.Id, c => c.MapFromColumnName(InnerEntityIdColumnName, true))
                .ForMember(e => e.TestEntityId, c => c.MapFromColumnName(InnerEntityTestEntityIdColumnName))
                .ForMember(e => e.StringProperty,
                    c => c.MapFromColumnName(InnerEntityStringPropertyColumnName))
                .ForMember(e => e.IntProperty, c => c.MapFromColumnName(InnerEntityIntPropertyColumnName));
        }

        private void AssumeDataRecordsAreInitialised()
        {
            this.dataRecord1 = DataRecordMockBuilder.CreateNew()
                                                    .WithIntColumn(InnerEntityIdColumnName,
                                                        0,
                                                        IdColumnValue1)
                                                    .WithIntColumn(InnerEntityTestEntityIdColumnName,
                                                        1,
                                                        TestEntityIdValue)
                                                    .WithStringColumn(InnerEntityStringPropertyColumnName,
                                                        2,
                                                        StringPropertyValue)
                                                    .WithIntColumn(InnerEntityIntPropertyColumnName, 3, 3)
                                                    .Build();

            this.dataRecord2 = DataRecordMockBuilder.CreateNew()
                                                    .WithIntColumn(InnerEntityIdColumnName,
                                                        0,
                                                        IdColumnValue2)
                                                    .WithIntColumn(InnerEntityTestEntityIdColumnName,
                                                        1,
                                                        TestEntityIdValue)
                                                    .WithStringColumn(InnerEntityStringPropertyColumnName,
                                                        TestEntityIdValue,
                                                        StringPropertyValue)
                                                    .WithIntColumn(InnerEntityIntPropertyColumnName,
                                                        3,
                                                        IntPropertyValue)
                                                    .Build();
        }

        private void AssumeMemberInfoIsInitialised(string memberName)
        {
            this.memberInfo = typeof(MappingTestEntity).GetMember(memberName)[0];
        }
    }
}