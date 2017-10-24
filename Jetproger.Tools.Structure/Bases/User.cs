using System;
using System.Collections.Generic;

namespace Jetproger.Tools.Structure.Bases
{
    [Serializable]
    public class User : MarshalByRefObject
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsSystem { get; set; }
        public bool IsAdmin { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
        public List<Role> Roles;
        public List<PermissionRole> Permissions;

        public User()
        {
            Roles = new List<Role>();
            Permissions = new List<PermissionRole>();
        }

        public override string ToString()
        {
            return Code;
        }
    }
}