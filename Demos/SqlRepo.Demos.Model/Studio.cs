using System;
using SqlRepo.Model;

namespace SqlRepo.Demos.Model
{
    public class Studio : Entity<int>
    {
        public string Name { get; set; }
    }
}