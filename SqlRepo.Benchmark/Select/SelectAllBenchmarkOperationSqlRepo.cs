using System;
using SqlRepo.Abstractions;

namespace SqlRepo.Benchmark.Select
{
    public class SelectAllBenchmarkOperationSqlRepo : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory repositoryFactory;

        public SelectAllBenchmarkOperationSqlRepo(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers)
            : base(benchmarkHelpers, Component.SqlRepo)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public override void Execute()
        {
            var results = this.repositoryFactory.Create<BenchmarkEntity>()
                              .Query()
                              .Go();
        }

        public override string GetNotes()
        {
            return "Select all (50000) records";
        }
    }
}