using System;
using System.Data;
using SqlRepo.Abstractions;

namespace SqlRepo.ValueMemberSetters
{
    public class GuidValueMemberSetter : ValueMemberSetterBase
    {
        public GuidValueMemberSetter()
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