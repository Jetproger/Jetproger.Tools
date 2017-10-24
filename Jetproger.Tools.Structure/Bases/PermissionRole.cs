using System;

namespace Jetproger.Tools.Structure.Bases
{
    [Serializable]
    public class PermissionRole : MarshalByRefObject
    {
        public string PermissionCode { get; set; }
        public string RoleCode { get; set; }
        public bool Access { get; set; }
        public Permission Permission { get; set; }
        public Role Role { get; set; }

        public override string ToString()
        {
            return $"{PermissionCode}\n{RoleCode}";
        }
    }
}