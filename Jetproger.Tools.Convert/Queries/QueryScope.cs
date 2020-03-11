using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Jc
{
    [Serializable]
    [DataContract]
    public class QueryScope
    {
        private readonly List<QueryWhere> _items = new List<QueryWhere>();
        private int _currentIndex;

        [DataMember]
        public QueryWhere[] Items
        {
            get { return _items.ToArray(); }
            set { SetItems(value); }
        }

        private void SetItems(QueryWhere[] values)
        {
            _items.Clear();
            if (values != null) _items.AddRange(values);
        }

        public QueryScope Where(int i)
        {
            _currentIndex = i >= 0 ? i : 0;
            while (_currentIndex >= _items.Count) _items.Add(new QueryWhere());
            return this;
        }

        public QueryScope And<TInstance, TMember>(Expression<Func<TInstance, TMember>> expression, QueryOp op, TMember sample)
        {
            _items[_currentIndex].And(expression, op, sample);
            return this;
        }

        public QueryScope Or<TInstance, TMember>(Expression<Func<TInstance, TMember>> expression, QueryOp op, TMember sample)
        {
            _items[_currentIndex].Or(expression, op, sample);
            return this;
        }
    }

    public static class QueryScopes
    {  
        public static QueryScope Where(int i)
        {
            return (new QueryScope()).Where(i);
        }
    }
}