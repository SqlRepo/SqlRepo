using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class ShortPropertySetter : PropertySetterBase
    {
        public ShortPropertySetter()
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