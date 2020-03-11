using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Converts;

namespace Jc
{
    [Serializable]
    [DataContract]
    public class QueryData
    {
        private readonly List<QueryDataPair> _items = new List<QueryDataPair>();

        [DataMember]
        public QueryDataPair[] Items
        {
            get { return _items.ToArray(); }
            set { SetItems(value); }
        }

        private void SetItems(QueryDataPair[] values)
        {
            _items.Clear();
            if (values != null) _items.AddRange(values);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        public void Add(object obj, QueryScope query)
        {
            var data = obj.As<DataScope>();
            Add(data, query);
        }

        public void Add(DataScope data, QueryScope query)
        {
            _items.Add(new QueryDataPair { Data = data, Query = query });
        }
    }

    [Serializable]
    [DataContract]
    public class QueryDataPair
    {

        [DataMember]
        public DataScope Data { get; set; }

        [DataMember]
        public QueryScope Query { get; set; }
    }
}