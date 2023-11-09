using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Commanders;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class TaskItem : CommandEntity
    {
        [DataMember] public Guid Id { get; set; }
        [DataMember] public Guid TaskId { get; set; }
        [DataMember] public string Code { get; set; }
        [DataMember] public string Value { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Note { get; set; }
        public ParameterScope Scope { get; set; }

        [DataMember]
        public string ScopeName
        {
            get
            {
                switch (Scope)
                {
                    case ParameterScope.Configuration: return "config";
                    case ParameterScope.Context: return "context";
                    case ParameterScope.Previous: return "prev";
                    default: return "const";
                }
            }
            set
            {
                switch (value)
                {
                    case "config": Scope = ParameterScope.Configuration; break;
                    case "context": Scope = ParameterScope.Context; break;
                    case "prev": Scope = ParameterScope.Previous; break;
                    default: Scope = ParameterScope.Constant; break;
                }
            }
        }
    }
}