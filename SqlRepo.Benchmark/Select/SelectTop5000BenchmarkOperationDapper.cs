using System.Data.SqlClient;
using Dapper;
using SqlRepo.Benchmark.Entities;

namespace SqlRepo.Benchmark.Select
{
    public class SelectTop5000BenchmarkOperationDapper : BenchmarkOperationBase
    {
        public SelectTop5000BenchmarkOperationDapper(IBenchmarkHelpers benchmarkHelpers) : base(benchmarkHelpers,
            Component.Dapper)
        {
        }

        public override void Execute()
        {
            using (var conn = new SqlConnection(ConnectionString.Value))
            {
                conn.Open();
                var result = conn.Query<BenchmarkEntity>("SELECT TOP(5000) * FROM BenchmarkEntity");
            }
        }

        public override string GetNotes()
        {
            return "Select TOP 5000 records";
        }
    }
}