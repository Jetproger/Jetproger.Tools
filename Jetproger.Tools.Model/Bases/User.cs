using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class User : CommandEntity
    {
        [DataMember] public Guid Id { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Note { get; set; }
        [DataMember] public string Password { get; set; }
        [DataMember] public bool IsSystem { get; set; }
        [DataMember] public bool IsAdmin { get; set; }
        [DataMember] public Role[] Roles { get; set; }
    }
}