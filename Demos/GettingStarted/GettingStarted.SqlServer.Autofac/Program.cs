using System;
using Autofac;
using System.Reflection;
using GettingStarted.SqlServer.Shared;
using GettingStarted.SqlServer.Shared.Abstractions;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Autofac;

namespace GettingStarted.SqlServer.Autofac
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule<SqlRepoSqlServerAutofacModule>();

            var connectionProvider = Helper.GetConnectionProvider();

            containerBuilder.RegisterInstance(connectionProvider)
                            .As<IConnectionProvider>();

            containerBuilder.RegisterType<Shared.GettingStarted>()
                            .As<IGettingStarted>();

            // ... other registrations

            var container = containerBuilder.Build();

            var gettingStarted = container.Resolve<IGettingStarted>();
            gettingStarted.DoIt();
        }
    }
}