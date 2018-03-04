using System;
using SqlRepo.Abstractions;

namespace SqlRepo.Benchmark.Select
{
    public class SelectTop1BenchmarkOperationSqlRepo : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public SelectTop1BenchmarkOperationSqlRepo(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers)
            : base(benchmarkHelpers, Component.SqlRepo)
        {
            this._repositoryFactory = repositoryFactory;
        }

        public override void Execute()
        {
            this._repositoryFactory.Create<BenchmarkEntity>()
                .Query()
                .Top(1)
                .Go();
        }

        public override string GetNotes()
        {
            return "Select TOP 1 records";
        }
    }
}