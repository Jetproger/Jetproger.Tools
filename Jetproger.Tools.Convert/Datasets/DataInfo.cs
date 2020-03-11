using System;
using System.Collections.Generic;
using System.Reflection;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jc
{
    public class DataInfo
    {
        public PropertyInfo[] Simples { get; private set; } 
        public PropertyInfo[] Lists { get; private set; }

        public Type Type { get; private set; }

        public DataInfo(Type type)
        {
            Type = type;
            var lists = new List<PropertyInfo>();
            var simples = new List<PropertyInfo>();
            foreach (var p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (Je.Meta.IsSimple(p.PropertyType)) simples.Add(p);
                if (Je.Meta.IsList(p.PropertyType)) lists.Add(p);
            }
            Simples = simples.ToArray();
            Lists = lists.ToArray();
        }

        public Dictionary<int, DataInfoItem> GetItems()
        {
            var items = new Dictionary<int, DataInfoItem>();
            GetItems(items, this, null, null, Type, Type, 0, 0, new Type[0]);
            return items;
        }

        private void GetItems(Dictionary<int, DataInfoItem> infoItems, DataInfo info, PropertyInfo itemsProperty, PropertyInfo selfItemsProperty, Type type, Type typeGe, int tableIndex, int parentTableIndex, Type[] parents)
        {
            if (!infoItems.ContainsKey(tableIndex))
            {
                infoItems.Add(tableIndex, new DataInfoItem(itemsProperty, selfItemsProperty, type, typeGe, tableIndex, parentTableIndex));
            }
            else
            {
                var item = infoItems[tableIndex];
                if (item.ItemsProperty == null && itemsProperty != null) infoItems[tableIndex] = new DataInfoItem(itemsProperty, item.SelfItemsProperty, type, typeGe, tableIndex, item.ParentTableIndex);
                if (item.SelfItemsProperty == null && selfItemsProperty != null) infoItems[tableIndex] = new DataInfoItem(item.ItemsProperty, selfItemsProperty, type, typeGe, tableIndex, item.ParentTableIndex);
            }
            if (Je.Ext.ArrContains(parents, typeGe))
            {
                return;
            }
            var newParents = Je.Ext.ArrAdd(parents, typeGe);
            foreach (PropertyInfo p in info.Lists)
            {
                var newTypeGe = Je.Meta.GenericOf(p.PropertyType);
                var newInfo = One<Type, DataInfo>.Get(newTypeGe, x => new DataInfo(x));
                var newTableIndex = typeGe == newTypeGe ? tableIndex : tableIndex + 1;
                GetItems(infoItems, newInfo, typeGe == newTypeGe ? null : p, typeGe == newTypeGe ? p : null, typeGe, newTypeGe, newTableIndex, tableIndex, newParents);
            }
        }
    }

    public class DataInfoItem
    {
        public readonly Dictionary<Guid, object> Items;
        public readonly PropertyInfo SelfItemsProperty;
        public readonly PropertyInfo ItemsProperty;
        public readonly int ParentTableIndex;
        public readonly int TableIndex;
        public readonly Type TypeGe;
        public readonly Type Type;

        public DataInfoItem(PropertyInfo itemsProperty, PropertyInfo selfItemsProperty, Type type, Type typeGe, int tableIndex, int parentTableIndex)
        {
            Items = new Dictionary<Guid, object>();
            SelfItemsProperty = selfItemsProperty;
            ItemsProperty = itemsProperty;
            ParentTableIndex = parentTableIndex;
            TableIndex = tableIndex;
            TypeGe = typeGe;
            Type = type;
        }
    }
}