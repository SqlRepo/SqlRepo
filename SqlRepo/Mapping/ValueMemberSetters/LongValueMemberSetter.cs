using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo.ValueMemberSetters
{
    public class LongValueMemberSetter : ValueMemberSetterBase
    {
        public LongValueMemberSetter()
            : base(new[] {typeof(long), typeof(long?)}) { }

        protected override object GetValueByColumnIndex(IDataRecord dataRecord, int columnIndex)
        {
            if(dataRecord.IsDBNull(columnIndex))
            {
                return null;
            }

            return dataRecord.GetInt64(columnIndex);
        }
    }
}