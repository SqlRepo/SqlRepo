using System.Linq;
using SqlRepo.Benchmark.Entities;

namespace SqlRepo.Benchmark.Select
{
    public class SelectUpdateDeleteBenchmarkOperationSqlRepo : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public SelectUpdateDeleteBenchmarkOperationSqlRepo(IBenchmarkHelpers benchmarkHelpers,
            IRepositoryFactory repositoryFactory) : base(benchmarkHelpers,
            Component.SqlRepo)
        {
            _repositoryFactory = repositoryFactory;
        }

        public override void Setup()
        {
            BenchmarkHelpers.ClearRecords();
            BenchmarkHelpers.InsertRecords(50000);
        }

        public override void Execute()
        {
            var repository = _repositoryFactory.Create<BenchmarkEntity>();

            var benchmark = repository.Query().Select(e => e.Id)
                .Where(e => e.DecimalValue == 506)
                .Go(ConnectionString.Value).First();

            repository.Update().Set(e => e.TextValue, "NewText").Where(e => e.Id == benchmark.Id)
                .Go(ConnectionString.Value);

            repository.Delete().Where(e => e.Id == benchmark.Id).Go(ConnectionString.Value);
        }
    }
}