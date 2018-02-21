using System.Collections.Generic;

namespace SqlRepo.Benchmark
{
    public class BenchmarkRunner : IBenchmarkRunner
    {
        private readonly IEnumerable<IBenchmarkOperation> _benchmarkOperations;
        private readonly IRepositoryFactory _repositoryFactory;

        public BenchmarkRunner(IEnumerable<IBenchmarkOperation> benchmarkOperations,
            IRepositoryFactory repositoryFactory)
        {
            _benchmarkOperations = benchmarkOperations;
            _repositoryFactory = repositoryFactory;
        }

        public void Run()
        {
            for (var i = 0; i < 10; i++)
                foreach (var benchmarkOperation in _benchmarkOperations)
                {
                    var benchmarkResult = benchmarkOperation.Run();

                    _repositoryFactory.Create<BenchmarkResult>().Insert()
                        .With(e => e.TestName, benchmarkResult.TestName)
                        .With(e => e.Created, benchmarkResult.Created)
                        .With(e => e.TimeTaken, benchmarkResult.TimeTaken)
                        .With(e => e.Notes, benchmarkResult.Notes)
                        .With(e => e.Component, benchmarkResult.Component)
                        .Go(ConnectionString.Value);
                }
        }
    }
}