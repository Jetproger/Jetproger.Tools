using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jc
{
    [Serializable]
    [DataContract]
    public class DataTuple: IDataTuple
    {
        private readonly List<object> _values = new List<object>();

        [XmlIgnore]
        [ScriptIgnore]
        public object[] Values
        {
            get { return _values.ToArray(); }
            set { SetValues(value); }
        }

        [DataMember]
        public string[] Labels
        {
            get { return GetStringRow(_values); }
            set { SetValues(value); }
        }

        private void SetValues(object[] values)
        {
            _values.Clear();
            if (values != null) _values.AddRange(values);
        }

        private void SetValues(string[] values)
        {
            _values.Clear();
            if (values != null) _values.AddRange(values);
        }

        private static string[] GetStringRow(List<object> objectRow)
        {
            if (objectRow == null) return null;
            var stringRow = new string[objectRow.Count];
            for (int i = 0; i < objectRow.Count; i++) stringRow[i] = Je.Txt.Of(objectRow[i]);
            return stringRow;
        }

        object IDataTuple.Get(int index)
        {
            index = ResolveIndex(index);
            return index >= 0 ? _values[index] : null;
        }

        void IDataTuple.Set(object value)
        {
            _values.Add(value);
        }

        void IDataTuple.Set(int index, object value)
        {
            index = ResolveIndex(index);
            if (index >= 0) _values[index] = value;
        }

        void IDataTuple.RemoveAt(int index)
        {
            index = ResolveIndex(index);
            if (index >= 0) _values.RemoveAt(index);
        }

        private int ResolveIndex(int index)
        {
            index = index >= 0 ? index : 0;
            return index < _values.Count ? index : _values.Count - 1;
        }
    }

    public interface IDataTuple
    {
        object Get(int index);
        void RemoveAt(int index);

        void Set(object value);
        void Set(int index, object value);
    }
}