using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SqlRepo.Benchmark.Entities;

namespace SqlRepo.Benchmark.Select
{
    public class SelectByDecimalValueBenchmarkOperationDapper : BenchmarkOperationBase
    {
        public SelectByDecimalValueBenchmarkOperationDapper(IBenchmarkHelpers benchmarkHelpers) : base(benchmarkHelpers,
            Component.Dapper)
        {
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
            }
        }
    }
}