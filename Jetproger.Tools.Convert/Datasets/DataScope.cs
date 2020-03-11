using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jc
{
    [Serializable]
    [DataContract]
    public class DataScope
    {
        private readonly List<DataBlock> _tabs = new List<DataBlock>();

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DataBlock[] Tables
        {
            get { return _tabs.ToArray(); }
            set { SetTables(value); }
        }

        private void SetTables(DataBlock[] values)
        {
            _tabs.Clear();
            if (values != null) _tabs.AddRange(values);
        }

        public DataBlock AddTable(string name = null)
        {
            name = !string.IsNullOrWhiteSpace(name) ? name : string.Format("Table{0}", _tabs.Count);
            foreach (DataBlock tab in _tabs)
            {
                if (name == tab.Name) return tab;
            }
            _tabs.Add(new DataBlock { Name = name });
            return _tabs[_tabs.Count - 1];
        }

        public void AddTable(DataBlock dataBlock)
        {
            if (dataBlock == null) return;
            foreach (DataBlock tab in _tabs)
            {
                if (ReferenceEquals(tab, dataBlock)) return;
            }
            _tabs.Add(dataBlock);
        }

        public void RemoveTable(int index)
        {
            index = ResolveIndex(_tabs, index);
            if (index >= 0) _tabs.RemoveAt(index);
        }

        private static int ResolveIndex(IList list, int index)
        {
            index = index >= 0 ? index : 0;
            return index < list.Count ? index : list.Count - 1;
        }

        public int TableIndexOf(DataField col)
        {
            for (int i = 0; i < _tabs.Count; i++)
            {
                if (_tabs[i].Name == col.Name) return i;
            }
            return -1;
        }

        public int TableIndexOf(string name)
        {
            for (int i = 0; i < _tabs.Count; i++)
            {
                if (_tabs[i].Name == name) return i;
            }
            return -1;
        }

        public DataBlock GetTable(string name)
        {
            foreach (DataBlock tab in _tabs)
            {
                if (tab.Name == name) return tab;
            }
            return null;
        }
    }
}