using System;
using System.Collections.Generic;
using NSubstitute;
using NSubstitute.Extensions;
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

        protected IDeleteStatement<TestEntity> DeleteStatement { get; private set; }
        protected IExecuteNonQueryProcedureStatement ExecuteNonQueryProcedureStatement { get; private set; }
        protected IExecuteNonQuerySqlStatement ExecuteNonQuerySqlStatement { get; private set; }
        protected IExecuteQueryProcedureStatement<TestEntity> ExecuteQueryProcedureStatement
        {
            get;
            private set;
        }
        protected IExecuteQuerySqlStatement<TestEntity> ExecuteQuerySqlStatement { get; private set; }
        protected IInsertStatement<TestEntity> InsertStatement { get; private set; }
        protected IRepository<TestEntity> Repository { get; private set; }
        protected ISelectStatement<TestEntity> SelectStatement { get; private set; }
        protected IStatementFactory StatementFactory { get; private set; }
        protected IUpdateStatement<TestEntity> UpdateStatement { get; private set; }

        protected void AssumeDeleteEntityIsRequested()
        {
            this.Repository.Delete(this.Entity);
        }

        protected IDeleteStatement<TestEntity> AssumeDeleteIsRequested()
        {
            return this.Repository.Delete();
        }

        protected IExecuteNonQueryProcedureStatement AssumeExecuteNonQueryProcedureIsRequested()
        {
            return this.Repository.ExecuteNonQueryProcedure();
        }

        protected IExecuteNonQuerySqlStatement AssumeExecuteNonQuerySqlIsRequested()
        {
            return this.Repository.ExecuteNonQuerySql();
        }

        protected IExecuteQueryProcedureStatement<TestEntity> AssumeExecuteQueryProcedureIsRequested()
        {
            return this.Repository.ExecuteQueryProcedure();
        }

        protected IExecuteQuerySqlStatement<TestEntity> AssumeExecuteQuerySqlIsRequested()
        {
            return this.Repository.ExecuteQuerySql();
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

        private void AssumeDeleteStatementIsInitialised()
        {
            this.DeleteStatement = Substitute.For<IDeleteStatement<TestEntity>>();
            this.StatementFactory.CreateDelete<TestEntity>()
                .Returns(this.DeleteStatement);
            this.DeleteStatement.ReturnsForAll(this.DeleteStatement);
        }

        private void AssumeExecuteNonQueryProcedureStatementIsInitialised()
        {
            this.ExecuteNonQueryProcedureStatement = Substitute.For<IExecuteNonQueryProcedureStatement>();
            this.StatementFactory.CreateExecuteNonQueryProcedure()
                .Returns(this.ExecuteNonQueryProcedureStatement);
            this.ExecuteNonQueryProcedureStatement.ReturnsForAll(this.ExecuteNonQueryProcedureStatement);
        }

        private void AssumeExecuteNonQuerySqlStatementIsInitialised()
        {
            this.ExecuteNonQuerySqlStatement = Substitute.For<IExecuteNonQuerySqlStatement>();
            this.StatementFactory.CreateExecuteNonQuerySql()
                .Returns(this.ExecuteNonQuerySqlStatement);
            this.ExecuteNonQuerySqlStatement.ReturnsForAll(this.ExecuteNonQuerySqlStatement);
        }

        private void AssumeExecuteQueryProcedureStatementIsInitialised()
        {
            this.ExecuteQueryProcedureStatement =
                Substitute.For<IExecuteQueryProcedureStatement<TestEntity>>();
            this.StatementFactory.CreateExecuteQueryProcedure<TestEntity>()
                .Returns(this.ExecuteQueryProcedureStatement);
            this.ExecuteQueryProcedureStatement.ReturnsForAll(this.ExecuteQueryProcedureStatement);
        }

        private void AssumeExecuteQuerySqlStatementIsInitialised()
        {
            this.ExecuteQuerySqlStatement = Substitute.For<IExecuteQuerySqlStatement<TestEntity>>();
            this.StatementFactory.CreateExecuteQuerySql<TestEntity>()
                .Returns(this.ExecuteQuerySqlStatement);
            this.ExecuteQuerySqlStatement.ReturnsForAll(this.ExecuteQuerySqlStatement);
        }

        private void AssumeInsertStatementIsInitialised()
        {
            this.InsertStatement = Substitute.For<IInsertStatement<TestEntity>>();
            this.StatementFactory.CreateInsert<TestEntity>()
                .Returns(this.InsertStatement);
            this.InsertStatement.ReturnsForAll(this.InsertStatement);
        }

        private void AssumeSelectStatementIsInitialised()
        {
            this.SelectStatement = Substitute.For<ISelectStatement<TestEntity>>();
            this.StatementFactory.CreateSelect<TestEntity>()
                .Returns(this.SelectStatement);
            this.StatementFactory.ReturnsForAll(this.SelectStatement);
        }

        private void AssumeStatementFactoryIsInitialised()
        {
            this.StatementFactory = Substitute.For<IStatementFactory>();
            this.AssumeDeleteStatementIsInitialised();
            this.AssumeInsertStatementIsInitialised();
            this.AssumeUpdateStatementIsInitialised();
            this.AssumeSelectStatementIsInitialised();
            this.AssumeExecuteQueryProcedureStatementIsInitialised();
            this.AssumeExecuteNonQueryProcedureStatementIsInitialised();
            this.AssumeExecuteNonQuerySqlStatementIsInitialised();
            this.AssumeExecuteQuerySqlStatementIsInitialised();
        }

        private void AssumeUpdateStatementIsInitialised()
        {
            this.UpdateStatement = Substitute.For<IUpdateStatement<TestEntity>>();
            this.StatementFactory.CreateUpdate<TestEntity>()
                .Returns(this.UpdateStatement);
            this.UpdateStatement.ReturnsForAll(this.UpdateStatement);
        }
    }
}