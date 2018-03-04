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
            this._selectCommand = this.BenchmarkEntityRepository.CreateSelectStatementSubstitute();
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
            this._selectCommand.Received()
                .Go();
        }

        [Test]
        public void SelectDecimalProperty()
        {
            this.AssumeTargetIsExecuted();
            this._selectCommand.ReceivedSelect("DecimalValue");
        }

        [Test]
        public void SelectIdProperty()
        {
            this.AssumeTargetIsExecuted();
            this._selectCommand.ReceivedSelect("Id");
        }

        [Test]
        public void SelectTopRows()
        {
            this.AssumeTargetIsExecuted();
            this._selectCommand.Received()
                .Top(5000);
        }

        private ISelectStatement<BenchmarkEntity> _selectCommand;

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