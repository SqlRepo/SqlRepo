using System;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Benchmark.Select;
using SqlRepo.Testing.NSubstitute;

namespace SqlRepo.Benchmark.Tests
{
    public class SelectAllBenchmarkOperationSqlRepoShould : SqlRepoBenchmarkTestBase
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
    }
}