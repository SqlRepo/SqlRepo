using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo.ValueMemberSetters
{
    public class BooleanValueMemberSetter : ValueMemberSetterBase
    {
        public BooleanValueMemberSetter()
            : base(new[] {typeof(bool), typeof(bool?)}) { }

        protected override object GetValueByColumnIndex(IDataRecord dataRecord, int columnIndex)
        {
            if(dataRecord.IsDBNull(columnIndex))
            {
                return null;
            }

            return dataRecord.GetBoolean(columnIndex);
        }
    }
}