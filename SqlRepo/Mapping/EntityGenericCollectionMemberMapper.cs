using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using SqlRepo.Abstractions;

namespace SqlRepo
{
    public class EntityGenericCollectionMemberMapper<TCollection, TItem> : IEntityMemberMapper
        where TItem: class, new() where TCollection: class, IEnumerable<TItem>, new()
    {
        private readonly IEntityMappingProfile<TItem> itemMappingProfile;

        public EntityGenericCollectionMemberMapper(MemberInfo memberInfo,
            IEntityMappingProfile<TItem> itemMappingProfile)
        {
            this.itemMappingProfile = itemMappingProfile;
            this.MemberInfo = memberInfo;
        }

        public MemberInfo MemberInfo { get; }

        public void Map(object entity, IDataRecord dataRecord)
        {
            var wasInitialised = false;
            if(!(this.MemberInfo.GetValue(entity) is TCollection collection))
            {
                collection = new TCollection();
                this.MemberInfo.SetValue(entity, collection);
                wasInitialised = true;
            }

            var itemInstance = (TItem)Activator.CreateInstance(typeof(TItem));
            this.itemMappingProfile.Map(itemInstance, dataRecord);

            if(wasInitialised || !this.CollectionContainsItem(collection, itemInstance))
            {
                var addMethod = typeof(TCollection).GetMethod("Add");
                if(addMethod != null)
                {
                    addMethod.Invoke(collection, new object[] {itemInstance});
                }
            }
        }

        private bool CollectionContainsItem(TCollection collection, TItem item)
        {
            foreach(var entity in collection)
            {
                if(this.itemMappingProfile.EntitiesMatch(item, entity))
                {
                    return true;
                }
            }
            return false;
        }
    }
}