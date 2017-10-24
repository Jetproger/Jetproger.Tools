using System;
using System.Collections.Generic;

namespace Jetproger.Tools.Structure.Bases
{
    [Serializable]
    public class Permission : MarshalByRefObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public List<PermissionRole> Items { get; set; }

        public override string ToString()
        {
            return Code;
        }
    }
}