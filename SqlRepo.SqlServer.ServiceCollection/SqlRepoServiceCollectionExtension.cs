using Microsoft.Extensions.DependencyInjection;
using SqlRepo.SqlServer.Abstractions;

namespace SqlRepo.SqlServer.ServiceCollection
{
    public static class SqlRepoServiceCollectionExtension
    {
        public static IServiceCollection AddSqlRepo(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IRepositoryFactory, RepositoryFactory>();
            serviceCollection.AddTransient<IStatementExecutor, StatementExecutor>();
            serviceCollection.AddTransient<IStatementFactory, StatementFactory>();
            serviceCollection.AddTransient<IEntityMapper, DataReaderEntityMapper>();
            serviceCollection.AddTransient<IWritablePropertyMatcher, WritablePropertyMatcher>();

            return serviceCollection;
        }
    }
}