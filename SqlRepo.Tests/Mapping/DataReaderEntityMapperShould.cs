using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class DataReaderEntityMapperShould
    {
        [SetUp]
        public void Setup()
        {
            this.AssumeDataReaderIsInitialised();
            this.AssumeEntityMappingProfileProviderIsInitialised();
            this.dataReaderEntityMapper = new DataReaderEntityMapper(this.entityMappingProfileProvider);
        }

        [Test]
        public void UseProviderToGetEntityMapperDefinition()
        {
            this.AssumeTargetIsExecuted();

            this.entityMappingProfileProvider.Received().Get<TestEntity>();
        }

        [Test]
        public void GetStringValueFromDataReader()
        {
            this.AssumeTargetIsExecuted();
            this.dataReader.Received()
                .GetString(0);
        }

        [Test]
        public void MapPropertyValueFromReader()
        {
            this.AssumeTargetIsExecuted()
                .First()
                .StringProperty.Should()
                .Be("Test String 123");
        }

        private IDataReader dataReader;
        private DataReaderEntityMapper dataReaderEntityMapper;
        private IEntityMappingProfileProvider entityMappingProfileProvider;

        private void AssumeDataReaderIsInitialised()
        {
            this.dataReader = Substitute.For<IDataReader>();
            this.dataReader.FieldCount.Returns(1);
            this.dataReader.GetName(0)
                .Returns("StringProperty");
            this.dataReader.GetString(0)
                .Returns("Test String 123");
            this.dataReader.Read()
                .Returns(true, false);
        }

        private void AssumeEntityMappingProfileProviderIsInitialised()
        {
            this.entityMappingProfileProvider = Substitute.For<IEntityMappingProfileProvider>();
            this.entityMappingProfileProvider.Get<TestEntity>()
                .Returns(new DefaultEntityMappingProfile(typeof(TestEntity)));
        }

        private List<TestEntity> AssumeTargetIsExecuted()
        {
            return this.dataReaderEntityMapper.Map<TestEntity>(this.dataReader)
                       .ToList();
        }
    }
}