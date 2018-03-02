using System.Data.SqlClient;
using Dapper;
using SqlRepo.Benchmark.Entities;

namespace SqlRepo.Benchmark.Select
{
    public class SelectTop1BenchmarkOperationDapper : BenchmarkOperationBase
    {
        public SelectTop1BenchmarkOperationDapper(IBenchmarkHelpers benchmarkHelpers) : base(benchmarkHelpers,
            Component.Dapper)
        {
        }

        public override void Execute()
        {
            using (var conn = new SqlConnection(ConnectionString.Value))
            {
                conn.Open();
                var result = conn.QuerySingleOrDefault<BenchmarkEntity>("SELECT TOP(1) * FROM BenchmarkEntity");
            }
        }

        public override string GetNotes()
        {
            return "Select TOP 1 records";
        }
    }
}