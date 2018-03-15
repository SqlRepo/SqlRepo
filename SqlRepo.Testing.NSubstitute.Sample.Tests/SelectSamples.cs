using System;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;

namespace SqlRepo.Testing.NSubstitute.Sample.Tests
{
    [TestFixture]
    public class SelectSamples
    {
        [SetUp]
        public void SetUp()
        {
            this.target = StatementSubstituteFactory.CreateSelectStatementSubstitute<TestEntity>();
            this.builder = new SampleSelectBuilder(this.target);
        }

        [Test]
        public void SelectAllSample()
        {
            this.builder.ExecuteSimpleSelectAll();
            this.target.Received()
                .SelectAll();
            this.target.Received()
                .Go();
        }

        [Test]
        public void SelectAllAsyncSample()
        {
            this.builder.ExecuteSimpleSelectAllAsync();
            this.target.Received()
                .SelectAll();
            this.target.Received()
                .GoAsync();
        }

        [Test]
        public void SelectSingleMemberSample()
        {
            this.builder.BuildSingleMemberSelect();
            this.target.ReceivedSelect(e => e.StringProperty);
        }

        private ISelectStatement<TestEntity> target;
        private SampleSelectBuilder builder;
    }
}