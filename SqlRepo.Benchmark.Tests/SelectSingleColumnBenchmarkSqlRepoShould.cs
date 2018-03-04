using System;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Benchmark.Select;
using SqlRepo.Testing.NSubstitute;

namespace SqlRepo.Benchmark.Tests
{
    public class SelectSingleColumnBenchmarkSqlRepoShould : SqlRepoBenchmarkTestBase
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

        private ISelectStatement<BenchmarkEntity> _selectCommand;

        public override IBenchmarkOperation Create(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers)
        {
            return new SelectSingleColumnBenchmarkSqlRepo(repositoryFactory, benchmarkHelpers);
        }

        public override string GetExpectedNotes()
        {
            return "Select the Decimal column from all records";
        }
    }
}