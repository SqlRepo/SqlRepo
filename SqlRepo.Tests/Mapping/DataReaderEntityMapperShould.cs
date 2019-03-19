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
            this.AssumeEntityMapperDefinitionProviderIsInitialised();
            this.dataReaderEntityMapper = new DataReaderEntityMapper(this.entityMapperDefinitionProvider);
        }

        [Test]
        public void UseProviderToGetEntityMapperDefinition()
        {
            this.AssumeTargetIsExecuted();

            this.entityMapperDefinitionProvider.Received().Get<TestEntity>();
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
        private IEntityMapperDefinitionProvider entityMapperDefinitionProvider;
        private EntityMapperDefinition<TestEntity> entityMapperDefinition;

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

        private void AssumeEntityMapperDefinitionProviderIsInitialised()
        {
            var realProvider = new EntityMapperDefinitionProvider(new EntityActivatorFactory());
            this.entityMapperDefinitionProvider = Substitute.For<IEntityMapperDefinitionProvider>();
            this.entityMapperDefinition = realProvider.Get<TestEntity>();
            this.entityMapperDefinitionProvider.Get<TestEntity>()
                .Returns(this.entityMapperDefinition);
        }

        private List<TestEntity> AssumeTargetIsExecuted()
        {
            return this.dataReaderEntityMapper.Map<TestEntity>(this.dataReader)
                       .ToList();
        }
    }
}