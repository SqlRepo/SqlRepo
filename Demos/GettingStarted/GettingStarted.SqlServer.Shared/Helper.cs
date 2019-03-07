using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using SqlRepo.Abstractions;
using SqlRepo.SqlServer.ConnectionProviders;

namespace GettingStarted.SqlServer.Shared
{
    public class Helper
    {
        public static IConnectionProvider GetConnectionProvider()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true)
                                                          .Build();
            var currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                              .Location) + "\\";
            var connectionString = configuration.GetConnectionString("GettingStartedDbCon")
                                                .Replace("|DataDirectory|", currentFolder);

            return new ConnectionStringConnectionProvider(connectionString);
        }
    }
}