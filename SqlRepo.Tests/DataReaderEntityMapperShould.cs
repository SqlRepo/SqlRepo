using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class DataReaderEntityMapperShould
    {
        [SetUp]
        public void Setup()
        {
            this.dataReaderEntityMapper = new DataReaderEntityMapper();
            this.dataReader = Substitute.For<IDataReader>();
            this.dataReader.FieldCount.Returns(1);
            this.dataReader.GetName(0).Returns("StringProperty");
            this.dataReader.GetString(0).Returns("Test String 123");
            this.dataReader.Read().Returns(true, false);
        }

        private DataReaderEntityMapper dataReaderEntityMapper;
        private IDataReader dataReader;

        private List<TestEntity> AssumeTargetIsExecuted()
        {
            return this.dataReaderEntityMapper.Map<TestEntity>(this.dataReader).ToList();
        }

        [Test]
        public void GetStringValueFromDataReader()
        {
            this.AssumeTargetIsExecuted();
            this.dataReader.Received().GetString(0);
        }

        [Test]
        public void SetStringValue()
        {
            this.AssumeTargetIsExecuted().First().StringProperty.Should().Be("Test String 123");
        }
    }
}