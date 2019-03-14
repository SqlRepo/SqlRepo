using System;
using SqlRepo.Model;

namespace SqlRepo.Demos.Model
{
    public class Language : Entity<int>
    {
        public string Name { get; set; }
    }
}