using System;
using System.Collections;

namespace Jetproger.Tools.File.Jsons
{
    [Serializable]
    public class JsonFilter
    {
        private JsonWhere _where;
        private JsonFrom _from;

        public JsonFilter() { }

        public JsonFilter(JsonFrom from)
        {
            _from = from;
        }

        public JsonFilter(JsonWhere where)
        {
            _where = where;
        }

        public JsonWhere Where
        {
            get
            {
                return _where;
            }
            set
            {
                _where = value;
                if (_where != null) _from = null;
            }
        }

        public JsonFrom From
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
                if (_from != null) _where = null;
            }
        }

        public IList Take(IList items)
        {
            return _from != null ? _from.Take(items) : (_where != null ? _where.Take(items) : items);
        }

        public IEnumerable Apply(IEnumerable items)
        {
            return _from != null ? _from.Apply(items) : (_where != null ? _where.Apply(items) : items);
        }
    }
}