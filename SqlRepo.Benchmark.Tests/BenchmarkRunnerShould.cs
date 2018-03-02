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
            this.benchmarkOperation = Substitute.For<IBenchmarkOperation>();
            this.benchmarkResultRepository = Substitute.For<IRepository<BenchmarkResult>>();
            this.insertCommand = this.benchmarkResultRepository.CreateInsertStatementSubstitute();
            this.repositoryFactory = Substitute.For<IRepositoryFactory>();
            this.repositoryFactory.Create<BenchmarkResult>()
                .Returns(this.benchmarkResultRepository);
            this.benchmarkOperations = new List<IBenchmarkOperation>();
            this.benchmarkOperations.Add(this.benchmarkOperation);

            this.benchmarkOperation.Run()
                .Returns(this.benchmarkResult);

            this.benchmarkRunner = new BenchmarkRunner(this.benchmarkOperations, this.repositoryFactory);
        }

        [Test]
        public void CreateBenchmarkResultRepository()
        {
            this.AssumeTargetIsExecuted();
            this.repositoryFactory.Received()
                .Create<BenchmarkResult>();
        }

        [Test]
        public void CreateInsertCommand()
        {
            this.AssumeTargetIsExecuted();
            this.benchmarkResultRepository.Received()
                .Insert();
        }

        [Test]
        public void ExecuteSqlStatement()
        {
            this.AssumeTargetIsExecuted();
            this.insertCommand.Received()
                .Go();
        }

        [Test]
        public void InsertComponent()
        {
            this.AssumeTargetIsExecuted();
            this.insertCommand.ReceivedWith("Component", this.benchmarkResult.Component);
        }

        [Test]
        public void InsertCreated()
        {
            this.AssumeTargetIsExecuted();
            this.insertCommand.ReceivedWith("Created", this.benchmarkResult.Created);
        }

        [Test]
        public void InserTimeTaken()
        {
            this.AssumeTargetIsExecuted();
            this.insertCommand.ReceivedWith("TimeTaken", this.benchmarkResult.TimeTaken);
        }

        [Test]
        public void InsertNotes()
        {
            this.AssumeTargetIsExecuted();
            this.insertCommand.ReceivedWith("Notes", this.benchmarkResult.Notes);
        }

        [Test]
        public void InsertTestName()
        {
            this.AssumeTargetIsExecuted();
            this.insertCommand.ReceivedWith("TestName", this.benchmarkResult.TestName);
        }

        [Test]
        public void RunBenchmarkOperation()
        {
            this.AssumeTargetIsExecuted();
            this.benchmarkOperation.Received()
                .Run();
        }

        private IBenchmarkOperation benchmarkOperation;
        private List<IBenchmarkOperation> benchmarkOperations;

        private readonly BenchmarkResult benchmarkResult = new BenchmarkResult
                                                            {
                                                                TestName = "TestName",
                                                                Component = "AnyCom",
                                                                Created = DateTime.UtcNow,
                                                                Notes = "Any Notes",
                                                                TimeTaken = TimeSpan
                                                                            .FromHours(55)
                                                                            .TotalMilliseconds
                                                            };
        private IRepository<BenchmarkResult> benchmarkResultRepository;

        private IBenchmarkRunner benchmarkRunner;
        private IInsertStatement<BenchmarkResult> insertCommand;
        private IRepositoryFactory repositoryFactory;

        private void AssumeTargetIsExecuted()
        {
            this.benchmarkRunner.Run();
        }
    }
}