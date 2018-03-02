using System;
using SqlRepo.Abstractions;

namespace SqlRepo.Benchmark.Select
{
    public class SelectTop5000BenchmarkOperationSqlRepo : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory repositoryFactory;

        public SelectTop5000BenchmarkOperationSqlRepo(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers)
            : base(benchmarkHelpers, Component.SqlRepo)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public override void Execute()
        {
            var results = this.repositoryFactory.Create<BenchmarkEntity>()
                              .Query()
                              .Top(5000)
                              .Select(e => e.Id)
                              .Select(e => e.DecimalValue)
                              .Select(e => e.TextValue)
                              .Go();
        }

        public override string GetNotes()
        {
            return "Select TOP 5000 records";
        }
    }
}