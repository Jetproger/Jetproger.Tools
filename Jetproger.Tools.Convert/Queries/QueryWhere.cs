using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Jc
{
    [Serializable]
    [DataContract]
    public class QueryWhere
    {
        private readonly List<QueryOr> _items = new List<QueryOr>();

        [DataMember]
        public QueryOr[] Items
        {
            get { return _items.ToArray(); }
            set { SetItems(value); }
        }

        private void SetItems(QueryOr[] values)
        {
            _items.Clear();
            if (values != null) _items.AddRange(values);
        }

        public QueryWhere And<TInstance, TMember>(Expression<Func<TInstance, TMember>> expression, QueryOp op, TMember sample)
        {
            var itemOr = _items.Count > 0 ? _items[_items.Count - 1] : null;
            if (itemOr == null)
            {
                itemOr = new QueryOr();
                _items.Add(itemOr);
            }
            itemOr.And(expression, op, sample);
            return this;
        }

        public QueryWhere Or<TInstance, TMember>(Expression<Func<TInstance, TMember>> expression, QueryOp op, TMember sample)
        {
            var itemOr = new QueryOr();
            _items.Add(itemOr);
            itemOr.And(expression, op, sample);
            return this;
        }

        public DataBlock Select(DataBlock table)
        {
            var newTable = new DataBlock();
            foreach (DataField col in table.Cols)
            {
                newTable.AddColumn(col.Name, col.Style);
            }
            foreach (DataTuple row in table.Rows)
            {
                if (IsMatch(row)) newTable.AddRow(row);
            }
            return newTable;
        }

        public void ResolveField(DataField[] cols)
        {
            foreach (QueryOr item in _items)
            {
                item.ResolveField(cols);
            }                                                               ;
        }

        public bool IsMatch(DataTuple row)
        {
            return _items.Any(x => x.IsMatch(row));
        }
    }
}