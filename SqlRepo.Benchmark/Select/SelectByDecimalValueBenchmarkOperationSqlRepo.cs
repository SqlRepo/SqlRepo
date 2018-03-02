using System.Linq;
using SqlRepo.Benchmark.Entities;

namespace SqlRepo.Benchmark.Select
{
    public class SelectByDecimalValueBenchmarkOperationSqlRepo : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public SelectByDecimalValueBenchmarkOperationSqlRepo(IBenchmarkHelpers benchmarkHelpers,
            IRepositoryFactory repositoryFactory) : base(benchmarkHelpers,
            Component.SqlRepo)
        {
            _repositoryFactory = repositoryFactory;
        }

        public override void Execute()
        {
            var repository = _repositoryFactory.Create<BenchmarkEntity>();

            var benchmark = repository.Query().Select(e => e.Id)
                .Where(e => e.DecimalValue == 506)
                .Go(ConnectionString.Value).First();
        }
    }
}