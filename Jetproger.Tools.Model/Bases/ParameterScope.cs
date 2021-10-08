using System;

namespace Jetproger.Tools.Model.Bases
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