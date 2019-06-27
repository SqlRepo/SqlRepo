using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class EntityEnumerableMemberMapperShould
    {
        [SetUp]
        public void SetUp()
        {
            this.entity = new MappingTestEntity();
        }

        [Test]
        public void MapPropertyToListCorrectlyWhenMemberNotInitialised()
        {
            this.AssumeDataRecordsAreInitialised();
            this.AssumeMemberInfoIsInitialised("EnumerableProperty");

            this.AssumeColumnNameMappingProfileIsInitialised();
            this.AssumeListMemberMapperIsInitialised();

            this.listMapper.Map(this.entity, this.dataRecord1);

            this.entity.EnumerableProperty.Should()
                .NotBeNullOrEmpty();
            this.entity.EnumerableProperty.Should()
                .BeAssignableTo<List<InnerEntity>>();
            this.entity.EnumerableProperty.Count()
                .Should()
                .Be(1);

            var item = this.entity.EnumerableProperty.First();
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
        public void AddItemToExistingList()
        {
            this.AssumeDataRecordsAreInitialised();
            this.AssumeMemberInfoIsInitialised("EnumerableProperty");

            this.AssumeColumnNameMappingProfileIsInitialised();
            this.AssumeListMemberMapperIsInitialised();

            this.entity.EnumerableProperty = new List<InnerEntity>();

            this.listMapper.Map(this.entity, this.dataRecord1);

            this.entity.EnumerableProperty.Should()
                .NotBeNullOrEmpty();
            this.entity.EnumerableProperty.Count()
                .Should()
                .Be(1);

            var item = this.entity.EnumerableProperty.First();
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
        public void MapPropertyToListCorrectlyAndAddItemWhenMemberNotInitialised()
        {
            this.AssumeDataRecordsAreInitialised();
            this.AssumeMemberInfoIsInitialised("EnumerableProperty");

            this.AssumeColumnNameMappingProfileIsInitialised();
            this.AssumeListMemberMapperIsInitialised();

            this.listMapper.Map(this.entity, this.dataRecord1);
            this.listMapper.Map(this.entity, this.dataRecord2);

            this.entity.EnumerableProperty.Should()
                .NotBeNullOrEmpty();
            this.entity.EnumerableProperty.Should()
                .BeAssignableTo<List<InnerEntity>>();
            this.entity.EnumerableProperty.Count()
                .Should()
                .Be(2);

            var first = this.entity.EnumerableProperty.First();
            first.Id.Should()
                .Be(IdColumnValue1);
            first.TestEntityId.Should()
                .Be(TestEntityIdValue);
            first.StringProperty.Should()
                .Be(StringPropertyValue);
            first.IntProperty.Should()
                .Be(IntPropertyValue);

            var last = this.entity.EnumerableProperty.Last();
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
        public void MapPropertyToCollectionCorrectlyAndAddItemWhenMemberNotInitialised()
        {
            this.AssumeDataRecordsAreInitialised();
            this.AssumeMemberInfoIsInitialised("EnumerableProperty");

            this.AssumeColumnNameMappingProfileIsInitialised();

            var mapper =
                new EntityEnumerableMemberMapper<Collection<InnerEntity>, InnerEntity>(this.memberInfo,
                    this.itemMappingProfile);
            mapper.Map(this.entity, this.dataRecord1);
            mapper.Map(this.entity, this.dataRecord2);

            this.entity.EnumerableProperty.Should()
                .NotBeNullOrEmpty();
            this.entity.EnumerableProperty.Should()
                .BeAssignableTo<Collection<InnerEntity>>();
            this.entity.EnumerableProperty.Count()
                .Should()
                .Be(2);

            var first = this.entity.EnumerableProperty.First();
            first.Id.Should()
                 .Be(IdColumnValue1);
            first.TestEntityId.Should()
                 .Be(TestEntityIdValue);
            first.StringProperty.Should()
                 .Be(StringPropertyValue);
            first.IntProperty.Should()
                 .Be(IntPropertyValue);

            var last = this.entity.EnumerableProperty.Last();
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
        public void MapFieldToListCorrectlyAndAddItemWhenMemberNotInitialised()
        {
            this.AssumeDataRecordsAreInitialised();
            this.AssumeMemberInfoIsInitialised("EnumerableField");

            this.AssumeColumnNameMappingProfileIsInitialised();
            this.AssumeListMemberMapperIsInitialised();

            this.listMapper.Map(this.entity, this.dataRecord1);
            this.listMapper.Map(this.entity, this.dataRecord2);

            this.entity.EnumerableField.Should()
                .NotBeNullOrEmpty();
            this.entity.EnumerableField.Should()
                .BeAssignableTo<List<InnerEntity>>();
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

        [Test]
        public void MapFieldToCollectionCorrectlyAndAddItemWhenMemberNotInitialised()
        {
            this.AssumeDataRecordsAreInitialised();
            this.AssumeMemberInfoIsInitialised("EnumerableField");

            this.AssumeColumnNameMappingProfileIsInitialised();

            var mapper =
                new EntityEnumerableMemberMapper<Collection<InnerEntity>, InnerEntity>(this.memberInfo,
                    this.itemMappingProfile);
            mapper.Map(this.entity, this.dataRecord1);
            mapper.Map(this.entity, this.dataRecord2);

            this.entity.EnumerableField.Should()
                .NotBeNullOrEmpty();
            this.entity.EnumerableField.Should()
                .BeAssignableTo<Collection<InnerEntity>>();
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
        private EntityEnumerableMemberMapper<List<InnerEntity>, InnerEntity> listMapper;
        private MemberInfo memberInfo;

        private void AssumeColumnNameMappingProfileIsInitialised()
        {
            this.itemMappingProfile = new EntityMappingProfile<InnerEntity>();
            this.itemMappingProfile.ForMember(e => e.Id, c => c.MapFromColumnName(InnerEntityIdColumnName))
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

        private void AssumeListMemberMapperIsInitialised()
        {
            this.listMapper =
                new EntityEnumerableMemberMapper<List<InnerEntity>, InnerEntity>(this.memberInfo,
                    this.itemMappingProfile);
        }
    }
}