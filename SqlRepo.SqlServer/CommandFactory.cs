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

        public IDeleteCommand<TEntity> CreateDelete<TEntity>() where TEntity : class, new()
        {
            return new DeleteCommand<TEntity>(commandExecutor,
                entityMapper,
                new WhereClauseBuilder());
        }

        public IInsertCommand<TEntity> CreateInsert<TEntity>() where TEntity : class, new()
        {
            return new InsertCommand<TEntity>(commandExecutor,
                entityMapper,
                writablePropertyMatcher);
        }

        public ISelectCommand<TEntity> CreateSelect<TEntity>() where TEntity : class, new()
        {
            return new SelectCommand<TEntity>(commandExecutor,
                entityMapper);
        }

        public IUpdateCommand<TEntity> CreateUpdate<TEntity>() where TEntity : class, new()
        {
            return new UpdateCommand<TEntity>(commandExecutor,
                entityMapper,
                writablePropertyMatcher,
                new WhereClauseBuilder());
        }
    }
}