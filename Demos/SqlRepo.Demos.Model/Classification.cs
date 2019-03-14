using System;
using SqlRepo.Model;

namespace SqlRepo.Demos.Model
{
    public class Classification : Entity<int>
    {
        public int AgeRestriction { get; set; }
        public string Code { get; set; }
        public string CountryCode { get; set; }
        public string Description { get; set; }
    }
}