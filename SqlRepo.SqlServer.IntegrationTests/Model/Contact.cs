using System;
using SqlRepo.Model;

namespace SqlRepo.SqlServer.IntegrationTests.Model
{
    public class Contact : Entity<int>
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}