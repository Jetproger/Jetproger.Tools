using System;

namespace Jetproger.Tools.Structure.Bases
{
    [Serializable]
    public enum CustomType
    {
        Guid = 0,
        String = 1,
        Integer = 2,
        Numeric = 3,
        Date = 4,
        Boolean = 5,
        Binary = 6,
        Range = 7,
        Reference = 8,
        List = 9
    }
}