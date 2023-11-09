using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class FlagRole : CommandEntity
    {
        [DataMember] public Guid Id { get; set; }
        [DataMember] public Guid FlagId { get; set; }
        [DataMember] public Guid RoleId { get; set; } 
    }
}