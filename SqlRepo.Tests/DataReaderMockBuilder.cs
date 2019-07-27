using System;
using System.Data;
using System.Linq;
using NSubstitute;

namespace SqlRepo.Tests
{
    public class DataReaderMockBuilder
    {
        private readonly int rows;
        private readonly IDataReader dataReader;
        private int fieldCount;
        private int currentRowIndex = -1;

        public static DataReaderMockBuilder CreateNew(int rows)
        {
            return new DataReaderMockBuilder(rows);
        }

        public DataReaderMockBuilder(int rows)
        {
            this.rows = rows;
            this.dataReader = Substitute.For<IDataReader>();
            this.dataReader.Read()
                .ReturnsForAnyArgs(a => this.currentRowIndex < this.rows)
                .AndDoes(r => this.currentRowIndex++);
        }

        public DataReaderMockBuilder WithBooleanColumn(string name, int index, params bool?[] values)
        {
            this.AddColumn(name, index, values);
            this.dataReader.GetBoolean(index)
                .Returns(a => values[this.currentRowIndex].GetValueOrDefault());

            return this;
        }

        public DataReaderMockBuilder WithByteColumn(string name, int index, params byte?[] values)
        {
            this.AddColumn(name, index, values);
            this.dataReader.GetByte(index)
                .Returns(a => values[this.currentRowIndex].GetValueOrDefault());

            return this;
        }

        public DataReaderMockBuilder WithDateTimeColumn(string name, int index, params DateTime?[] values)
        {
            this.AddColumn(name, index, values);
            this.dataReader.GetDateTime(index)
                .Returns(a => values[this.currentRowIndex].GetValueOrDefault());

            return this;
        }

        public DataReaderMockBuilder WithDateTimeOffsetColumn(string name, int index, params DateTimeOffset?[] values)
        {
            this.AddColumn(name, index, values);
            this.dataReader.GetValue(index)
                .Returns(a => values[this.currentRowIndex]
                             .GetValueOrDefault());

            return this;
        }

        public DataReaderMockBuilder WithDecimalColumn(string name, int index, params decimal?[] values)
        {
            this.AddColumn(name, index, values);
            this.dataReader.GetDecimal(index)
                .Returns(a => values[this.currentRowIndex]
                             .GetValueOrDefault());

            return this;
        }

        public DataReaderMockBuilder WithDoubleColumn(string name, int index, params double?[] values)
        {
            this.AddColumn(name, index, values);
            this.dataReader.GetDouble(index)
                .Returns(a => values[this.currentRowIndex]
                             .GetValueOrDefault());

            return this;
        }

        public DataReaderMockBuilder WithFloatColumn(string name, int index, params float?[] values)
        {
            this.AddColumn(name, index, values);
            this.dataReader.GetFloat(index)
                .Returns(a => values[this.currentRowIndex]
                             .GetValueOrDefault());

            return this;
        }


        public DataReaderMockBuilder WithGuidColumn(string name, int index, params Guid?[] values)
        {
            this.AddColumn(name, index, values);
            this.dataReader.GetGuid(index)
                .Returns(a => values[this.currentRowIndex]
                             .GetValueOrDefault());

            return this;
        }

        public DataReaderMockBuilder WithIntColumn(string name, int index, params int?[] values)
        {
            this.AddColumn(name, index, values);
            this.dataReader.GetInt32(index)
                .Returns(a => values[this.currentRowIndex]
                             .GetValueOrDefault());

            return this;
        }

        public DataReaderMockBuilder WithLongColumn(string name, int index, params long?[] values)
        {
            this.AddColumn(name, index, values);
            this.dataReader.GetInt64(index)
                .Returns(a => values[this.currentRowIndex]
                             .GetValueOrDefault());

            return this;
        }

        public DataReaderMockBuilder WithShortColumn(string name, int index, params short?[] values)
        {
            this.AddColumn(name, index, values);
            this.dataReader.GetInt16(index)
                .Returns(a => values[this.currentRowIndex]
                             .GetValueOrDefault());

            return this;
        }

        public DataReaderMockBuilder WithStringColumn(string name, int index, params string[] values)
        {
            this.AddColumn(name, index, values);
            this.dataReader.GetString(index)
                .Returns(a => values[this.currentRowIndex]);

            return this;
        }

        public IDataReader Build()
        {
            this.dataReader.FieldCount.Returns(this.fieldCount);
            return this.dataReader;
        }

        private void AddColumn<T>(string name, int index, T[] values)
        {
            if(values == null || values.Length != this.rows)
            {
                throw new InvalidOperationException("Number of values does not match number of rows specified");
            }

            this.fieldCount++;
            this.dataReader.GetOrdinal(name)
                .Returns(index);
            this.dataReader.GetName(index)
                .Returns(name);

            if(!values.Any())
            {
                this.dataReader.IsDBNull(index)
                    .Returns(true);
                this.dataReader.GetValue(index)
                    .Returns(null);
            }
            else
            {
                var valueCount = values.Length;
                var isDbNullValues = new bool[valueCount];
                var getValueValues = new object[valueCount];

                for(var i = 0; i < valueCount; i++)
                {
                    var value = values[i];
                    isDbNullValues[i] = value == null;
                    getValueValues[i] = value;
                }

                this.dataReader.IsDBNull(index)
                    .Returns(a => isDbNullValues[this.currentRowIndex]);
                this.dataReader.GetValue(index)
                    .Returns(a => getValueValues[this.currentRowIndex]);
            }
        }
    }
}
