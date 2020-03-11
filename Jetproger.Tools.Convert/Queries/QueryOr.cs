using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Bases;

namespace Jc
{
    [Serializable]
    [DataContract]
    public class QueryOr
    {
        private readonly List<QueryAnd> _items = new List<QueryAnd>();
        [DataMember]
        public QueryAnd[] Items
        {
            get { return _items.ToArray(); }
            set { SetItems(value); }
        }

        private void SetItems(QueryAnd[] values)
        {
            _items.Clear();
            if (values != null) _items.AddRange(values);
        }

        public void ResolveField(DataField[] cols)
        {
            foreach (QueryAnd item in _items)
            {
                item.ResolveField(cols);
            }
        }

        public bool IsMatch(DataTuple row)
        {
            return _items.All(x => x.IsMatch(row));
        }

        public void And<TInstance, TMember>(Expression<Func<TInstance, TMember>> expression, QueryOp op, TMember sample)
        {
            _items.Add(new QueryAnd  {
                Name = Je.Meta.GetMemberName(expression),
                Operator = op,
                Sample = sample
            });
        }
    }
}