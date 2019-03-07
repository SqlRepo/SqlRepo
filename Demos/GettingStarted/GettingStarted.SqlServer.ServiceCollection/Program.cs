using System;
using GettingStarted.SqlServer.Shared;
using GettingStarted.SqlServer.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.ServiceCollection;

namespace GettingStarted.SqlServer.ServiceCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

            serviceCollection.AddSqlRepo();

            var connectionProvider = Helper.GetConnectionProvider();

            serviceCollection.AddSingleton(connectionProvider);
            serviceCollection.AddSingleton<IGettingStarted, Shared.GettingStarted>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var gettingStarted = serviceProvider.GetService<IGettingStarted>();
            gettingStarted.DoIt();
        }
    }
}
