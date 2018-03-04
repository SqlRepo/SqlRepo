using System;
using SqlRepo.Abstractions;

namespace SqlRepo.Benchmark.Select
{
    public class SelectTop1BenchmarkOperationSqlRepoCache : BenchmarkOperationBase
    {
        private readonly ISelectStatement<BenchmarkEntity> _cachedCommand;
        private readonly IRepositoryFactory _repositoryFactory;

        public SelectTop1BenchmarkOperationSqlRepoCache(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers)
            : base(benchmarkHelpers, Component.SqlRepo)
        {
            this._repositoryFactory = repositoryFactory;
            this._cachedCommand = this._repositoryFactory.Create<BenchmarkEntity>()
                                      .Query()
                                      .Top(1);
        }

        public override void Execute()
        {
            this._cachedCommand.Go();
        }

        public override string GetNotes()
        {
            return "Select TOP 1 records";
        }
    }
}