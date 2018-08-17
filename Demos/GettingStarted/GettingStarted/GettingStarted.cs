using SqlRepo.Abstractions;
using System;

namespace GettingStartedIoC
{
    public class GettingStarted : IGettingStarted
    {
        private readonly IRepositoryFactory repositoryFactory;

        public GettingStarted(IRepositoryFactory repositoryFactory)
        {
            this.repositoryFactory = repositoryFactory;
        }

        public void DoIt()
        {
            var repository = repositoryFactory.Create<ToDo>();
            var results = repository.Query()
                                    .Select(e => e.Id, e => e.Task, e => e.CreatedDate)
                                    .Where(e => e.IsCompleted == false)
                                    .Go();

            foreach (var row in results)
            {
                Console.WriteLine($"{row.Id},{row.Task},{row.IsCompleted},{row.CreatedDate}");

            }

        }
    }
}
