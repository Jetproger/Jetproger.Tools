using System;
using System.Runtime.Serialization; 
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class UserRole : CommandEntity
    {
        [DataMember] public Guid Id { get; set; }
        [DataMember] public Guid UserId { get; set; }
        [DataMember] public Guid RoleId { get; set; }
    }
}