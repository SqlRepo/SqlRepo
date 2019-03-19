using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class DateTimePropertySetter : PropertySetterBase
    {
        public DateTimePropertySetter()
            : base(new[] {typeof(DateTime), typeof(DateTime?)}) { }

        protected override object GetValueByColumnIndex(IDataRecord dataRecord, int columnIndex)
        {
            if(dataRecord.IsDBNull(columnIndex))
            {
                return null;
            }

            return dataRecord.GetDateTime(columnIndex);
        }
    }
}