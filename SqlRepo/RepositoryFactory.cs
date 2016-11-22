using System;

namespace SqlRepo
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly ICommandFactory commandFactory;

        public RepositoryFactory(ICommandFactory commandFactory)
        {
            this.commandFactory = commandFactory;
        }

        public IRepository<TEntity> Create<TEntity>() where TEntity: class, new()
        {
            return new Repository<TEntity>(this.commandFactory);
        }
    }
}