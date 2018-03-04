using System;
using SqlRepo.Abstractions;

namespace SqlRepo.Benchmark.Select
{
    public class SelectTop5000BenchmarkOperationSqlRepo : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public SelectTop5000BenchmarkOperationSqlRepo(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers)
            : base(benchmarkHelpers, Component.SqlRepo)
        {
            this._repositoryFactory = repositoryFactory;
        }

        public override void Execute()
        {
            var results = this._repositoryFactory.Create<BenchmarkEntity>()
                              .Query()
                              .Top(5000)
                              .Go();
        }

        public override string GetNotes()
        {
            return "Select TOP 5000 records";
        }
    }
}