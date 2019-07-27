using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo.ValueMemberSetters
{
    public class FloatValueMemberSetter : ValueMemberSetterBase
    {
        public FloatValueMemberSetter()
            : base(new[] {typeof(float), typeof(float?)}) { }

        protected override object GetValueByColumnIndex(IDataRecord dataRecord, int columnIndex)
        {
            if(dataRecord.IsDBNull(columnIndex))
            {
                return null;
            }

            return dataRecord.GetFloat(columnIndex);
        }
    }
}