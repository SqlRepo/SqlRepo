using NSubstitute;
using NUnit.Framework;
using SqlRepo.Benchmark.Select;
using SqlRepo.Testing;
using SqlRepo.Testing.FluentAssertions;

namespace SqlRepo.Benchmark.Tests
{
    public class SelectTop5000BenchmarkOperationSqlRepoShould : SqlRepoBenchmarkTestBase
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
            return new SelectTop5000BenchmarkOperationSqlRepo(repositoryFactory, benchmarkHelpers);
        }

        public override string GetExpectedNotes()
        {
            return "Select TOP 5000 records";
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
        public void SelectTopRows()
        {
            AssumeTargetIsExecuted();
            _selectCommand.Received().Top(5000);
        }
    }
}