using System;
using SqlRepo.SqlServer.ConnectionProviders;
using SqlRepo.SqlServer.Static;

namespace GettingStartedStatic
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var connectionProvider = new AppConfigFirstConnectionProvider();
            RepoFactory.UseConnectionProvider(connectionProvider);

            var gettingStarted = new GettingStarted();
            gettingStarted.DoIt();
        }
    }
}