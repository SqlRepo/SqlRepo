using System;
using SqlRepo.Model;

namespace SqlRepo.Demos.Model
{
    public class Country : Entity<int>
    {
        public string Name { get; set; }
    }
}