using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class IntPropertySetter : PropertySetterBase
    {
        public IntPropertySetter()
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