using System;

namespace Jetproger.Tools.File.Jsons
{
    [Serializable]
    public class JsonSet : MarshalByRefObject
    {
        private JsonTable[] _tables;

        public JsonSet()
        {
        }

        public JsonSet(JsonTable[] tables)
        {
            _tables = tables;
        }

        public JsonTable[] Tables
        {
            get { return _tables; }
            set { _tables = value; }
        }

        public JsonSet Copy()
        {
            if (_tables == null)
            {
                return new JsonSet();
            }
            if (_tables.Length == 0)
            {
                return new JsonSet(new JsonTable[0]);
            }
            var newTables = new JsonTable[_tables.Length];
            var i = 0;
            foreach (var table in _tables)
            {
                var newTable = table.Copy();
                newTables[i++] = newTable;
            }
            return new JsonSet(newTables);
        }
    }
}