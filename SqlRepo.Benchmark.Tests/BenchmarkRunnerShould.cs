using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Testing.NSubstitute;

namespace SqlRepo.Benchmark.Tests
{
    [TestFixture]
    public class BenchmarkRunnerShould
    {
        [SetUp]
        public void SetUp()
        {
            this._benchmarkOperation = Substitute.For<IBenchmarkOperation>();
            this._benchmarkResultRepository = Substitute.For<IRepository<BenchmarkResult>>();
            this._insertCommand = this._benchmarkResultRepository.CreateInsertStatementSubstitute();
            this._repositoryFactory = Substitute.For<IRepositoryFactory>();
            this._repositoryFactory.Create<BenchmarkResult>()
                .Returns(this._benchmarkResultRepository);
            this._benchmarkOperations = new List<IBenchmarkOperation>();
            this._benchmarkOperations.Add(this._benchmarkOperation);

            this._benchmarkOperation.Run()
                .Returns(this._benchmarkResult);

            this._benchmarkRunner = new BenchmarkRunner(this._benchmarkOperations,
                this._repositoryFactory,
                this._benchmarkHelpers);
        }

        [Test]
        public void CreateBenchmarkResultRepository()
        {
            this.AssumeTargetIsExecuted();
            this._repositoryFactory.Received()
                .Create<BenchmarkResult>();
        }

        [Test]
        public void CreateInsertCommand()
        {
            this.AssumeTargetIsExecuted();
            this._benchmarkResultRepository.Received()
                .Insert();
        }

        [Test]
        public void ExecuteSqlStatement()
        {
            this.AssumeTargetIsExecuted();
            this._insertCommand.Received()
                .Go();
        }

        [Test]
        public void InsertComponent()
        {
            this.AssumeTargetIsExecuted();
            this._insertCommand.ReceivedWith("Component", this._benchmarkResult.Component);
        }

        [Test]
        public void InsertCreated()
        {
            this.AssumeTargetIsExecuted();
            this._insertCommand.ReceivedWith("Created", this._benchmarkResult.Created);
        }

        [Test]
        public void InserTimeTaken()
        {
            this.AssumeTargetIsExecuted();
            this._insertCommand.ReceivedWith("TimeTaken", this._benchmarkResult.TimeTaken);
        }

        [Test]
        public void InsertNotes()
        {
            this.AssumeTargetIsExecuted();
            this._insertCommand.ReceivedWith("Notes", this._benchmarkResult.Notes);
        }

        [Test]
        public void InsertTestName()
        {
            this.AssumeTargetIsExecuted();
            this._insertCommand.ReceivedWith("TestName", this._benchmarkResult.TestName);
        }

        [Test]
        public void RunBenchmarkOperation()
        {
            this.AssumeTargetIsExecuted();
            this._benchmarkOperation.Received()
                .Run();
        }

        private readonly BenchmarkResult _benchmarkResult = new BenchmarkResult
                                                            {
                                                                TestName = "TestName",
                                                                Component = "AnyCom",
                                                                Created = DateTime.UtcNow,
                                                                Notes = "Any Notes",
                                                                TimeTaken = TimeSpan
                                                                    .FromHours(55)
                                                                    .TotalMilliseconds
                                                            };

        private IBenchmarkHelpers _benchmarkHelpers;
        private IBenchmarkOperation _benchmarkOperation;
        private List<IBenchmarkOperation> _benchmarkOperations;
        private IRepository<BenchmarkResult> _benchmarkResultRepository;

        private IBenchmarkRunner _benchmarkRunner;
        private IInsertStatement<BenchmarkResult> _insertCommand;
        private IRepositoryFactory _repositoryFactory;

        private void AssumeTargetIsExecuted()
        {
            this._benchmarkRunner.Run();
        }
    }
}