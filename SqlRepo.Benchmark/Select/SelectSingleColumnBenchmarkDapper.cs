using System.Data.SqlClient;
using Dapper;
using SqlRepo.Benchmark.Entities;

namespace SqlRepo.Benchmark.Select
{
    public class SelectSingleColumnBenchmarkDapper : BenchmarkOperationBase
    {
        public SelectSingleColumnBenchmarkDapper(IBenchmarkHelpers benchmarkHelpers) : base(benchmarkHelpers,
            Component.Dapper)
        {
        }

        public override void Execute()
        {
            using (var conn = new SqlConnection(ConnectionString.Value))
            {
                conn.Open();
                var result = conn.Query<BenchmarkEntity>("SELECT DecimalValue FROM BenchmarkEntity");
            }
        }

        public override string GetNotes()
        {
            return "Select the Decimal column from all records";
        }
    }
}