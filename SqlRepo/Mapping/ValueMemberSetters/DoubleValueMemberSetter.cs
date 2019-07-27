using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo.ValueMemberSetters
{
    public class DoubleValueMemberSetter : ValueMemberSetterBase
    {
        public DoubleValueMemberSetter()
            : base(new[] {typeof(double), typeof(double?)}) { }

        protected override object GetValueByColumnIndex(IDataRecord dataRecord, int columnIndex)
        {
            if(dataRecord.IsDBNull(columnIndex))
            {
                return null;
            }

            return dataRecord.GetDouble(columnIndex);
        }
    }
}