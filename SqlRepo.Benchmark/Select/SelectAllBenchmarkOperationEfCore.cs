using System.Linq;

namespace SqlRepo.Benchmark.Select
{
    public class SelectAllBenchmarkOperationEfCore : BenchmarkOperationBase
    {
        public SelectAllBenchmarkOperationEfCore(IBenchmarkHelpers benchmarkHelpers) : base(
            benchmarkHelpers, Component.EfCore2)
        {
        }

        public override void Execute()
        {
            var dbContext = new SqlRepoBenchmarkDbContext();
            var results = dbContext.BenchmarkEntities.ToList();
        }

        public override string GetNotes()
        {
            return "Select all (50000) records";
        }
    }
}