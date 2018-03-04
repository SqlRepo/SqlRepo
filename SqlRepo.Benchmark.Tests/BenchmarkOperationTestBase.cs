using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;

namespace SqlRepo.Benchmark.Tests
{
    [TestFixture]
    public abstract class BenchmarkOperationTestBase
    {
        [SetUp]
        public void SetupTests()
        {
            this.BenchmarkEntityRepository = Substitute.For<IRepository<BenchmarkEntity>>();
            this.RepositoryFactory = Substitute.For<IRepositoryFactory>();
            this.RepositoryFactory.Create<BenchmarkEntity>()
                .Returns(this.BenchmarkEntityRepository);
            this.BenchmarkHelpers = Substitute.For<IBenchmarkHelpers>();
            this.BenchmarkOperation = this.Create(this.RepositoryFactory, this.BenchmarkHelpers);
        }

        [Test]
        public void CreateBenchmarkEntityRepository()
        {
            this.AssumeTargetIsExecuted();
            this.RepositoryFactory.Received()
                .Create<BenchmarkEntity>();
        }

        [Test]
        public void ReturnCorrectNotes()
        {
            this.AssumeTargetIsExecuted()
                .Notes.Should()
                .Be(this.GetExpectedNotes());
        }

        public IRepository<BenchmarkEntity> BenchmarkEntityRepository { get; private set; }
        public IBenchmarkHelpers BenchmarkHelpers { get; private set; }

        public IBenchmarkOperation BenchmarkOperation { get; private set; }

        public IRepositoryFactory RepositoryFactory { get; private set; }

        public abstract IBenchmarkOperation Create(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers);

        public abstract string GetExpectedNotes();

        protected BenchmarkResult AssumeTargetIsExecuted()
        {
            return this.BenchmarkOperation.Run();
        }
    }
}