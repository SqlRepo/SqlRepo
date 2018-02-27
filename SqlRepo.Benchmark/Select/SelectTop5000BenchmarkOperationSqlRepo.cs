namespace SqlRepo.Benchmark.Select
{
    public class SelectTop5000BenchmarkOperationSqlRepo : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public SelectTop5000BenchmarkOperationSqlRepo(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers) : base(benchmarkHelpers, Component.SqlRepo)
        {
            _repositoryFactory = repositoryFactory;
        }

        public override void Execute()
        {
            var results = _repositoryFactory.Create<BenchmarkEntity>()
                .Query()
                .Top(5000).Select(e => e.Id)
                .Select(e => e.DecimalValue)
                .Select(e => e.TextValue).Go(ConnectionString.Value);
        }

        public override string GetNotes() => "Select TOP 5000 records";
    }
}