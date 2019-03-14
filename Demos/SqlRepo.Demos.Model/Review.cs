using System;
using SqlRepo.Model;

namespace SqlRepo.Demos.Model
{
    public class Review : Entity<int>
    {
        public string Content { get; set; }
        public int MovieId { get; set; }
        public DateTime Reviewed { get; set; }
        public string Reviewer { get; set; }
    }
}