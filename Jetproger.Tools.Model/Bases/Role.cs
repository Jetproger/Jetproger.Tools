using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class Role : CommandEntity
    { 
        [DataMember] public Guid Id { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Note { get; set; }
        [DataMember] public bool IsSystem { get; set; }
        [DataMember] public Flag[] Flags { get; set; }
    }
}