using SqlRepo.Benchmark.Entities;

namespace SqlRepo.Benchmark.Select
{
    public class SelectWhereBetweenBenchmarkOperationSqlRepo : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public SelectWhereBetweenBenchmarkOperationSqlRepo(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers) : base(benchmarkHelpers, Component.SqlRepo)
        {
            _repositoryFactory = repositoryFactory;
        }

        public override void Execute()
        {
            var results = _repositoryFactory.Create<BenchmarkEntity>()
                .Query()
                .WhereBetween(e => e.DecimalValue, 500, 1000).Go(ConnectionString.Value);
        }

        public override string GetNotes() => "Select all records WHERE DecimalValue is between 500 and 1000";
    }
}