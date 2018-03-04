using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SqlRepo.Abstractions;
using SqlRepo.Benchmark.Select;
using SqlRepo.SqlServer.ConnectionProviders;
using SqlRepo.SqlServer.ServiceCollection;

namespace SqlRepo.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddSqlRepo();

            SqlRepoBenchmarkDbContext dbContext = new SqlRepoBenchmarkDbContext();
            dbContext.Database.Migrate();

            services.AddSingleton<ISqlLogger, NoOpSqlLogger>();
            services.AddSingleton<IBenchmarkHelpers, BenchmarkHelpers>();

            services.AddSingleton<IBenchmarkOperation, SelectUpdateDeleteBenchmarkOperationDapper>();
            services.AddSingleton<IBenchmarkOperation, SelectUpdateDeleteBenchmarkOperationSqlRepo>();

            services.AddSingleton<IBenchmarkOperation, SelectByDecimalValueBenchmarkOperationDapper>();
            services.AddSingleton<IBenchmarkOperation, SelectAllBenchmarkOperationDapper>();
            services.AddSingleton<IBenchmarkOperation, SelectTop5000BenchmarkOperationDapper>();
            services.AddSingleton<IBenchmarkOperation, SelectTop1BenchmarkOperationDapper>();
            services.AddSingleton<IBenchmarkOperation, SelectWhereBetweenBenchmarkOperationDapper>();
            services.AddSingleton<IBenchmarkOperation, SelectSingleColumnBenchmarkDapper>();
            services.AddSingleton<IBenchmarkOperation, SelectByDecimalValueBenchmarkOperationSqlRepo>();
            services.AddSingleton<IBenchmarkOperation, SelectAllBenchmarkOperationSqlRepo>();
            services.AddSingleton<IBenchmarkOperation, SelectWhereBetweenBenchmarkOperationSqlRepo>();
            services.AddSingleton<IBenchmarkOperation, SelectTop5000BenchmarkOperationSqlRepo>();
            services.AddSingleton<IBenchmarkOperation, SelectTop1BenchmarkOperationSqlRepo>();
            services.AddSingleton<IBenchmarkOperation, SelectSingleColumnBenchmarkSqlRepo>();
            
            services.AddSingleton<IBenchmarkOperation, SelectAllBenchmarkOperationEfCore>();
            services.AddSingleton<IBenchmarkOperation, SelectTop5000BenchmarkOperationEfCore>();
            services.AddSingleton<IBenchmarkOperation, SelectWhereBetweenBenchmarkOperationEfCore>();
            services.AddSingleton<IBenchmarkOperation, SelectTop1BenchmarkOperationEfCore>();
            services.AddSingleton<IBenchmarkOperation, SelectSingleColumnBenchmarkEfCore>();

            services.AddSingleton<IBenchmarkRunner, BenchmarkRunner>();

            services.AddSingleton<IConnectionProvider>(new ConnectionStringConnectionProvider(ConnectionString.Value));

            var serviceCollection = services.BuildServiceProvider();

            serviceCollection.GetService<IBenchmarkRunner>().Run();

            Console.WriteLine("Completed All Benchmarks");

            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
        }
    }
}