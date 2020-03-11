using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Bases
{
    public static class ValueExtensions
    {
        private static readonly Type DataTableType = typeof(DataTable);
        private static readonly Type DataSetType = typeof(DataSet);
        private static readonly Type GuidType = typeof(Guid);
        private static readonly Type IntType = typeof(int);

        public static T ReadEx<T>(this IValueExpander expander, ValueSet vs)
        {
            var type = typeof(T);
            if (type == DataSetType) return (T)(object)expander.Read(vs);
            if (type == DataTableType) return (T)(object)expander.Read(vs).Tables[0];
            var items = expander.Read<T>(vs);
            return items.Length > 0 ? items[0] : default(T);
        }

        public static T[] Read<T>(this IValueExpander expander, ValueSet vs)
        {
            var type = typeof(T);
            if (IsPrimitive(type)) return vs.Tables[0].Values.Select(row => row[0].As<T>()).ToArray();
            var types = GetStructure(type);
            var dict = new Dictionary<int, Tuple<object, type_info, IList[]>>();
            var items = new List<T>();
            var tableIndex = 0;
            foreach (var valueTable in vs.Tables)
            {
                var currentType = types[tableIndex];
                var currentInfo = Jc.One<Type, type_info>.Get(currentType, x => new type_info(x));
                foreach (object[] row in valueTable.Values)
                {
                    var obj = currentInfo.GetObject(row);
                    if (tableIndex == 0) items.Add((T)obj);
                    var id = row[row.Length - 2].As<int>();
                    if (!dict.ContainsKey(id))
                    {
                        dict.Add(id, new Tuple<object, type_info, IList[]>(obj, currentInfo, currentInfo.GetLists()));
                    }
                    var parent = row[row.Length - 1].As<int>();
                    if (dict.ContainsKey(parent))
                    {
                        var tuple = dict[parent];
                        var info = tuple.Item2;
                        var lists = tuple.Item3;
                        info.AddToLists(obj, lists);
                    }
                }
                tableIndex++;
            }
            foreach (var tuple in dict.Values)
            {
                var obj = tuple.Item1;
                var info = tuple.Item2;
                var lists = tuple.Item3;
                info.SetLists(obj, lists);
            }
            return items.ToArray();
        }

        public static DataSet Read(this IValueExpander expander, ValueSet vs)
        {
            var ds = new DataSet();
            foreach (ValueTable vt in vs.Tables)
            {
                var dt = new DataTable();
                for (int i = 0; i < vt.Columns.Length; i++)
                {
                    dt.Columns.Add(vt.Columns[i], CodeToType(vt.Types[i]));
                }
                for (int i = 0; i < vt.Values.Length; i++)
                {
                    var dr = dt.NewRow();
                    var vr = vt.Values[i];
                    for (int j = 0; j < vt.Columns.Length; j++)
                    {
                        dr[j] = vr[j];
                    }
                    dt.Rows.Add(dr);
                }
                ds.Tables.Add(dt);
            }
            return ds;
        }

        public static ValueSet Write<T>(this IValueExpander expander, T item) where T : class
        {
            var type = typeof(T);
            if (type == DataSetType) return expander.Write((DataSet)(object)item);
            if (type == DataTableType) return expander.Write((DataTable)(object)item);
            return expander.Write<T>(new [] {item});
        }

        public static ValueSet Write<T>(this IValueExpander expander, IEnumerable<T> items) where T : class
        {
            var vs = new ValueSet();
            var type = typeof(T);
            var info = Jc.One<Type, type_info>.Get(type, x => new type_info(x));
            var n = 0;
            vs.Tables = Write0(items, type, info, -1, ref n);
            return vs;
        }

        public static ValueSet WriteEx<T>(this IValueExpander expander, T item)
        {
            var type = typeof(T);
            if (type == DataSetType) return expander.Write((DataSet)(object)item);
            if (type == DataTableType) return expander.Write((DataTable)(object)item);
            return expander.WriteEx<T>(new[] { item });
        }

        public static ValueSet WriteEx<T>(this IValueExpander expander, IEnumerable<T> items)
        {
            var vs = new ValueSet();
            var type = typeof(T);
            if (!IsPrimitive(type))
            {
                var info = Jc.One<Type, type_info>.Get(type, x => new type_info(x));
                var n = 0;
                vs.Tables = Write0(items, type, info, -1, ref n);
                return vs;
            }
            var values = new List<object[]>();
            foreach (var item in items)
            {
                var row = new object[1];
                row[0] = item;
                values.Add(row);
            }
            vs.Tables = new [] { new ValueTable { Columns = new [] { "Column0" }, Types = new [] { TypeToCode(type) }, Values = values.ToArray() } };
            return vs;
        }

        private static ValueTable[] Write0(IEnumerable items, Type type, type_info info, int nn, ref int n)
        {
            var tables = new List<ValueTable> { new ValueTable { Columns = info.Columns, Types = info.Codes } };
            var rowGroups = info.GetGroups<object[]>();
            var itemGroups = info.GetGroups<object>();
            var list = new List<object[]>();
            foreach (var item in items)
            {
                n = n + 1;
                var parent = n;
                list.Add(info.GetRow(item, n, nn));
                AddRows(item, rowGroups, itemGroups, info.Arrays, parent, ref n);
            }
            var i = 0;
            foreach (var tuple in info.Arrays)
            {
                var elements = itemGroups[i];
                var elementRows = rowGroups[i++];
                var elementType = tuple.Item2;
                var elementInfo = Jc.One<Type, type_info>.Get(elementType, x => new type_info(x));
                tables.Add(new ValueTable { Columns = elementInfo.Columns, Types = elementInfo.Codes, Values = elementRows.ToArray() });
                tables.AddRange(Write1(elements, elementRows, elementType, elementInfo, ref n));
            }
            tables[0].Values = list.ToArray();
            return tables.ToArray();
        }

        private static ValueTable[] Write1(IEnumerable items, List<object[]> rows, Type type, type_info info, ref int n)
        {
            var rowGroups = info.GetGroups<object[]>();
            var itemGroups = info.GetGroups<object>();
            var tables = new List<ValueTable>();
            var i = 0;
            foreach (var item in items)
            {
                var row = rows[i++];
                AddRows(item, rowGroups, itemGroups, info.Arrays, row[row.Length - 2].As<int>(), ref n);
            }
            i = 0;
            foreach (var tuple in info.Arrays)
            {
                var elements = itemGroups[i];
                var elementRows = rowGroups[i++];
                var elementType = tuple.Item2;
                var elementInfo = Jc.One<Type, type_info>.Get(elementType, x => new type_info(x));
                tables.Add(new ValueTable { Columns = elementInfo.Columns, Types = elementInfo.Codes, Values = elementRows.ToArray() });
                tables.AddRange(Write2(elements, elementRows, elementType, elementInfo, ref n));
            }
            return tables.ToArray();
        }

        private static ValueTable[] Write2(IEnumerable items, List<object[]> rows, Type type, type_info info, ref int n)
        {
            var rowGroups = info.GetGroups<object[]>();
            var itemGroups = info.GetGroups<object>();
            var tables = new List<ValueTable>();
            var i = 0;
            foreach (var item in items)
            {
                var row = rows[i++];
                AddRows(item, rowGroups, itemGroups, info.Arrays, row[row.Length - 2].As<int>(), ref n);
            }
            i = 0;
            foreach (var tuple in info.Arrays)
            {
                var elements = itemGroups[i];
                var elementRows = rowGroups[i++];
                var elementType = tuple.Item2;
                var elementInfo = Jc.One<Type, type_info>.Get(elementType, x => new type_info(x));
                tables.Add(new ValueTable { Columns = elementInfo.Columns, Types = elementInfo.Codes, Values = elementRows.ToArray() });
                tables.AddRange(Write3(elements, elementRows, elementType, elementInfo, ref n));
            }
            return tables.ToArray();
        }

        private static ValueTable[] Write3(IEnumerable items, List<object[]> rows, Type type, type_info info, ref int n)
        {
            var rowGroups = info.GetGroups<object[]>();
            var itemGroups = info.GetGroups<object>();
            var tables = new List<ValueTable>();
            var i = 0;
            foreach (var item in items)
            {
                var row = rows[i++];
                AddRows(item, rowGroups, itemGroups, info.Arrays, row[row.Length - 2].As<int>(), ref n);
            }
            i = 0;
            foreach (var tuple in info.Arrays)
            {
                var elements = itemGroups[i];
                var elementRows = rowGroups[i++];
                var elementType = tuple.Item2;
                var elementInfo = Jc.One<Type, type_info>.Get(elementType, x => new type_info(x));
                tables.Add(new ValueTable { Columns = elementInfo.Columns, Types = elementInfo.Codes, Values = elementRows.ToArray() });
            }
            return tables.ToArray();
        }

        private static void AddRows(object item, List<object[]>[] rowGroups, List<object>[] itemGroups, Tuple<PropertyInfo, Type>[] arrays, int nn, ref int n)
        {
            var i = 0;
            foreach (var tuple in arrays)
            {
                var p = tuple.Item1;
                var rows = rowGroups[i];
                var items = itemGroups[i++];
                var elementType = tuple.Item2;
                var array = p.GetValue(item) as IEnumerable;
                if (array == null) continue;
                var elementInfo = Jc.One<Type, type_info>.Get(elementType, x => new type_info(x));
                foreach (var element in array)
                {
                    var row = elementInfo.GetRow(element, ++n, nn);
                    items.Add(element);
                    rows.Add(row);
                }
            }
        }

        public static ValueSet Write(this IValueExpander expander, DataTable table)
        {
            var ds = new DataSet();
            ds.Tables.Add(table);
            return expander.Write(ds);
        }

        public static ValueSet Write(this IValueExpander expander, DataSet ds)
        {
            var vs = new ValueSet();
            var vts = new List<ValueTable>();
            foreach (DataTable dt in ds.Tables)
            {
                var columns = new string[dt.Columns.Count];
                var codes = new TypeCode[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    columns[i] = dt.Columns[i].ColumnName;
                    codes[i] = TypeToCode(dt.Columns[i].DataType);
                }
                var values = new List<object[]>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    var vr = new object[dt.Columns.Count];
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        vr[j] = dr[j];
                    }
                    values.Add(vr);
                }
                vts.Add(new ValueTable { Columns = columns, Types = codes, Values = values.ToArray() });
            }
            vs.Tables = vts.ToArray();
            return vs;
        }

        private class type_info
        {
            public readonly Tuple<PropertyInfo, Type>[] Arrays;
            public readonly PropertyInfo[] Primitives;
            public readonly string[] Columns;
            public readonly TypeCode[] Codes;
            public readonly Type[] Types;
            public readonly Type Type;

            public type_info(Type type)
            {
                var arrays = new List<Tuple<PropertyInfo, Type>>();
                var primitives = new List<PropertyInfo>();
                var columns = new List<string>();
                var codes = new List<TypeCode>();
                var types = new List<Type>();
                Type = type;
                foreach (var p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (IsPrimitive(p.PropertyType))
                    {
                        codes.Add(TypeToCode(p.PropertyType));
                        types.Add(p.PropertyType);
                        columns.Add(p.Name);
                        primitives.Add(p);
                    }
                    if (p.PropertyType.IsArray)
                    {
                        var elementType = p.PropertyType.HasElementType ? p.PropertyType.GetElementType() : null;
                        if (elementType != null && !elementType.IsPrimitive) arrays.Add(new Tuple<PropertyInfo, Type>(p, elementType));
                    }
                }
                Primitives = primitives.ToArray();
                Arrays = arrays.ToArray();
                codes.AddRange(new [] { TypeCode.Int32, TypeCode.Int32 });
                Codes = codes.ToArray();
                types.AddRange(new [] { IntType, IntType });
                Types = types.ToArray();
                columns.AddRange(new[] { "n", "nn" });
                Columns = columns.ToArray();
            }

            public List<T>[] GetGroups<T>()
            {
                var groups = new List<T>[Arrays.Length];
                var i = 0;
                foreach (var array in Arrays)
                {
                    groups[i++] = new List<T>();
                }
                return groups;
            }

            public object[] GetRow(object obj, int n, int  nn)
            {
                var row = new object[Types.Length];
                var i = 0;
                if (obj == null || obj == DBNull.Value || obj.GetType() != Type)
                {
                    foreach (var type in Types)
                    {
                        row[i++] = Je.Meta.DefaultOf(type);
                    }
                    return row;
                }
                foreach (var p in Primitives)
                {
                    row[i++] = p.GetValue(obj);
                }
                row[row.Length - 1] = nn;
                row[row.Length - 2] = n;
                return row;
            }

            public object GetObject(object[] row)
            {
                var obj = Activator.CreateInstance(Type);
                for (int i = 0; i < Primitives.Length; i++)
                {
                    Primitives[i].SetValue(obj, row[i].As(Types[i]));
                }
                return obj;
            }

            public IList[] GetLists()
            {
                var lists = new IList[Arrays.Length];
                var i = 0;
                foreach (var tuple in Arrays)
                {
                    var listType = (typeof(List<>)).MakeGenericType(tuple.Item2);
                    lists[i++] = Activator.CreateInstance(listType) as IList;
                }
                return lists;
            }

            public void SetLists(object obj, IList[] lists)
            {
                for (int i = 0; i < Arrays.Length; i++)
                {
                    var list = lists[i];
                    var tuple = Arrays[i];
                    var propertyArray = tuple.Item1;
                    var elementType = tuple.Item2;
                    var array = Array.CreateInstance(elementType, list.Count);
                    for (int j = 0; j < list.Count; j++)
                    {
                        array.SetValue(list[j], j);
                    }
                    propertyArray.SetValue(obj, array);
                }
            }

            public void AddToLists(object obj, IList[] lists)
            {
                var type = obj.GetType();
                for (int i = 0; i < Arrays.Length; i++)
                {
                    var list = lists[i];
                    var tuple = Arrays[i];
                    var elementType = tuple.Item2;
                    if (elementType == type)
                    {
                        list.Add(obj);
                        break;
                    }
                }
            }
        }

        private static Type[] GetStructure(Type type)
        {
            var types = new List<Type>();
            types.Add(type);
            GetStructure(type, types);
            return types.ToArray();
        }

        private static void GetStructure(Type type, List<Type> types)
        {
            var info = Jc.One<Type, type_info>.Get(type, x => new type_info(x));
            foreach (var tuple in info.Arrays)
            {
                var elementType = tuple.Item2;
                if (types.Any(x => x == elementType)) break;
                types.Add(elementType);
                GetStructure(elementType, types);
            }
        }

        public static TypeCode TypeToCode(Type type)
        {
            return Type.GetTypeCode(type);
        }

        public static Type CodeToType(TypeCode typeCode)
        {
            return typeCode != TypeCode.Object ? Type.GetType("System." + typeCode) : GuidType;
        }

        public static bool IsPrimitive(Type type)
        {
            return type == GuidType || type.IsEnum || Type.GetTypeCode(type) != TypeCode.Object;
        }
    }

    [DataContract]
    [Serializable]
    public class ValueSet
    {
        private ValueTable[] _tables;

        [DataMember]
        public ValueTable[] Tables
        {
            get { return _tables; }
            set { _tables = value; }
        }
    }
    [DataContract]
    [Serializable]
    public class ValueTable
    {
        private string[] _columns;
        private TypeCode[] _types;
        private object[][] _values;

        [DataMember]
        public string[] Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        [DataMember]
        public TypeCode[] Types
        {
            get { return _types; }
            set { _types = value; }
        }

        public object[][] Values
        {
            get { return _values; }
            set { _values = value; }
        }

        [DataMember]
        public string[][] StringValues
        {
            get { return GetStringTable(_values); }
            set { _values = GetObjectTable(value); }
        }

        private static string[][] GetStringTable(object[][] objectTable)
        {
            var stringTable = new List<string[]>();
            foreach (object[] objectRow in (objectTable ?? new object[0][]))
            {
                var stringRow = GetStringRow(objectRow);
                stringTable.Add(stringRow);
            }
            return stringTable.ToArray();
        }

        private object[][] GetObjectTable(string[][] stringTable)
        {
            var objectTable = new List<object[]>();
            foreach (string[] stringRow in (stringTable ?? new string[0][]))
            {
                var objectRow = GetObjectRow(stringRow);
                objectTable.Add(objectRow);
            }
            return objectTable.ToArray();
        }

        private static string[] GetStringRow(object[] objectRow)
        {
            if (objectRow == null) return null;
            var stringRow = new string[objectRow.Length];
            for (int i = 0; i < objectRow.Length; i++) stringRow[i] = Je.Txt.Of(objectRow[i]);
            return stringRow;
        }

        private object[] GetObjectRow(string[] stringRow)
        {
            if (stringRow == null) return null;
            var objectRow = new object[stringRow.Length];
            for (int i = 0; i < stringRow.Length; i++)
            {
                var type = ValueExtensions.CodeToType(Types[i]);
                objectRow[i] = Je.Txt.To(stringRow[i], type);
            }
            return objectRow;
        }
    }
}