using System;
using SqlRepo.Abstractions;

namespace SqlRepo.Benchmark.Select
{
    public class SelectWhereBetweenBenchmarkOperationSqlRepo : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public SelectWhereBetweenBenchmarkOperationSqlRepo(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers)
            : base(benchmarkHelpers, Component.SqlRepo)
        {
            this._repositoryFactory = repositoryFactory;
        }

        public override void Execute()
        {
            var results = this._repositoryFactory.Create<BenchmarkEntity>()
                              .Query()
                              .WhereBetween(e => e.DecimalValue, 500, 1000)
                              .Go();
        }

        public override string GetNotes()
        {
            return "Select all records WHERE DecimalValue is between 500 and 1000";
        }
    }
}