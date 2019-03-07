using System;
using GettingStarted.SqlServer.Shared.Abstractions;
using SqlRepo.Abstractions;

namespace GettingStarted.SqlServer.Shared
{
    public class GettingStarted: IGettingStarted
    {
        private readonly IRepositoryFactory repositoryFactory;

        public GettingStarted(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public void DoIt()
        {
            var repository = this.repositoryFactory.Create<ToDo>();
            var results = repository.Query()
                                    .Select(e => e.Id, e => e.Task, e => e.CreatedDate)
                                    .Where(e => e.IsCompleted == false)
                                    .Go();
            Console.WriteLine("Id,Task,Completed,Created");
            foreach(var row in results)
            {
                Console.WriteLine($"{row.Id},{row.Task},{row.IsCompleted},{row.CreatedDate}");
            }

            Console.ReadLine();
        }
    }
}
