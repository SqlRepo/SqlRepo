using System;
using System.Collections.Generic;
using SqlRepo.Abstractions;

namespace SqlRepo.Benchmark
{
    public class BenchmarkRunner : IBenchmarkRunner
    {
        private readonly IEnumerable<IBenchmarkOperation> benchmarkOperations;
        private readonly IRepositoryFactory repositoryFactory;

        public BenchmarkRunner(IEnumerable<IBenchmarkOperation> benchmarkOperations,
            IRepositoryFactory repositoryFactory)
        {
            this.benchmarkOperations = benchmarkOperations;
            this.repositoryFactory = repositoryFactory;
        }

        public void Run()
        {
            for(var i = 0; i < 6; i++)
            {
                foreach(var benchmarkOperation in this.benchmarkOperations)
                {
                    var benchmarkResult = benchmarkOperation.Run();

                    this.repositoryFactory.Create<BenchmarkResult>()
                        .Insert()
                        .With(e => e.TestName, benchmarkResult.TestName)
                        .With(e => e.Created, benchmarkResult.Created)
                        .With(e => e.TimeTaken, benchmarkResult.TimeTaken)
                        .With(e => e.Notes, benchmarkResult.Notes)
                        .With(e => e.Component, benchmarkResult.Component)
                        .Go();
                }
            }
        }
    }
}