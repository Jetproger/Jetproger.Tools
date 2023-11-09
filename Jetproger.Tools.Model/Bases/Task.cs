using System; 
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class Task : CommandEntity
    { 
        [DataMember] public Guid Id { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Note { get; set; }
        [DataMember] public string Flag { get; set; }
        [DataMember] public string Assembly { get; set; }
        [DataMember] public string Type { get; set; }
        [DataMember] public bool Isolate { get; set; }
        [DataMember] public bool Remote { get; set; }
        [DataMember] public TaskItem[] Items { get; set; }
    }
}