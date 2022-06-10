using System; 

namespace Jetproger.Tools.Convert.Settings
{
    public abstract class Setting : MarshalByRefObject
    {
        public bool IsDeclared { get; protected set; }
        public string Value { get; protected set; }
    }
}