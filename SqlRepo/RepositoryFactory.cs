using System;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IStatementFactoryProvider statementFactoryProvider;

        public RepositoryFactory(IStatementFactoryProvider statementFactoryProvider)
        {
            this.statementFactoryProvider = statementFactoryProvider;
        }

        public IRepository<TEntity> Create<TEntity>() where TEntity: class, new()
        {
            return new Repository<TEntity>(this.statementFactoryProvider.Provide());
        }
    }
}