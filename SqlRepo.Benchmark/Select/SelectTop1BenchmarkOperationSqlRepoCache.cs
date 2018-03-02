using SqlRepo.Benchmark.Entities;

namespace SqlRepo.Benchmark.Select
{
    public class SelectTop1BenchmarkOperationSqlRepoCache : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ISelectCommand<BenchmarkEntity> _cachedCommand;

        public SelectTop1BenchmarkOperationSqlRepoCache(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers) : base(benchmarkHelpers, Component.SqlRepo)
        {
            _repositoryFactory = repositoryFactory;
            _cachedCommand = _repositoryFactory.Create<BenchmarkEntity>()
                .Query()
                .Top(1);
        }

        public override void Execute()
        {
            _cachedCommand.Go(ConnectionString.Value);
        }

        public override string GetNotes() => "Select TOP 1 records";
    }
}