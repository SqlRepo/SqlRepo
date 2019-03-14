using System;
using SqlRepo.Model;

namespace SqlRepo.Demos.Model
{
    public class Actor : Entity<int>
    {
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
    }
}