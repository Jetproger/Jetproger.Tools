using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Datasets;

namespace Jc
{
    [Serializable]
    [DataContract]
    public class DataBlock
    {
        private readonly List<DataField> _cols = new List<DataField>();
        private readonly List<DataTuple> _rows = new List<DataTuple>();

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DataField[] Cols
        {
            get { return _cols.ToArray(); }
            set { SetFields(value); }
        }

        [DataMember]
        public DataTuple[] Rows
        {
            get { return _rows.ToArray(); }
            set { SetTuples(value); }
        }

        private void SetFields(DataField[] values)
        {
            _cols.Clear();
            if (values != null) _cols.AddRange(values);
        }

        private void SetTuples(DataTuple[] values)
        {
            _rows.Clear();
            if (values != null) _rows.AddRange(values);
        }

        #region Value

        public object GetValue(DataField col, int row)
        {
            return GetValue(ColumnIndexOf(col.Name), row);
        }

        public object GetValue(string col, int row)
        {
            return GetValue(ColumnIndexOf(col), row);
        }

        public object GetValue(int col, int row)
        {
            col = ResolveIndex(_cols, col);
            if (col < 0) return null;
            row = ResolveIndex(_rows, row);
            if (row < 0) return null;
            return ((IDataTuple)_rows[row]).Get(col);
        }

        public void SetValue(DataField col, int row, object value)
        {
            SetValue(ColumnIndexOf(col.Name), row, value);
        }

        public void SetValue(string col, int row, object value)
        {
            SetValue(ColumnIndexOf(col), row, value);
        }

        public void SetValue(int col, int row, object value)
        {
            col = ResolveIndex(_cols, col);
            if (col < 0) return;
            row = ResolveIndex(_rows, row);
            if (row < 0) return;
            var tuple = (IDataTuple)_rows[row];
            tuple.Set(col, value);
        }

        #endregion

        #region Column

        public void AddColumn(string name, Type type)
        {
            AddColumn(new DataField { Name = name, Style = type.AsStyle() });
        }

        public void AddColumn(string name, DataStyle dataStyle)
        {
            AddColumn(new DataField { Name = name, Style = dataStyle});
        }

        public void AddColumn(DataField field)
        {
            if (field == null) return;
            _cols.Add(field);
            var value = Je.Meta.DefaultOf(field.Style.AsType());
            foreach (IDataTuple row in _rows) row.Set(value);
        }

        public void RemoveColumn(int index)
        {
            index = ResolveIndex(_cols, index);
            if (index < 0) return;
            _cols.RemoveAt(index);
            foreach (IDataTuple row in _rows) row.RemoveAt(index);
        }

        public int ColumnIndexOf(DataField col)
        {
            for (int i = 0; i < _cols.Count; i++)
            {
                if (_cols[i].Name == col.Name) return i;
            }
            return -1;
        }

        public int ColumnIndexOf(string name)
        {
            for (int i = 0; i < _cols.Count; i++)
            {
                if (_cols[i].Name == name) return i;
            }
            return -1;
        }

        public DataField GetColumn(string name)
        {
            foreach (DataField col in _cols)
            {
                if (col.Name == name) return col;
            }
            return null;
        }

        public DataField GetColumn(int index)
        {
            return index >= 0 && index < _cols.Count ? _cols[index] : null;
        }

        #endregion

        #region Row

        public void AddRow(DataTuple row)
        {
            foreach (var tuple in _rows)
            {
                if (ReferenceEquals(tuple, row)) return;
            }
            _rows.Add(row);
        }

        public IDataTuple AddRow()
        {
            var row = new DataTuple();
            var tuple = (IDataTuple)row;
            foreach (DataField col in _cols) tuple.Set(Je.Meta.DefaultOf(col.Style.AsType()));
            _rows.Add(row);
            return row;
        }

        public void RemoveRow(int index)
        {
            index = ResolveIndex(_rows, index);
            if (index < 0) return;
            _rows.RemoveAt(index);
        }

        public IDataTuple GetRow(int index)
        {
            return index >= 0 && index < _rows.Count ? _rows[index] : null;
        }

        public int RowIndexOf(DataTuple row)
        {
            for (int i = 0; i < _rows.Count; i++)
            {
                if (ReferenceEquals(_rows[i], row)) return i;
            }
            return -1;
        }

        #endregion

        private static int ResolveIndex(IList list, int index)
        {
            index = index >= 0 ? index : 0;
            return index < list.Count ? index : list.Count - 1;
        }
    }
}