using System;
using GettingStarted.SqlServer.Shared;
using SqlRepo.SqlServer.Static;

namespace GettingStarted.SqlServer.Static
{
    public class GettingStarted
    {
        public void DoIt()
        {
            var repository = RepoFactory.Create<ToDo>();
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