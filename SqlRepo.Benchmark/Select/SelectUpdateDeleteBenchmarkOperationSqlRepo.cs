using System;
using System.Linq;
using SqlRepo.Abstractions;

namespace SqlRepo.Benchmark.Select
{
    public class SelectUpdateDeleteBenchmarkOperationSqlRepo : BenchmarkOperationBase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public SelectUpdateDeleteBenchmarkOperationSqlRepo(IBenchmarkHelpers benchmarkHelpers,
            IRepositoryFactory repositoryFactory)
            : base(benchmarkHelpers, Component.SqlRepo)
        {
            this._repositoryFactory = repositoryFactory;
        }

        public override void Execute()
        {
            var repository = this._repositoryFactory.Create<BenchmarkEntity>();

            var benchmark = repository.Query()
                                      .Select(e => e.Id)
                                      .Where(e => e.DecimalValue == 506)
                                      .Go()
                                      .First();

            repository.Update()
                      .Set(e => e.TextValue, "NewText")
                      .Where(e => e.Id == benchmark.Id)
                      .Go();

            repository.Delete()
                      .Where(e => e.Id == benchmark.Id)
                      .Go();
        }

        public override void Setup()
        {
            this.BenchmarkHelpers.ClearRecords();
            this.BenchmarkHelpers.InsertRecords(50000);
        }
    }
}