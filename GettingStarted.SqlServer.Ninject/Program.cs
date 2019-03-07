using System;
using GettingStarted.SqlServer.Shared;
using GettingStarted.SqlServer.Shared.Abstractions;
using Ninject;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.Ninject;

namespace GettingStarted.SqlServer.Ninject
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Load(new []{ new SqlRepoSqlServerNinjectModule() });

            var connectionProvider = Helper.GetConnectionProvider();

            kernel.Bind<IConnectionProvider>()
                  .ToConstant(connectionProvider)
                  .InSingletonScope();
            
            kernel.Bind<IGettingStarted>()
                  .To<Shared.GettingStarted>()
                  .InSingletonScope();

            var gettingStarted = kernel.Get<IGettingStarted>();
            gettingStarted.DoIt();
        }

    }
}
