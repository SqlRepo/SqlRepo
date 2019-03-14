using System;
using SqlRepo.Model;

namespace SqlRepo.Demos.Model
{
    public class Rating : Entity<int>
    {
        public int MovieId { get; set; }
        public int Stars { get; set; }
    }
}