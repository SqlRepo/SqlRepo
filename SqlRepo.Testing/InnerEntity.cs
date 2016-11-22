using System;
using SqlRepo.Model;

namespace SqlRepo.Testing
{
    public class InnerEntity : Entity<int>
    {
        public int TestEntityId { get; set; }
        public int IntProperty { get; set; }
        public string StringProperty { get; set; }
    }
}