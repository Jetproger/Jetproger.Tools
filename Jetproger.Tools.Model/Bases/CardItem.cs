using System;  
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class CardItem : CommandEntity
    {
        [DataMember] public Guid Id { get; set; }
        [DataMember] public Guid ParentId { get; set; }
        [DataMember] public Guid CardId { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Note { get; set; }
        [DataMember] public string Flag { get; set; }
        [DataMember] public CardItem[] Items { get; set; } 
    }
}