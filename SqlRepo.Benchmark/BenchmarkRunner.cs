using System;
using System.Collections.Generic;
using SqlRepo.Abstractions;

namespace SqlRepo.Benchmark
{
    public class BenchmarkRunner : IBenchmarkRunner
    {
        private readonly IBenchmarkHelpers _benchmarkHelpers;
        private readonly IEnumerable<IBenchmarkOperation> _benchmarkOperations;
        private readonly IRepositoryFactory _repositoryFactory;

        public BenchmarkRunner(IEnumerable<IBenchmarkOperation> benchmarkOperations,
            IRepositoryFactory repositoryFactory,
            IBenchmarkHelpers benchmarkHelpers)
        {
            this._benchmarkOperations = benchmarkOperations;
            this._repositoryFactory = repositoryFactory;
            this._benchmarkHelpers = benchmarkHelpers;
        }

        public void Run()
        {
            for(var j = 0; j < 10; j++)
            {
                foreach(var benchmarkOperation in this._benchmarkOperations)
                {
                    this._benchmarkHelpers.ClearRecords();
                    this._benchmarkHelpers.InsertRecords(250000);

                    for(var i = 0; i < 15; i++)
                    {
                        var benchmarkResult = benchmarkOperation.Run();

                        this._repositoryFactory.Create<BenchmarkResult>()
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
}