using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Testing;

namespace SqlRepo.Tests
{
    [TestFixture]
    public abstract class RepositoryTestBase : TestBase
    {
        [SetUp]
        public void SetUp()
        {
            this.AssumeTestEntityIsInitialised();
            this.AssumeStatementFactoryIsInitialised();
            this.Repository = new Repository<TestEntity>(this.StatementFactory);
        }

        protected void AssumeDeleteEntityIsRequested()
        {
            this.Repository.Delete(this.Entity);
        }

        protected IDeleteStatement<TestEntity> AssumeDeleteIsRequested()
        {
            return this.Repository.Delete();
        }

        protected TestEntity AssumeInsertEntityIsRequested()
        {
            return this.Repository.Insert(this.Entity);
        }

        protected IInsertStatement<TestEntity> AssumeInsertIsRequested()
        {
            return this.Repository.Insert();
        }

        protected IEnumerable<TestEntity> AssumeResultsFromIsRequested(ISelectStatement<TestEntity> query)
        {
            return this.Repository.ResultsFrom(query);
        }

        protected ISelectStatement<TestEntity> AssumeSelectIsRequested()
        {
            return this.Repository.Query();
        }

        protected void AssumeUpdateEntityIsRequested()
        {
            this.Repository.Update(this.Entity);
        }

        protected IUpdateStatement<TestEntity> AssumeUpdateIsRequested()
        {
            return this.Repository.Update();
        }

        private void AssumeDeleteCommandIsInitialised()
        {
            this.DeleteStatement = Substitute.For<IDeleteStatement<TestEntity>>();
            this.StatementFactory.CreateDelete<TestEntity>()
                .Returns(this.DeleteStatement);
            this.DeleteStatement.For(this.Entity)
                .Returns(this.DeleteStatement);
        }

        private void AssumeInsertCommandIsInitialised()
        {
            this.InsertStatement = Substitute.For<IInsertStatement<TestEntity>>();
            this.InsertStatement.For(this.Entity)
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

        private void AssumeStatementFactoryIsInitialised()
        {
            this.StatementFactory = Substitute.For<IStatementFactory>();
            this.AssumeDeleteCommandIsInitialised();
            this.AssumeInsertCommandIsInitialised();
            this.AssumeUpdateCommandIsInitialised();
            this.AssumeSelectStatementIsInitialised();
        }

        private void AssumeUpdateCommandIsInitialised()
        {
            this.UpdateStatement = Substitute.For<IUpdateStatement<TestEntity>>();
            this.StatementFactory.CreateUpdate<TestEntity>()
                .Returns(this.UpdateStatement);
            this.UpdateStatement.For(this.Entity)
                .Returns(this.UpdateStatement);
        }

        protected IDeleteStatement<TestEntity> DeleteStatement { get; private set; }
        protected IInsertStatement<TestEntity> InsertStatement { get; private set; }

        protected IRepository<TestEntity> Repository { get; private set; }
        protected ISelectStatement<TestEntity> SelectStatement { get; private set; }
        protected IStatementFactory StatementFactory { get; private set; }
        protected IUpdateStatement<TestEntity> UpdateStatement { get; private set; }
    }
}