using System;
using System.Data;
using NSubstitute;

namespace SqlRepo.Tests
{
    public class DataRecordMockBuilder
    {
        private readonly IDataRecord dataRecord;
        private int fieldCount;

        public static DataRecordMockBuilder CreateNew()
        {
            return new DataRecordMockBuilder();
        }

        public DataRecordMockBuilder()
        {
            this.dataRecord = Substitute.For<IDataRecord>();
        }

        public DataRecordMockBuilder WithBooleanColumn(string name, int index, bool? value)
        {
            this.AddColumn(name, index, value);
            this.dataRecord.GetBoolean(index)
                .Returns(value.GetValueOrDefault());

            return this;
        }

        public DataRecordMockBuilder WithByteColumn(string name, int index, byte? value)
        {
            this.AddColumn(name, index, value);
            this.dataRecord.GetByte(index)
                .Returns(value.GetValueOrDefault());

            return this;
        }

        public DataRecordMockBuilder WithDateTimeColumn(string name, int index, DateTime? value)
        {
            this.AddColumn(name, index, value);
            this.dataRecord.GetDateTime(index)
                .Returns(value.GetValueOrDefault());

            return this;
        }

        public DataRecordMockBuilder WithDateTimeOffsetColumn(string name, int index, DateTimeOffset? value)
        {
            this.AddColumn(name, index, value);
            this.dataRecord.GetValue(index)
                .Returns(value.GetValueOrDefault());

            return this;
        }

        public DataRecordMockBuilder WithDecimalColumn(string name, int index, decimal? value)
        {
            this.AddColumn(name, index, value);
            this.dataRecord.GetDecimal(index)
                .Returns(value.GetValueOrDefault());

            return this;
        }

        public DataRecordMockBuilder WithDoubleColumn(string name, int index, double? value)
        {
            this.AddColumn(name, index, value);
            this.dataRecord.GetDouble(index)
                .Returns(value.GetValueOrDefault());

            return this;
        }

        public DataRecordMockBuilder WithFloatColumn(string name, int index, float? value)
        {
            this.AddColumn(name, index, value);
            this.dataRecord.GetFloat(index)
                .Returns(value.GetValueOrDefault());

            return this;
        }


        public DataRecordMockBuilder WithGuidColumn(string name, int index, Guid? value)
        {
            this.AddColumn(name, index, value);
            this.dataRecord.GetGuid(index)
                .Returns(value.GetValueOrDefault());

            return this;
        }

        public DataRecordMockBuilder WithIntColumn(string name, int index, int? value)
        {
            this.AddColumn(name, index, value);
            this.dataRecord.GetInt32(index)
                .Returns(value.GetValueOrDefault());

            return this;
        }

        public DataRecordMockBuilder WithLongColumn(string name, int index, long? value)
        {
            this.AddColumn(name, index, value);
            this.dataRecord.GetInt64(index)
                .Returns(value.GetValueOrDefault());

            return this;
        }

        public DataRecordMockBuilder WithShortColumn(string name, int index, short? value)
        {
            this.AddColumn(name, index, value);
            this.dataRecord.GetInt16(index)
                .Returns(value.GetValueOrDefault());

            return this;
        }

        public DataRecordMockBuilder WithStringColumn(string name, int index, string value)
        {
            this.AddColumn(name, index, value);
            this.dataRecord.GetString(index)
                .Returns(value);

            return this;
        }

        public IDataRecord Build()
        {
            this.dataRecord.FieldCount.Returns(this.fieldCount);
            return this.dataRecord;
        }

        private void AddColumn(string name, int index, object value)
        {
            this.fieldCount++;
            this.dataRecord.GetOrdinal(name)
                .Returns(index);
            this.dataRecord.GetName(index)
                .Returns(name);
            this.dataRecord.IsDBNull(index)
                .Returns(value == null);
            this.dataRecord.GetValue(index)
                .Returns(value);
        }
    }
}
