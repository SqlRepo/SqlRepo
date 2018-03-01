using NSubstitute;
using NUnit.Framework;
using SqlRepo.Benchmark.Select;
using SqlRepo.Testing;
using SqlRepo.Testing.NSubstitute;

namespace SqlRepo.Benchmark.Tests
{
    public class SelectAllBenchmarkOperationSqlRepoShould : SqlRepoBenchmarkTestBase
    {
        [SetUp]
        public void Setup()
        {
            _selectCommand = BenchmarkEntityRepository.CreateSelectStatementSubstitute();
        }

        private ISelectStatement<BenchmarkEntity> _selectCommand;

        public override IBenchmarkOperation Create(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers)
        {
            return new SelectAllBenchmarkOperationSqlRepo(repositoryFactory, benchmarkHelpers);
        }

        public override string GetExpectedNotes()
        {
            return "Select all (50000) records";
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
    }
}