using System;
using SqlRepo.Abstractions;

namespace GettingStartedIoC
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

            // ...

        }
    }
}
