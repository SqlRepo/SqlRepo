using System.Data.SqlClient;
using Dapper;

namespace SqlRepo.Benchmark.Select
{
    public class SelectAllBenchmarkOperationDapper : BenchmarkOperationBase
    {
        public SelectAllBenchmarkOperationDapper(IBenchmarkHelpers benchmarkHelpers) : base(
            benchmarkHelpers, Component.Dapper)
        {
        }

        public override void Execute()
        {
            using (var conn = new SqlConnection(ConnectionString.Value))
            {
                conn.Open();
                var result = conn.Query<BenchmarkEntity>("SELECT * FROM BenchmarkEntity");
            }
        }

        public override string GetNotes()
        {
            return "Select all (50000) records";
        }
    }
}