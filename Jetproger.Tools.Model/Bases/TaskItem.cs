using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class TaskItem : IEntityId, IDocumentId
    {
        private Guid _id;
        private Guid _taskId;
        private string _code;
        private string _value;
        private string _name;
        private string _note;
        private ParameterScope _scope;

        public TaskItem()
        {
            _id = Guid.NewGuid();
        }

        [DataMember]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [DataMember]
        public Guid TaskId
        {
            get { return _taskId; }
            set { _taskId = value; }
        }

        [DataMember]
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        [DataMember]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [DataMember]
        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }

        public ParameterScope Scope
        {
            get { return _scope; }
            set { _scope = value; }
        }

        [DataMember]
        public string ScopeName
        {
            get
            {
                switch (_scope)
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
                    case "config": _scope = ParameterScope.Configuration; break;
                    case "context": _scope = ParameterScope.Context; break;
                    case "prev": _scope = ParameterScope.Previous; break;
                    default: _scope = ParameterScope.Constant; break;
                }
            }
        }

        public override string ToString()
        {
            return $"{_taskId}-{_code}";
        }

        public Guid GetDocumentId()
        {
            return _taskId;
        }

        public Guid GetEntityId()
        {
            return _id;
        }
    }
}