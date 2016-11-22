using System;
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
            this.AssumeTestEntityIsInitialised();
            this.AssumeCommandFactoryIsInitialised();
            this.repository = new Repository<TestEntity>(this.CommandFactory);
        }

        private IRepository<TestEntity> repository;

        protected void AssumeDeleteEntityIsRequested()
        {
            this.repository.Delete(this.Entity);
        }

        protected IDeleteCommand<TestEntity> AssumeDeleteIsRequested()
        {
            return this.repository.Delete();
        }

        protected TestEntity AssumeInsertEntityIsRequested()
        {
            return this.repository.Insert(this.Entity);
        }

        protected IInsertCommand<TestEntity> AssumeInsertIsRequested()
        {
            return this.repository.Insert();
        }

        protected IEnumerable<TestEntity> AssumeResultsFromIsRequested(ISelectCommand<TestEntity> query)
        {
            return this.repository.ResultsFrom(query);
        }

        protected ISelectCommand<TestEntity> AssumeSelectIsRequested()
        {
            return this.repository.Query();
        }

        protected void AssumeUpdateEntityIsRequested()
        {
            this.repository.Update(this.Entity);
        }

        protected IUpdateCommand<TestEntity> AssumeUpdateIsRequested()
        {
            return this.repository.Update();
        }

        private void AssumeCommandFactoryIsInitialised()
        {
            this.CommandFactory = Substitute.For<ICommandFactory>();
            this.AssumeDeleteCommandIsInitialised();
            this.AssumeInsertCommandIsInitialised();
            this.AssumeUpdateCommandIsInitialised();
            this.AssumeSelectCommandIsInitialised();
        }

        private void AssumeDeleteCommandIsInitialised()
        {
            this.DeleteCommand = Substitute.For<IDeleteCommand<TestEntity>>();
            this.CommandFactory.CreateDelete<TestEntity>()
                .Returns(this.DeleteCommand);
            this.DeleteCommand.For(this.Entity)
                .Returns(this.DeleteCommand);
        }

        private void AssumeInsertCommandIsInitialised()
        {
            this.InsertCommand = Substitute.For<IInsertCommand<TestEntity>>();
            this.InsertCommand.For(this.Entity)
                .Returns(this.InsertCommand);
            this.CommandFactory.CreateInsert<TestEntity>()
                .Returns(this.InsertCommand);
        }

        private void AssumeSelectCommandIsInitialised()
        {
            this.SelectCommand = Substitute.For<ISelectCommand<TestEntity>>();
            this.CommandFactory.CreateSelect<TestEntity>()
                .Returns(this.SelectCommand);
            this.SelectCommand.FromScratch()
                .Returns(this.SelectCommand);
        }

        private void AssumeUpdateCommandIsInitialised()
        {
            this.UpdateCommand = Substitute.For<IUpdateCommand<TestEntity>>();
            this.CommandFactory.CreateUpdate<TestEntity>()
                .Returns(this.UpdateCommand);
            this.UpdateCommand.For(this.Entity)
                .Returns(this.UpdateCommand);
        }

        protected ICommandFactory CommandFactory { get; private set; }
        protected IDeleteCommand<TestEntity> DeleteCommand { get; private set; }
        protected IInsertCommand<TestEntity> InsertCommand { get; private set; }
        protected ISelectCommand<TestEntity> SelectCommand { get; private set; }
        protected IUpdateCommand<TestEntity> UpdateCommand { get; private set; }
    }
}