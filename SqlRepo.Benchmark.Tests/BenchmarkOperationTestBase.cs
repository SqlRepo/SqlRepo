using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace SqlRepo.Benchmark.Tests
{
    [TestFixture]
    public abstract class BenchmarkOperationTestBase
    {
        [SetUp]
        public void SetupTests()
        {
            BenchmarkEntityRepository = Substitute.For<IRepository<BenchmarkEntity>>();
            RepositoryFactory = Substitute.For<IRepositoryFactory>();
            RepositoryFactory.Create<BenchmarkEntity>().Returns(BenchmarkEntityRepository);
            BenchmarkHelpers = Substitute.For<IBenchmarkHelpers>();
            BenchmarkOperation = Create(RepositoryFactory, BenchmarkHelpers);
        }

        public IRepositoryFactory RepositoryFactory { get; private set; }
        public IRepository<BenchmarkEntity> BenchmarkEntityRepository { get; private set; }
        public IBenchmarkHelpers BenchmarkHelpers { get; private set; }

        public abstract IBenchmarkOperation Create(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers);

        public abstract string GetExpectedNotes();

        public IBenchmarkOperation BenchmarkOperation { get; private set; }

        protected BenchmarkResult AssumeTargetIsExecuted()
        {
            return BenchmarkOperation.Run();
        }

        [Test]
        public void CreateBenchmarkEntityRepository()
        {
            this.AssumeTargetIsExecuted();
            this.RepositoryFactory.Received().Create<BenchmarkEntity>();
        }

        [Test]
        public void ReturnCorrectNotes()
        {
            this.AssumeTargetIsExecuted().Notes.Should().Be(GetExpectedNotes());
        }
    }
}