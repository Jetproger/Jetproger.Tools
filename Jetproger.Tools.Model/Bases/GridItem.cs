using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class GridItem : CommandEntity
    { 
        [DataMember] public Guid Id { get; set; }
        [DataMember] public Guid GridId { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Note { get; set; }
        [DataMember] public string Category { get; set; }
        [DataMember] public string Beforechange { get; set; }
        [DataMember] public string Afterchange { get; set; }
        [DataMember] public CustomType Type { get; set; }
        [DataMember] public int Ordinal { get; set; }
        [DataMember] public int Width { get; set; }
        [DataMember] public int Colspan { get; set; }
        [DataMember] public bool Visible { get; set; }
        [DataMember] public bool Readonly { get; set; }
        [DataMember] public bool Null { get; set; }
        [DataMember] public bool Stored { get; set; }
        [DataMember] public string Flag { get; set; }
    }
}