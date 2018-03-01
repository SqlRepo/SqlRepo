using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public abstract class RepositoryTestBase : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            AssumeTestEntityIsInitialised();
            AssumeStatementFactoryIsInitialised();
            repository = new Repository<TestEntity>(this.StatementFactory);
        }

        private IRepository<TestEntity> repository;

        protected void AssumeDeleteEntityIsRequested()
        {
            repository.Delete(Entity);
        }

        protected IDeleteStatement<TestEntity> AssumeDeleteIsRequested()
        {
            return repository.Delete();
        }

        protected TestEntity AssumeInsertEntityIsRequested()
        {
            return repository.Insert(Entity);
        }

        protected IInsertStatement<TestEntity> AssumeInsertIsRequested()
        {
            return repository.Insert();
        }

        protected IEnumerable<TestEntity> AssumeResultsFromIsRequested(ISelectStatement<TestEntity> query)
        {
            return repository.ResultsFrom(query);
        }

        protected ISelectStatement<TestEntity> AssumeSelectIsRequested()
        {
            return repository.Query();
        }

        protected void AssumeUpdateEntityIsRequested()
        {
            repository.Update(Entity);
        }

        protected IUpdateStatement<TestEntity> AssumeUpdateIsRequested()
        {
            return repository.Update();
        }

        private void AssumeStatementFactoryIsInitialised()
        {
            this.StatementFactory = Substitute.For<IStatementFactory>();
            AssumeDeleteCommandIsInitialised();
            AssumeInsertCommandIsInitialised();
            AssumeUpdateCommandIsInitialised();
            AssumeSelectStatementIsInitialised();
        }

        private void AssumeDeleteCommandIsInitialised()
        {
            this.DeleteStatement = Substitute.For<IDeleteStatement<TestEntity>>();
            this.StatementFactory.CreateDelete<TestEntity>()
                .Returns(this.DeleteStatement);
            this.DeleteStatement.For(Entity)
                .Returns(this.DeleteStatement);
        }

        private void AssumeInsertCommandIsInitialised()
        {
            this.InsertStatement = Substitute.For<IInsertStatement<TestEntity>>();
            this.InsertStatement.For(Entity)
                .Returns(this.InsertStatement);
            this.StatementFactory.CreateInsert<TestEntity>()
                .Returns(this.InsertStatement);
        }

        private void AssumeSelectStatementIsInitialised()
        {
            this.SelectStatement = Substitute.For<ISelectStatement<TestEntity>>();
            this.StatementFactory.CreateSelect<TestEntity>()
                .Returns(this.SelectStatement);
        }

        private void AssumeUpdateCommandIsInitialised()
        {
            this.UpdateStatement = Substitute.For<IUpdateStatement<TestEntity>>();
            this.StatementFactory.CreateUpdate<TestEntity>()
                .Returns(this.UpdateStatement);
            this.UpdateStatement.For(Entity)
                .Returns(this.UpdateStatement);
        }

        protected IStatementFactory StatementFactory { get; private set; }
        protected IDeleteStatement<TestEntity> DeleteStatement { get; private set; }
        protected IInsertStatement<TestEntity> InsertStatement { get; private set; }
        protected ISelectStatement<TestEntity> SelectStatement { get; private set; }
        protected IUpdateStatement<TestEntity> UpdateStatement { get; private set; }
    }
}