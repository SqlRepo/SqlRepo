using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing.NSubstitute;

namespace SqlRepo.Benchmark.Tests
{
    [TestFixture]
    public class BenchmarkRunnerShould
    {
        [SetUp]
        public void SetUp()
        {
            _benchmarkOperation = Substitute.For<IBenchmarkOperation>();
            _benchmarkResultRepository = Substitute.For<IRepository<BenchmarkResult>>();
            _insertCommand = _benchmarkResultRepository.CreateInsertCommandSubstitute();
            _repositoryFactory = Substitute.For<IRepositoryFactory>();
            _repositoryFactory.Create<BenchmarkResult>().Returns(_benchmarkResultRepository);
            _benchmarkOperations = new List<IBenchmarkOperation>();
            _benchmarkOperations.Add(_benchmarkOperation);

            _benchmarkOperation.Run().Returns(_benchmarkResult);

            _benchmarkRunner = new BenchmarkRunner(_benchmarkOperations, _repositoryFactory);
        }

        private IBenchmarkRunner _benchmarkRunner;
        private IRepositoryFactory _repositoryFactory;
        private List<IBenchmarkOperation> _benchmarkOperations;
        private IBenchmarkOperation _benchmarkOperation;
        private IRepository<BenchmarkResult> _benchmarkResultRepository;
        private IInsertCommand<BenchmarkResult> _insertCommand;

        private readonly BenchmarkResult _benchmarkResult = new BenchmarkResult
        {
            TestName = "TestName",
            Component = "AnyCom",
            Created = DateTime.UtcNow,
            Notes = "Any Notes",
            TimeTaken = TimeSpan.FromHours(55).TotalMilliseconds
        };

        private void AssumeTargetIsExecuted()
        {
            _benchmarkRunner.Run();
        }

        [Test]
        public void CreateBenchmarkResultRepository()
        {
            AssumeTargetIsExecuted();
            _repositoryFactory.Received().Create<BenchmarkResult>();
        }

        [Test]
        public void CreateInsertCommand()
        {
            AssumeTargetIsExecuted();
            _benchmarkResultRepository.Received().Insert();
        }

        [Test]
        public void ExecuteSqlStatement()
        {
            AssumeTargetIsExecuted();
            _insertCommand.Received().Go(ConnectionString.Value);
        }

        [Test]
        public void InsertComponent()
        {
            AssumeTargetIsExecuted();
            _insertCommand.ReceivedWith("Component", _benchmarkResult.Component);
        }

        [Test]
        public void InsertCreated()
        {
            AssumeTargetIsExecuted();
            _insertCommand.ReceivedWith("Created", _benchmarkResult.Created);
        }

        [Test]
        public void InserTimeTaken()
        {
            AssumeTargetIsExecuted();
            _insertCommand.ReceivedWith("TimeTaken", _benchmarkResult.TimeTaken);
        }

        [Test]
        public void InsertNotes()
        {
            AssumeTargetIsExecuted();
            _insertCommand.ReceivedWith("Notes", _benchmarkResult.Notes);
        }

        [Test]
        public void InsertTestName()
        {
            AssumeTargetIsExecuted();
            _insertCommand.ReceivedWith("TestName", _benchmarkResult.TestName);
        }

        [Test]
        public void RunBenchmarkOperation()
        {
            AssumeTargetIsExecuted();
            _benchmarkOperation.Received().Run();
        }
    }
}