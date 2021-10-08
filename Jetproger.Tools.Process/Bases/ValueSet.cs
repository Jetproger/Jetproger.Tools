using System;

namespace Jetproger.Tools.Process.Bases
{
    public class ValueSet : MarshalByRefObject
    {
        private ValueTable[] _tables;

        public ValueTable[] Tables
        {
            get { return _tables; }
            set { _tables = value; }
        }
    }
}