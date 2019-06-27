using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo.ValueMemberSetters
{
    public class IntValueMemberSetter : ValueMemberSetterBase
    {
        public IntValueMemberSetter()
            : base(new[] {typeof(int), typeof(int?)}) { }

        protected override object GetValueByColumnIndex(IDataRecord dataRecord, int columnIndex)
        {
            if(dataRecord.IsDBNull(columnIndex))
            {
                return null;
            }

            return dataRecord.GetInt32(columnIndex);
        }
    }
}