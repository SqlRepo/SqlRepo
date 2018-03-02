using System.Data.SqlClient;
using Dapper;
using SqlRepo.Benchmark.Entities;

namespace SqlRepo.Benchmark.Select
{
    public class SelectWhereBetweenBenchmarkOperationDapper : BenchmarkOperationBase
    {
        public SelectWhereBetweenBenchmarkOperationDapper(IBenchmarkHelpers benchmarkHelpers) : base(benchmarkHelpers,
            Component.Dapper)
        {
        }

        public override void Execute()
        {
            using (var conn = new SqlConnection(ConnectionString.Value))
            {
                conn.Open();
                var result =
                    conn.Query<BenchmarkEntity>(
                        "SELECT * FROM BenchmarkEntity WHERE DecimalValue BETWEEN 500 AND 1000");
            }
        }

        public override string GetNotes()
        {
            return "Select all records WHERE DecimalValue is between 500 and 1000";
        }
    }
}