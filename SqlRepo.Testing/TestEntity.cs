using System;
using SqlRepo.Model;

namespace SqlRepo.Testing
{
    public class TestEntity : Entity<int>
    {
        public DateTimeOffset DateTimeOffsetProperty { get; set; }
        public DateTimeOffset? NullableDateTimeOffsetProperty { get; set; }
        public DateTime DateTimeProperty { get; set; }
        public DateTime? NullableDateTimeProperty { get; set; }
        public double DoubleProperty { get; set; }
        public int IntProperty { get; set; }
        public int IntProperty2 { get; set; }
        public string StringProperty { get; set; }
        public TestEnum TestEnumProperty { get; set; }
        public decimal DecimalProperty { get; set; }
        public byte ByteProperty { get; set; }
        public short ShortProperty { get; set; }
        public float SingleProperty { get; set; }
        public Guid GuidProperty { get; set; }
    }
}