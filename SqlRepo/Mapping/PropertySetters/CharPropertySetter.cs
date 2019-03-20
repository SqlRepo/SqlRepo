using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class CharPropertySetter : PropertySetterBase
    {
        public CharPropertySetter()
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