using System;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Benchmark.Select;
using SqlRepo.Testing.NSubstitute;

namespace SqlRepo.Benchmark.Tests
{
    public class SelectWhereBetweenBenchmarkOperationSqlRepoShould : SqlRepoBenchmarkTestBase
    {
        [SetUp]
        public void Setup()
        {
            this.selectCommand = this.BenchmarkEntityRepository.CreateSelectStatementSubstitute();
        }

        [Test]
        public void CreateSelectStatement()
        {
            this.AssumeTargetIsExecuted();
            this.BenchmarkEntityRepository.Received()
                .Query();
        }

        [Test]
        public void ExecuteSqlStatement()
        {
            this.AssumeTargetIsExecuted();
            this.selectCommand.Received()
                .Go();
        }

        [Test]
        public void SelectIdProperty()
        {
            this.AssumeTargetIsExecuted();
            this.selectCommand.ReceivedSelect("Id");
        }

        [Test]
        public void SelectDecimalProperty()
        {
            this.AssumeTargetIsExecuted();
            this.selectCommand.ReceivedSelect("DecimalValue");
        }

        [Test]
        public void SelectWhereBetween()
        {
            this.AssumeTargetIsExecuted();
            this.selectCommand.ReceivedWhereBetween("DecimalValue", 500m, 1000m, null);
        }

        private ISelectStatement<BenchmarkEntity> selectCommand;

        public override IBenchmarkOperation Create(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers)
        {
            return new SelectWhereBetweenBenchmarkOperationSqlRepo(repositoryFactory, benchmarkHelpers);
        }

        public override string GetExpectedNotes()
        {
            return "Select all records WHERE DecimalValue is between 500 and 1000";
        }
    }
}