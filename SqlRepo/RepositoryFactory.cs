using System;

namespace SqlRepo
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IStatementFactory statementFactory;

        public RepositoryFactory(IStatementFactory statementFactory)
        {
            this.statementFactory = statementFactory;
        }

        public IRepository<TEntity> Create<TEntity>() where TEntity: class, new()
        {
            return new Repository<TEntity>(this.statementFactory);
        }
    }
}