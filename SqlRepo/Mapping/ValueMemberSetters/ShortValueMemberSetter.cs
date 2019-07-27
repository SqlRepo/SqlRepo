using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo.ValueMemberSetters
{
    public class ShortValueMemberSetter : ValueMemberSetterBase
    {
        public ShortValueMemberSetter()
            : base(new[] {typeof(short), typeof(short?)}) { }

        protected override object GetValueByColumnIndex(IDataRecord dataRecord, int columnIndex)
        {
            if(dataRecord.IsDBNull(columnIndex))
            {
                return null;
            }

            return dataRecord.GetInt16(columnIndex);
        }
    }
}