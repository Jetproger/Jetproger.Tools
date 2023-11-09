using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class UnitItem : CommandEntity
    {
        [DataMember] public Guid Id { get; set; }
        [DataMember] public Guid UnitId { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Note { get; set; }
        [DataMember] public string Flag { get; set; }
        [DataMember] public bool Unique { get; set; }
        [DataMember] public bool Index { get; set; }
        [DataMember] public bool Null { get; set; }
        [DataMember] public int Length { get; set; }
        [DataMember] public CustomType Type { get; set; }
    }
}