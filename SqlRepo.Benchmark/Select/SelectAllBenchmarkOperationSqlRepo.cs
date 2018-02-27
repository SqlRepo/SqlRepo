using System.Linq;

namespace SqlRepo.Benchmark.Select
{
    public class SelectAllBenchmarkOperationSqlRepo : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public SelectAllBenchmarkOperationSqlRepo(IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers) : base(benchmarkHelpers, Component.SqlRepo)
        {
            _repositoryFactory = repositoryFactory;
        }

        public override void Execute()
        {
             var results =  _repositoryFactory.Create<BenchmarkEntity>()
                 .Query().Go(ConnectionString.Value);
        }

        public override string GetNotes()
        {
            return "Select all (50000) records";
        }
    }
}