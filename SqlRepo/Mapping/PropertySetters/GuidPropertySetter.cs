using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class GuidPropertySetter : PropertySetterBase
    {
        public GuidPropertySetter()
            : base(new[] {typeof(Guid), typeof(Guid?)}) { }

        protected override object GetValueByColumnIndex(IDataRecord dataRecord, int columnIndex)
        {
            if(dataRecord.IsDBNull(columnIndex))
            {
                return null;
            }

            return dataRecord.GetGuid(columnIndex);
        }
    }
}