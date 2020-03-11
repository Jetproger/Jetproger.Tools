using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Datasets
{
    public static class DataExtensions
    {
        public static Type AsType(this Jc.DataStyle dataStyle)
        {
            switch (dataStyle)
            {
                case Jc.DataStyle.String: return typeof(string);
                case Jc.DataStyle.Char: return typeof(char);
                case Jc.DataStyle.NullChar: return typeof(char?);
                case Jc.DataStyle.Bool: return typeof(bool);
                case Jc.DataStyle.NullBool: return typeof(bool?);
                case Jc.DataStyle.Byte: return typeof(byte);
                case Jc.DataStyle.NullByte: return typeof(byte?);
                case Jc.DataStyle.Sbyte: return typeof(sbyte);
                case Jc.DataStyle.NullSbyte: return typeof(sbyte?);
                case Jc.DataStyle.Short: return typeof(short);
                case Jc.DataStyle.NullShort: return typeof(short?);
                case Jc.DataStyle.Ushort: return typeof(ushort);
                case Jc.DataStyle.NullUshort: return typeof(ushort?);
                case Jc.DataStyle.Int: return typeof(int);
                case Jc.DataStyle.NullInt: return typeof(int?);
                case Jc.DataStyle.Uint: return typeof(uint);
                case Jc.DataStyle.NullUint: return typeof(uint?);
                case Jc.DataStyle.Long: return typeof(long);
                case Jc.DataStyle.NullLong: return typeof(long?);
                case Jc.DataStyle.Ulong: return typeof(ulong);
                case Jc.DataStyle.NullUlong: return typeof(ulong?);
                case Jc.DataStyle.Guid: return typeof(Guid);
                case Jc.DataStyle.NullGuid: return typeof(Guid?);
                case Jc.DataStyle.Float: return typeof(float);
                case Jc.DataStyle.NullFloat: return typeof(float?);
                case Jc.DataStyle.Decimal: return typeof(decimal);
                case Jc.DataStyle.NullDecimal: return typeof(decimal?);
                case Jc.DataStyle.Double: return typeof(double);
                case Jc.DataStyle.NullDouble: return typeof(double?);
                case Jc.DataStyle.Datetime: return typeof(DateTime);
                case Jc.DataStyle.NullDatetime: return typeof(DateTime?);
                default: return typeof(string);
            }
        }

        public static Jc.DataStyle AsStyle(this Type type)
        {
            if (type.IsEnum) return Jc.DataStyle.Int;
            if (type == typeof(string)) return Jc.DataStyle.String;
            if (type == typeof(char)) return Jc.DataStyle.Char;
            if (type == typeof(char?)) return Jc.DataStyle.NullChar;
            if (type == typeof(bool)) return Jc.DataStyle.Bool;
            if (type == typeof(bool?)) return Jc.DataStyle.NullBool;
            if (type == typeof(byte)) return Jc.DataStyle.Byte;
            if (type == typeof(byte?)) return Jc.DataStyle.NullByte;
            if (type == typeof(sbyte)) return Jc.DataStyle.Sbyte;
            if (type == typeof(sbyte?)) return Jc.DataStyle.NullSbyte;
            if (type == typeof(short)) return Jc.DataStyle.Short;
            if (type == typeof(short?)) return Jc.DataStyle.NullShort;
            if (type == typeof(ushort)) return Jc.DataStyle.Ushort;
            if (type == typeof(ushort?)) return Jc.DataStyle.NullUshort;
            if (type == typeof(int)) return Jc.DataStyle.Int;
            if (type == typeof(int?)) return Jc.DataStyle.NullInt;
            if (type == typeof(uint)) return Jc.DataStyle.Uint;
            if (type == typeof(uint?)) return Jc.DataStyle.NullUint;
            if (type == typeof(long)) return Jc.DataStyle.Long;
            if (type == typeof(long?)) return Jc.DataStyle.NullLong;
            if (type == typeof(ulong)) return Jc.DataStyle.Ulong;
            if (type == typeof(ulong?)) return Jc.DataStyle.NullUlong;
            if (type == typeof(Guid)) return Jc.DataStyle.Guid;
            if (type == typeof(Guid?)) return Jc.DataStyle.NullGuid;
            if (type == typeof(float)) return Jc.DataStyle.Float;
            if (type == typeof(float?)) return Jc.DataStyle.NullFloat;
            if (type == typeof(decimal)) return Jc.DataStyle.Decimal;
            if (type == typeof(decimal?)) return Jc.DataStyle.NullDecimal;
            if (type == typeof(double)) return Jc.DataStyle.Double;
            if (type == typeof(double?)) return Jc.DataStyle.NullDouble;
            if (type == typeof(DateTime)) return Jc.DataStyle.Datetime;
            if (type == typeof(DateTime?)) return Jc.DataStyle.NullDatetime;
            return Jc.DataStyle.String;
        }
    }

    public class DataSerializer
    {
        private readonly Dictionary<int, Jc.DataBlock> _tables;
        private long _id;

        public readonly Jc.DataScope DataScope;

        public DataSerializer(object value)
        {
            _id = 1;
            DataScope = new Jc.DataScope();
            _tables = new Dictionary<int, Jc.DataBlock>();
            if (value == null || value == DBNull.Value)
            {
                return;
            }
            var type = value.GetType();
            if (type == typeof(Jc.DataScope))
            {
                DataScope = (Jc.DataScope)value;
                return;
            }
            if (SerializeSimple(value, type))
            {
                return;
            }
            if (SerializeDataset(value, type))
            {
                return;
            }
            IEnumerable items;
            Type typeGe;
            if (Je.Meta.IsList(type))
            {
                items = (IEnumerable)value;
                typeGe = Je.Meta.GenericOf(type);
            }
            else
            {
                items = new List<object> { value };
                typeGe = type;
            }
            SerializeList(items, typeGe, 0, Guid.Empty, new object[0]);
        }

        private void SerializeList(IEnumerable items, Type typeGe, int tableIndex, Guid parentId, object[] parents)
        {
            var info = Jc.One<Type, Jc.DataInfo>.Get(typeGe, x => new Jc.DataInfo(x));
            var table = GetTable(tableIndex, info);
            if (items == null) return;
            foreach (var item in items)
            {
                var row = GetRow(table, item, info, parentId);
                if (Je.Ext.ArrContains(parents, item)) continue;
                var newParents = Je.Ext.ArrAdd(parents, item);
                foreach (PropertyInfo p in info.Lists)
                {
                    var newItems = p.GetValue(item) as IEnumerable;
                    var newParentId = row.Get(0).As<Guid>();
                    var newTypeGe = Je.Meta.GenericOf(p.PropertyType);
                    var newTableIndex = typeGe != newTypeGe ? tableIndex + 1 : tableIndex;
                    SerializeList(newItems, newTypeGe, newTableIndex, newParentId, newParents);
                }
            }
        }

        private bool SerializeSimple(object value, Type type)
        {
            if (!Je.Meta.IsSimple(type)) return false;
            var tab = DataScope.AddTable();
            tab.AddColumn("Col0", type);
            var row = tab.AddRow();
            row.Set(0, value);
            return true;
        }

        private bool SerializeDataset(object value, Type type)
        {
            if (type != typeof(DataSet)) return false;
            var ds = (DataSet)value;
            foreach (DataTable tab in ds.Tables)
            {
                var dbl = DataScope.AddTable(tab.TableName);
                foreach (DataColumn col in tab.Columns)
                {
                    dbl.AddColumn(col.ColumnName, col.DataType);
                }
                foreach (DataRow row in tab.Rows)
                {
                    var i = 0;
                    var tup = dbl.AddRow();
                    foreach (object o in row.ItemArray) tup.Set(i++, o);
                }
            }
            return true;
        }

        private Jc.IDataTuple GetRow(Jc.DataBlock table, object item, Jc.DataInfo info, Guid parentId)
        {
            var row = table.AddRow();
            var index = 0;
            row.Set(index++, Guid.NewGuid());
            row.Set(index++, parentId);
            foreach (PropertyInfo p in info.Simples)
            {
                var value = p.GetValue(item);
                row.Set(index++, value);
            }
            return (Jc.DataTuple)row;
        }

        private Jc.DataBlock GetTable(int tableIndex, Jc.DataInfo info)
        {
            if (_tables.ContainsKey(tableIndex)) return _tables[tableIndex];
            while (DataScope.Tables.Length <= tableIndex) DataScope.AddTable();
            var table = DataScope.Tables[tableIndex];
            _tables.Add(tableIndex, table);
            table.AddColumn("$rid", Jc.DataStyle.Guid);
            table.AddColumn("$pid", Jc.DataStyle.Guid);
            foreach (PropertyInfo p in info.Simples) table.AddColumn(p.Name, p.PropertyType);
            return table;
        }
    }

    public class DataDeserializer<T>
    {
        public T Item => Je.Meta.IsList(typeof(T)) ? (T)_items : (_items != null && _items.Count > 0 ? (T)_items[0] : default(T));

        private readonly IList _items;

        private readonly Type _type;

        public DataDeserializer(Jc.DataScope dataScope)
        {
            if (dataScope.Tables.Length == 0)
            {
                return;
            }
            _type = typeof(T);
            _items = Je.Meta.IsList(_type) ? (IList)Je.Meta.CreateInstance(_type) : new List<T>();
            if (_type == typeof(Jc.DataScope))
            {
                _items.Add(dataScope.As(_type));
                return;
            }
            if (Je.Meta.IsSimple(_type))
            {
                if (dataScope.Tables[0].Cols.Length > 0 && dataScope.Tables[0].Rows.Length > 0) _items.Add(dataScope.Tables[0].GetRow(0).Get(0).As<T>());
                return;
            }
            if (DeserializeDataset(dataScope, _type))
            {
                return;
            }
            _type = !Je.Meta.IsList(_type) ? _type : Je.Meta.GenericOf(_type);
            var info = Jc.One<Type, Jc.DataInfo>.Get(_type, x => new Jc.DataInfo(x));
            var infoItems = info.GetItems();
            DeserializeList(dataScope, infoItems);
        }

        private bool DeserializeDataset(Jc.DataScope dataScope, Type type)
        {
            if (type != typeof(DataSet)) return false;
            var ds = new DataSet();
            _items.Add(ds.As(_type));
            foreach (Jc.DataBlock dbl in dataScope.Tables)
            {
                var tab = new DataTable(dbl.Name);
                ds.Tables.Add(tab);
                foreach (Jc.DataField col in dbl.Cols)
                {
                    tab.Columns.Add(col.Name, col.Style.AsType());
                }
                foreach (Jc.DataTuple tup in dbl.Rows)
                {
                    var i = 0;
                    var row = tab.NewRow();
                    foreach (object o in tup.Values) row[i++] = o;
                }
            }
            return true;
        }

        private void DeserializeList(Jc.DataScope dataScope, Dictionary<int, Jc.DataInfoItem> infoItems)
        {
            var tableIndex = 0;
            foreach (Jc.DataBlock dbl in dataScope.Tables)
            {
                var infoItem = infoItems[tableIndex];
                foreach (Jc.DataTuple tup in dbl.Rows)
                {
                    Guid rowId, parentId;
                    var item = GetItem(infoItem.TypeGe, dbl.Cols, tup, out rowId, out parentId);
                    if (!infoItem.Items.ContainsKey(rowId)) infoItem.Items.Add(rowId, item);
                    if (parentId == Guid.Empty)
                    {
                        _items.Add(item.As(_type));
                        continue;
                    }
                    if (!TryAddToParent(infoItems, infoItem.ParentTableIndex, parentId, infoItem.ItemsProperty, item))
                    {
                        TryAddToParent(infoItems, tableIndex, parentId, infoItem.SelfItemsProperty, item);
                    }
                }
                tableIndex++;
            }
        }

        private bool TryAddToParent(Dictionary<int, Jc.DataInfoItem> infoItems, int tableIndex, Guid parentId, PropertyInfo itemsProperty, object item)
        {
            if (itemsProperty == null) return false;
            var infoParent = infoItems.ContainsKey(tableIndex) ? infoItems[tableIndex] : null;
            if (infoParent == null) return false;
            var parent = infoParent.Items.ContainsKey(parentId) ? infoParent.Items[parentId] : null;
            if (parent == null) return false;
            var list = itemsProperty.GetValue(parent) as IList;
            if (list == null) return false;
            list.Add(item);
            return true;
        }

        private object GetItem(Type itemType, Jc.DataField[] cols, Jc.IDataTuple row, out Guid rowId, out Guid parentId)
        {
            var info = Jc.One<Type, Jc.DataInfo>.Get(itemType, x => new Jc.DataInfo(x));
            var item = Je.Meta.CreateInstance(itemType);
            parentId = row.Get(1).As<Guid>();
            rowId = row.Get(0).As<Guid>();
            var i = 0;
            foreach (Jc.DataField col in cols)
            {
                foreach (PropertyInfo p in info.Simples)
                {
                    if (p.Name != col.Name) continue;
                    p.SetValue(item, row.Get(i).As(p.PropertyType));
                    break;
                }
                i++;
            }
            foreach (PropertyInfo p in info.Lists)
            {
                var list = p.GetValue(item);
                if (list == null)
                {
                    list = Je.Meta.CreateInstance(p.PropertyType);
                    p.SetValue(item, list);
                }
            }
            return item;
        }
    }
}