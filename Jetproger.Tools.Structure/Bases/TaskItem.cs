using System;

namespace Jetproger.Tools.Structure.Bases
{
    [Serializable]
    public class TaskItem : MarshalByRefObject
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public ParameterScope Scope { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string TaskCode { get; set; }
        public Task Task { get; set; }
        public override string ToString()
        {
            return $"{TaskCode}&{Code}";
        }

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