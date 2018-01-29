using Microsoft.Extensions.DependencyInjection;

namespace SqlRepo.SqlServer.ServiceCollection
{
    public static class SqlRepoServiceCollectionExtension
    {
        public static IServiceCollection AddSqlRepo(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IRepositoryFactory, RepositoryFactory>();
            serviceCollection.AddTransient<ICommandExecutor, CommandExecutor>();
            serviceCollection.AddTransient<ICommandFactory, CommandFactory>();
            serviceCollection.AddTransient<IEntityMapper, DataReaderEntityMapper>();
            serviceCollection.AddTransient<IWritablePropertyMatcher, WritablePropertyMatcher>();

            return serviceCollection;
        }
    }
}