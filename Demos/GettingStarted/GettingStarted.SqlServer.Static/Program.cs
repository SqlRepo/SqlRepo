using System;
using System.Reflection;
using GettingStarted.SqlServer.Shared;
using SqlRepo.SqlServer.Static;

namespace GettingStarted.SqlServer.Static
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var connectionProvider = Helper.GetConnectionProvider();

            RepoFactory.UseConnectionProvider(connectionProvider);

            var gettingStarted = new GettingStarted();
            gettingStarted.DoIt();
        }
    }
}