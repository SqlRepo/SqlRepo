using NSubstitute;
using NUnit.Framework;
using SqlRepo.Benchmark.Entities;
using SqlRepo.Benchmark.Select;
using SqlRepo.Testing;
using SqlRepo.Testing.FluentAssertions;

namespace SqlRepo.Benchmark.Tests
{
    public class SelectSingleColumnBenchmarkSqlRepoShould : SqlRepoBenchmarkTestBase
    {
        [SetUp]
        public void Setup()
        {
            _selectCommand = BenchmarkEntityRepository.CreateSelectCommandStub();
        }

        private ISelectCommand<BenchmarkEntity> _selectCommand;

        public override IBenchmarkOperation Create(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers)
        {
            return new SelectSingleColumnBenchmarkSqlRepo(repositoryFactory, benchmarkHelpers);
        }

        public override string GetExpectedNotes()
        {
            return "Select the Decimal column from all records";
        }

        [Test]
        public void CreateSelectStatement()
        {
            AssumeTargetIsExecuted();
            BenchmarkEntityRepository.Received().Query();
        }

        [Test]
        public void ExecuteSqlStatement()
        {
            AssumeTargetIsExecuted();
            _selectCommand.Received().Go(ConnectionString.Value);
        }

        [Test]
        public void SelectDecimalProperty()
        {
            AssumeTargetIsExecuted();
            _selectCommand.ReceivedSelect("DecimalValue");
        }
    }
}