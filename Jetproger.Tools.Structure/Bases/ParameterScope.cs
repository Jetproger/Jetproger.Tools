using System;

namespace Jetproger.Tools.Structure.Bases
{
    [Serializable]
    public enum ParameterScope
    {
        Configuration = 1,

        Constant = 0,

        Context = 2,

        Previous = 3
    }
}