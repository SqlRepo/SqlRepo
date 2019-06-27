using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo.ValueMemberSetters
{
    public class CharValueMemberSetter : ValueMemberSetterBase
    {
        public CharValueMemberSetter()
            : base(new[] {typeof(char), typeof(char?)}) { }

        protected override object GetValueByColumnIndex(IDataRecord dataRecord, int columnIndex)
        {
            if(dataRecord.IsDBNull(columnIndex))
            {
                return null;
            }

            return dataRecord.GetChar(columnIndex);
        }
    }
}