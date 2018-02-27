using System;

namespace SqlRepo.Benchmark
{
    public class BenchmarkResult
    {
        public BenchmarkResult()
        {
            Created = DateTime.UtcNow;    
        }

        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string TestName { get; set; }
        public string Notes { get; set; }
        public double TimeTaken { get; set; }
        public string Component { get; set; }
    }
}