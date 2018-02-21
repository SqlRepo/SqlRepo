using System.Linq;

namespace SqlRepo.Benchmark.Select
{
    public class SelectWhereBetweenBenchmarkOperationEfCore : BenchmarkOperationBase
    {
        public override void Execute()
        {
            var dbContext = new SqlRepoBenchmarkDbContext();
            var results = dbContext.BenchmarkEntities.Where(e => e.DecimalValue > 500 && e.DecimalValue < 1000)
                .ToList();
        }

        public override string GetNotes() => "Select all records WHERE DecimalValue is between 500 and 1000";

        public SelectWhereBetweenBenchmarkOperationEfCore(IBenchmarkHelpers benchmarkHelpers) : base(benchmarkHelpers,
            Component.EfCore2)
        {
        }
    }
}