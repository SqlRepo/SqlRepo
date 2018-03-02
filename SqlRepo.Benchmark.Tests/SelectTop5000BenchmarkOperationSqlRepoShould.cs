using System;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Benchmark.Select;
using SqlRepo.Testing.NSubstitute;

namespace SqlRepo.Benchmark.Tests
{
    public class SelectTop5000BenchmarkOperationSqlRepoShould : SqlRepoBenchmarkTestBase
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
        public void SelectTopRows()
        {
            this.AssumeTargetIsExecuted();
            this.selectCommand.Received()
                .Top(5000);
        }

        private ISelectStatement<BenchmarkEntity> selectCommand;

        public override IBenchmarkOperation Create(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers)
        {
            return new SelectTop5000BenchmarkOperationSqlRepo(repositoryFactory, benchmarkHelpers);
        }

        public override string GetExpectedNotes()
        {
            return "Select TOP 5000 records";
        }
    }
}