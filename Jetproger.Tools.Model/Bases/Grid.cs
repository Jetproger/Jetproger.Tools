using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class Grid : CommandEntity
    {
        [DataMember] public Guid Id { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Note { get; set; }
        [DataMember] public string Renderer { get; set; }
        [DataMember] public string Permission { get; set; }
        [DataMember] public GridItem[] Items { get; set; }
    }
}