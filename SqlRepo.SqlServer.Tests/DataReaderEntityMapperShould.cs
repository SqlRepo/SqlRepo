using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.SqlServer.Tests
{
    [TestFixture]
    public class DataReaderEntityMapperShould
    {
        [SetUp]
        public void Setup()
        {
            dataReaderEntityMapper = new DataReaderEntityMapper();
            dataReader = Substitute.For<IDataReader>();
            dataReader.FieldCount.Returns(1);
            dataReader.GetName(0).Returns("StringProperty");
            dataReader.GetString(0).Returns("Test String 123");
            dataReader.Read().Returns(true, false);
        }

        private DataReaderEntityMapper dataReaderEntityMapper;
        private IDataReader dataReader;

        private List<TestEntity> AssumeTargetIsExecuted()
        {
            return dataReaderEntityMapper.Map<TestEntity>(dataReader).ToList();
        }

        [Test]
        public void GetStringValueFromDataReader()
        {
            AssumeTargetIsExecuted();
            dataReader.Received().GetString(0);
        }

        [Test]
        public void SetStringValue()
        {
            AssumeTargetIsExecuted().First().StringProperty.Should().Be("Test String 123");
        }
    }
}