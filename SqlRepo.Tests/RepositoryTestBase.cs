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
            AssumeCommandFactoryIsInitialised();
            repository = new Repository<TestEntity>(CommandFactory);
        }

        private IRepository<TestEntity> repository;

        protected void AssumeDeleteEntityIsRequested()
        {
            repository.Delete(Entity);
        }

        protected IDeleteCommand<TestEntity> AssumeDeleteIsRequested()
        {
            return repository.Delete();
        }

        protected TestEntity AssumeInsertEntityIsRequested()
        {
            return repository.Insert(Entity);
        }

        protected IInsertCommand<TestEntity> AssumeInsertIsRequested()
        {
            return repository.Insert();
        }

        protected IEnumerable<TestEntity> AssumeResultsFromIsRequested(ISelectCommand<TestEntity> query)
        {
            return repository.ResultsFrom(query);
        }

        protected ISelectCommand<TestEntity> AssumeSelectIsRequested()
        {
            return repository.Query();
        }

        protected void AssumeUpdateEntityIsRequested()
        {
            repository.Update(Entity);
        }

        protected IUpdateCommand<TestEntity> AssumeUpdateIsRequested()
        {
            return repository.Update();
        }

        private void AssumeCommandFactoryIsInitialised()
        {
            CommandFactory = Substitute.For<ICommandFactory>();
            AssumeDeleteCommandIsInitialised();
            AssumeInsertCommandIsInitialised();
            AssumeUpdateCommandIsInitialised();
            AssumeSelectCommandIsInitialised();
        }

        private void AssumeDeleteCommandIsInitialised()
        {
            DeleteCommand = Substitute.For<IDeleteCommand<TestEntity>>();
            CommandFactory.CreateDelete<TestEntity>()
                .Returns(DeleteCommand);
            DeleteCommand.For(Entity)
                .Returns(DeleteCommand);
        }

        private void AssumeInsertCommandIsInitialised()
        {
            InsertCommand = Substitute.For<IInsertCommand<TestEntity>>();
            InsertCommand.For(Entity)
                .Returns(InsertCommand);
            CommandFactory.CreateInsert<TestEntity>()
                .Returns(InsertCommand);
        }

        private void AssumeSelectCommandIsInitialised()
        {
            SelectCommand = Substitute.For<ISelectCommand<TestEntity>>();
            CommandFactory.CreateSelect<TestEntity>()
                .Returns(SelectCommand);
        }

        private void AssumeUpdateCommandIsInitialised()
        {
            UpdateCommand = Substitute.For<IUpdateCommand<TestEntity>>();
            CommandFactory.CreateUpdate<TestEntity>()
                .Returns(UpdateCommand);
            UpdateCommand.For(Entity)
                .Returns(UpdateCommand);
        }

        protected ICommandFactory CommandFactory { get; private set; }
        protected IDeleteCommand<TestEntity> DeleteCommand { get; private set; }
        protected IInsertCommand<TestEntity> InsertCommand { get; private set; }
        protected ISelectCommand<TestEntity> SelectCommand { get; private set; }
        protected IUpdateCommand<TestEntity> UpdateCommand { get; private set; }
    }
}