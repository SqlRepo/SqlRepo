using System;
using SqlRepo.Abstractions;

namespace SqlRepo.Benchmark.Select
{
    public class SelectSingleColumnBenchmarkSqlRepo : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public SelectSingleColumnBenchmarkSqlRepo(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers)
            : base(benchmarkHelpers, Component.SqlRepo)
        {
            this._repositoryFactory = repositoryFactory;
        }

        public override void Execute()
        {
            var result = this._repositoryFactory.Create<BenchmarkEntity>()
                             .Query()
                             .Select(e => e.DecimalValue)
                             .Go();
        }

        public override string GetNotes()
        {
            return "Select the Decimal column from all records";
        }
    }
}