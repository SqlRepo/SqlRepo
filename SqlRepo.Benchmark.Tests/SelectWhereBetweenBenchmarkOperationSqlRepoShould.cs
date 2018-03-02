using NSubstitute;
using NUnit.Framework;
using SqlRepo.Benchmark.Entities;
using SqlRepo.Benchmark.Select;
using SqlRepo.Testing;
using SqlRepo.Testing.FluentAssertions;

namespace SqlRepo.Benchmark.Tests
{
    public class SelectWhereBetweenBenchmarkOperationSqlRepoShould : SqlRepoBenchmarkTestBase
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
            return new SelectWhereBetweenBenchmarkOperationSqlRepo(repositoryFactory, benchmarkHelpers);
        }

        public override string GetExpectedNotes()
        {
            return "Select all records WHERE DecimalValue is between 500 and 1000";
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
        public void SelectIdProperty()
        {
            AssumeTargetIsExecuted();
            _selectCommand.ReceivedSelect("Id");
        }

        [Test]
        public void SelectDecimalProperty()
        {
            AssumeTargetIsExecuted();
            _selectCommand.ReceivedSelect("DecimalValue");
        }

        [Test]
        public void SelectWhereBetween()
        {
            AssumeTargetIsExecuted();
            _selectCommand.ReceivedWhereBetween("DecimalValue", 500m, 1000m, null);
        }
    }
}