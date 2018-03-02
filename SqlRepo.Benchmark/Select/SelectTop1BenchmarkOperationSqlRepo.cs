using SqlRepo.Benchmark.Entities;

namespace SqlRepo.Benchmark.Select
{
    public class SelectTop1BenchmarkOperationSqlRepo : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public SelectTop1BenchmarkOperationSqlRepo(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers) : base(benchmarkHelpers, Component.SqlRepo)
        {
            _repositoryFactory = repositoryFactory;
        }

        public override void Execute()
        {
            _repositoryFactory.Create<BenchmarkEntity>()
                .Query()
                .Top(1)
                .Go(ConnectionString.Value);
        }

        public override string GetNotes() => "Select TOP 1 records";
    }
}