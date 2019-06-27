using System;
using System.Collections.Generic;
using SqlRepo.Testing;

namespace SqlRepo.Tests.TestObjects
{
    public class MappingTestEntity : TestEntity
    {
        public bool BooleanField;
        public byte ByteField;
        public char CharField;
        public DateTime DateTimeField;
        public DateTimeOffset DateTimeOffsetField;
        public decimal DecimalField;
        public double DoubleField;
        public float FloatField;
        public Guid GuidField;
        public int IntField;
        public long LongField;
        public bool? NullableBooleanField;
        public byte? NullableByteField;
        public char? NullableCharField;
        public DateTime? NullableDateTimeField;
        public DateTimeOffset? NullableDateTimeOffsetField;
        public decimal? NullableDecimalField;
        public double? NullableDoubleField;
        public float? NullableFloatField;
        public Guid? NullableGuidField;
        public int? NullableIntField;
        public long? NullableLongField;
        public short? NullableShortField;
        public InnerEntity ObjectField;
        public IEnumerable<InnerEntity> EnumerableField;
        public short ShortField;
        public string StringField;
        public bool BooleanProperty { get; set; }
        public char CharProperty { get; set; }
        public float FloatProperty { get; set; }
        public long LongProperty { get; set; }
        public bool? NullableBooleanProperty { get; set; }
        public byte? NullableByteProperty { get; set; }
        public char? NullableCharProperty { get; set; }
        public decimal? NullableDecimalProperty { get; set; }
        public double? NullableDoubleProperty { get; set; }
        public float? NullableFloatProperty { get; set; }
        public Guid? NullableGuidProperty { get; set; }
        public int? NullableIntProperty { get; set; }
        public long? NullableLongProperty { get; set; }
        public short? NullableShortProperty { get; set; }
        public InnerEntity ObjectProperty { get; set; }
        public IEnumerable<InnerEntity> EnumerableProperty { get; set; }
    }
}