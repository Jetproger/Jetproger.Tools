using System;
using System.Collections.Generic;

namespace Jetproger.Tools.Structure.Bases
{
    [Serializable]
    public class Role : MarshalByRefObject
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsSystem { get; set; }
        public string Code { get; set; }
        public List<UserRole> Users { get; set; }
        public List<PermissionRole> Premissions { get; set; }

        public Role()
        {
            Premissions = new List<PermissionRole>();
            Users = new List<UserRole>();
            Code = Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return Code;
        }
    }
}