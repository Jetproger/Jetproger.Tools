using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class TreeItem : CommandEntity
    {
        [DataMember] public Guid Id { get; set; }
        [DataMember] public Guid TreeId { get; set; }
        [DataMember] public Guid TaskId { get; set; }
        [DataMember] public Guid ParentId { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Note { get; set; }
        [DataMember] public string Icon { get; set; }
        [DataMember] public string Spec { get; set; }
        [DataMember] public string Loop { get; set; }
        [DataMember] public int Ordinal { get; set; }
        [DataMember] public bool Aggregator { get; set; }
        [DataMember] public string Shortcut { get; set; }
        [DataMember] public string Location { get; set; }
        [DataMember] public string Falg { get; set; }
        [DataMember] public TreeItem[] Items { get; set; }
    }
}