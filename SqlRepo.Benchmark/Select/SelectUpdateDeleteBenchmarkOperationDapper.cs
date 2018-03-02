using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SqlRepo.Benchmark.Entities;

namespace SqlRepo.Benchmark.Select
{
    public class SelectUpdateDeleteBenchmarkOperationDapper : BenchmarkOperationBase
    {
        public SelectUpdateDeleteBenchmarkOperationDapper(IBenchmarkHelpers benchmarkHelpers) : base(benchmarkHelpers,
            Component.Dapper)
        {
        }

        public override void Setup()
        {
            BenchmarkHelpers.ClearRecords();
            BenchmarkHelpers.InsertRecords(50000);
        }

        public override void Execute()
        {
            using (var conn = new SqlConnection(ConnectionString.Value))
            {
                decimal decimalValueToUse = 504;

                conn.Open();
                var result = conn
                    .Query<BenchmarkEntity>("SELECT Id FROM BenchmarkEntity WHERE DecimalValue = " + decimalValueToUse)
                    .First();

                conn.Query("UPDATE BenchmarkEntity SET TextValue = 'NewText' WHERE Id = " + result.Id);

                conn.Query("DELETE FROM BenchmarkEntity WHERE Id = " + result.Id);
            }
        }
    }
}