using System;
using SqlRepo.Testing;

namespace SqlRepo.Tests.TestObjects
{
    public class MappingTestEntity : TestEntity
    {
        public bool BooleanField;
        public byte ByteField;
        public DateTime DateTimeField;
        public DateTimeOffset DateTimeOffsetField;
        public decimal DecimalField;
        public double DoubleField;
        public float FloatField;
        public int IntField;
        public long LongField;
        public bool? NullableBooleanField;
        public byte? NullableByteField;
        public DateTime? NullableDateTimeField;
        public DateTimeOffset? NullableDateTimeOffsetField;
        public decimal? NullableDecimalField;
        public double? NullableDoubleField;
        public float? NullableFloatField;
        public int? NullableIntField;
        public long? NullableLongField;
        public short? NullableShortField;
        public short ShortField;
        public string StringField;
        public bool BooleanProperty { get; set; }
        public float FloatProperty { get; set; }
        public long LongProperty { get; set; }
        public bool? NullableBooleanProperty { get; set; }
        public byte? NullableByteProperty { get; set; }
        public decimal? NullableDecimalProperty { get; set; }
        public double? NullableDoubleProperty { get; set; }
        public float? NullableFloatProperty { get; set; }
        public int? NullableIntProperty { get; set; }
        public long? NullableLongProperty { get; set; }
        public short? NullableShortProperty { get; set; }
    }
}