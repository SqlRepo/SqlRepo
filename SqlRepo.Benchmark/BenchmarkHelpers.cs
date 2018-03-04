using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SqlRepo.Abstractions;

namespace SqlRepo.Benchmark
{
    public class BenchmarkHelpers : IBenchmarkHelpers
    {
        private readonly IRepository<BenchmarkEntity> repository;

        public BenchmarkHelpers(IRepositoryFactory repositoryFactory)
        {
            this.repository = repositoryFactory.Create<BenchmarkEntity>();
        }

        public void ClearBufferPool()
        {
            var dbContext = new SqlRepoBenchmarkDbContext();
            dbContext.Database.ExecuteSqlCommand("DBCC DROPCLEANBUFFERS");
        }

        public void ClearRecords()
        {
            this.repository.Delete()
                .Go();

            Console.WriteLine($"Deleted all records");
        }

        public void InsertRecords(int amount)
        {
            var entities = new List<BenchmarkEntity>();

            for(var i = 0; i < amount; i++)
            {
                bool? nullableBool = null;

                if(i % 5 == 0)
                {
                    nullableBool = false;
                }
                else if(i % 6 == 0)
                {
                    nullableBool = true;
                }

                entities.Add(new BenchmarkEntity
                             {
                                 DecimalValue = i,
                                 TextValue = i.ToString(),
                                 IntegerValue = i,
                                 NullableBoolean = nullableBool
                             });
            }

            var dbContext = new SqlRepoBenchmarkDbContext();
            dbContext.AddRange(entities);
            dbContext.SaveChanges();

            Console.WriteLine($"Completed inserting {amount} records");
        }

        public void RunActionMultiple(Action action, int timesToRun)
        {
            for(var i = 0; i < timesToRun; i++)
            {
                action.Invoke();
            }
        }
    }
}