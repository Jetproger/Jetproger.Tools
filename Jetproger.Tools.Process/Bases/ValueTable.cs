using System;

namespace Jetproger.Tools.Process.Bases
{
    public class ValueTable : MarshalByRefObject
    {
        private string[] _columns;
        private Type[] _types;
        private object[][] _rows;

        public string[] Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        public Type[] Types
        {
            get { return _types; }
            set { _types = value; }
        }

        public object[][] Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }
    }
}