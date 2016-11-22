using System;

namespace SqlRepo.SqlServer
{
    public class CommandFactory : ICommandFactory
    {
        private readonly ICommandExecutor commandExecutor;
        private readonly IEntityMapper entityMapper;
        private readonly IWritablePropertyMatcher writablePropertyMatcher;

        public CommandFactory(ICommandExecutor commandExecutor,
            IEntityMapper entityMapper,
            IWritablePropertyMatcher writablePropertyMatcher)
        {
            this.commandExecutor = commandExecutor;
            this.entityMapper = entityMapper;
            this.writablePropertyMatcher = writablePropertyMatcher;
        }

        public IDeleteCommand<TEntity> CreateDelete<TEntity>() where TEntity: class, new()
        {
            return new DeleteCommand<TEntity>(this.commandExecutor, this.entityMapper, new WhereClauseBuilder());
        }

        public IInsertCommand<TEntity> CreateInsert<TEntity>() where TEntity: class, new()
        {
            return new InsertCommand<TEntity>(this.commandExecutor, this.entityMapper, this.writablePropertyMatcher);
        }

        public ISelectCommand<TEntity> CreateSelect<TEntity>() where TEntity: class, new()
        {
            return new SelectCommand<TEntity>(this.commandExecutor,
                this.entityMapper,
                new SelectClauseBuilder(),
                new FromClauseBuilder(),
                new WhereClauseBuilder(),
                new OrderByClauseBuilder());
        }

        public IUpdateCommand<TEntity> CreateUpdate<TEntity>() where TEntity: class, new()
        {
            return new UpdateCommand<TEntity>(this.commandExecutor,
                this.entityMapper,
                this.writablePropertyMatcher,
                new WhereClauseBuilder());
        }
    }
}