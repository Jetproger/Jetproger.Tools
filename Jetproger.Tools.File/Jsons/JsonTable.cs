using System;
using Tools;

namespace Jetproger.Tools.File.Jsons
{
    [Serializable]
    public class JsonTable : MarshalByRefObject
    {
        private object[] _rows;

        public JsonTable()
        {
        }

        public JsonTable(object[] rows)
        {
            _rows = rows;
        }

        public object[] Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }

        public JsonTable Copy()
        {
            if (_rows == null) return new JsonTable();
            if (_rows.Length == 0) return new JsonTable(new object[0]);
            var newRows = new object[_rows.Length];
            var i = 0;
            foreach (var row in _rows)
            {
                var newRow = row.Copy();
                newRows[i++] = newRow;
            }
            return new JsonTable(newRows);
        }

        public Type GetTypeItem()
        {
            return _rows != null && _rows.Length > 0 ? _rows[0].GetType() : typeof(object);
        }
    }
}