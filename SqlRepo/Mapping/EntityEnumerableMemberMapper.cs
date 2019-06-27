using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityEnumerableMemberMapper<TEnumerable, TItem> : IEntityMemberMapper
    where TItem: class, new()
    where TEnumerable: class, IEnumerable<TItem>, new()
    {
        private readonly IEntityMappingProfile<TItem> itemMappingProfile;

        public EntityEnumerableMemberMapper(MemberInfo memberInfo, IEntityMappingProfile<TItem> itemMappingProfile)
        {
            this.itemMappingProfile = itemMappingProfile;
            this.MemberInfo = memberInfo;
        }

        public MemberInfo MemberInfo { get; }

        public void Map(object entity, IDataRecord dataRecord)
        {
            if(!(this.MemberInfo.GetValue(entity) is TEnumerable currentValue))
            {
                currentValue = new TEnumerable();
                this.MemberInfo.SetValue(entity, currentValue);
            }

            var itemInstance = Activator.CreateInstance(typeof(TItem));
            this.itemMappingProfile.Map(itemInstance, dataRecord);

            var addMethod = typeof(TEnumerable).GetMethod("Add");
            if(addMethod != null)
            {
                addMethod.Invoke(currentValue, new []{itemInstance});
            }
        }
    }
}