using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo.ValueMemberSetters
{
    public class StringValueMemberSetter : ValueMemberSetterBase
    {
        public StringValueMemberSetter()
            : base(new[] {typeof(string)}) { }

        protected override object GetValueByColumnIndex(IDataRecord dataRecord, int columnIndex)
        {
            if(dataRecord.IsDBNull(columnIndex))
            {
                return null;
            }

            return dataRecord.GetString(columnIndex);
        }
    }
}