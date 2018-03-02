using System;
using System.Linq;

namespace SqlRepo.Benchmark.Select
{
    public class SelectTop5000BenchmarkOperationEfCore : BenchmarkOperationBase
    {
        public SelectTop5000BenchmarkOperationEfCore(IBenchmarkHelpers benchmarkHelpers)
            : base(benchmarkHelpers, Component.EfCore2) { }

        public override void Execute()
        {
            var dbContext = new SqlRepoBenchmarkDbContext();
            var results = dbContext.BenchmarkEntities.Take(5000)
                                   .ToList();
        }

        public override string GetNotes()
        {
            return "Select TOP 5000 records";
        }
    }
}