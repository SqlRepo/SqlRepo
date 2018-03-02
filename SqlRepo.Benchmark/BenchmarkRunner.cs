using System.Collections.Generic;
using System.Linq;

namespace SqlRepo.Benchmark
{
    public class BenchmarkRunner : IBenchmarkRunner
    {
        private readonly IEnumerable<IBenchmarkOperation> _benchmarkOperations;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IBenchmarkHelpers _benchmarkHelpers;

        public BenchmarkRunner(IEnumerable<IBenchmarkOperation> benchmarkOperations,
            IRepositoryFactory repositoryFactory, IBenchmarkHelpers benchmarkHelpers)
        {
            _benchmarkOperations = benchmarkOperations;
            _repositoryFactory = repositoryFactory;
            _benchmarkHelpers = benchmarkHelpers;
        }

        public void Run()
        {
            for (var j = 0; j < 10; j++)
            {
                foreach (var benchmarkOperation in _benchmarkOperations)
                {
                    _benchmarkHelpers.ClearRecords();
                    _benchmarkHelpers.InsertRecords(50000);

                    for (var i = 0; i < 15; i++)
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
    }
}