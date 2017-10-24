using System;

namespace Jetproger.Tools.Structure.Bases
{
    [Serializable]
    public class UserRole : MarshalByRefObject
    {
        public string UserCode { get; set; }
        public string RoleCode { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }

        public override string ToString()
        {
            return $"{UserCode}\n{RoleCode}";
        }
    }
}