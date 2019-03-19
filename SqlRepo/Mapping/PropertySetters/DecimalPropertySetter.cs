using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class DecimalPropertySetter : PropertySetterBase
    {
        public DecimalPropertySetter()
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