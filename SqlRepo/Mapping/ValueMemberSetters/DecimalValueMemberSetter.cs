using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo.ValueMemberSetters
{
    public class DecimalValueMemberSetter : ValueMemberSetterBase
    {
        public DecimalValueMemberSetter()
            : base(new[] {typeof(decimal), typeof(decimal?)}) { }

        protected override object GetValueByColumnIndex(IDataRecord dataRecord, int columnIndex)
        {
            if(dataRecord.IsDBNull(columnIndex))
            {
                return null;
            }

            return dataRecord.GetDecimal(columnIndex);
        }
    }
}