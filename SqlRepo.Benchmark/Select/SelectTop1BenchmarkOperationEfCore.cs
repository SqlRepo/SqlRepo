using System.Linq;

namespace SqlRepo.Benchmark.Select
{
    public class SelectTop1BenchmarkOperationEfCore : BenchmarkOperationBase
    {
        public override void Execute()
        {
            var dbContext = new SqlRepoBenchmarkDbContext();
            var results = dbContext.BenchmarkEntities.Take(1).ToList();
        }

        public override string GetNotes() => "Select TOP 1 records";

        public SelectTop1BenchmarkOperationEfCore(IBenchmarkHelpers benchmarkHelpers) : base(benchmarkHelpers,
            Component.EfCore2)
        {
        }
    }
}