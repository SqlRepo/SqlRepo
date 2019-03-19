using System;
using System.Collections.Generic;

namespace SqlRepo.Tests.TestObjects
{
    public class Person
    {
        public IEnumerable<Person> Children { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public int Id { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
        public Person Spouse { get; set; }
    }
}