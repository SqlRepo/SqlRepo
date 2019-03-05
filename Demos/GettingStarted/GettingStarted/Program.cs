using System;
using Autofac;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Autofac;
using SqlRepo.SqlServer.ConnectionProviders;

namespace GettingStartedIoC
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule<SqlRepoSqlServerAutofacModule>();

            var connectionProvider = new AppConfigFirstConnectionProvider();
            containerBuilder.RegisterInstance(connectionProvider)
                            .As<IConnectionProvider>();

            containerBuilder.RegisterType<GettingStarted>()
                            .As<IGettingStarted>();

            // ... other registrations

            var container = containerBuilder.Build();

            var gettingStarted = container.Resolve<IGettingStarted>();
            gettingStarted.DoIt();
        }
    }
}