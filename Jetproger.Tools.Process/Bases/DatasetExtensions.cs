using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Jetproger.Tools.Cache.Bases;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Process.Bases
{
    public static class DatasetExtensions
    {
        public static DataTable CreateDataTable<T>()
        {
            return CreateDataTable(typeof(T));
        }

        public static DataTable CreateDataTable(Type type)
        {
            var table = new DataTable { TableName = type.Name };
            foreach (var p in Ex.Dotnet.GetSimpleProperties(type)) table.Columns.Add(new DataColumn(p.Name, p.PropertyType));
            return table;
        }

        public static void AddItem(this DataTable table, object item)
        {
            if (table == null)
            {
                return;
            }
            var row = table.NewRow();
            if (item != null)
            {
                row.ItemArray = (Ex.Dotnet.GetSimpleProperties(item.GetType()).Select(p => p.GetValue(item))).ToArray();
            }
            else
            {
                row.ItemArray = (from DataColumn column in table.Columns select (object)null).ToArray();
            }
            table.Rows.Add(row);
        }

        public static IEnumerable<T> GetItems<T>(this DataTable table)
        {
            if (table == null)
            {
                yield break;
            }
            foreach (var row in table.Rows)
            {
                var item = Ex.Dotnet.CreateInstance<T>();
                yield return item;
            }
        }
    }
}