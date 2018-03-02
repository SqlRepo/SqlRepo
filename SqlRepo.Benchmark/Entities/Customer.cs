using System.Collections.Generic;

namespace SqlRepo.Benchmark.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
        public List<Order> Orders { get; set; }
        public Gender Gender { get; set; }
    }
}