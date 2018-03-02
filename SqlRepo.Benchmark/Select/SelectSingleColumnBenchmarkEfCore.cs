using System.Linq;
using SqlRepo.Benchmark.Entities;

namespace SqlRepo.Benchmark.Select
{
    public class SelectSingleColumnBenchmarkEfCore : BenchmarkOperationBase
    {
        public SelectSingleColumnBenchmarkEfCore(IBenchmarkHelpers benchmarkHelpers) : base(benchmarkHelpers,
            Component.EfCore2)
        {
        }

        public override void Execute()
        {
            var dbContext = new SqlRepoBenchmarkDbContext();
            var result = from r in dbContext.BenchmarkEntities.OrderBy(e => e.DecimalValue)
                select new
                {
                    r.DecimalValue
                };

            var results = result.Select(e => new BenchmarkEntity {DecimalValue = e.DecimalValue}).ToList();
        }

        public override string GetNotes()
        {
            return "Select the Decimal column from all records";
        }
    }
}