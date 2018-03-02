using System;

namespace SqlRepo.Benchmark
{
    public class BenchmarkResult
    {
        public BenchmarkResult()
        {
            this.Created = DateTime.UtcNow;
        }

        public string Component { get; set; }
        public DateTime Created { get; set; }
        public int Id { get; set; }
        public string Notes { get; set; }
        public string TestName { get; set; }
        public double TimeTaken { get; set; }
    }
}