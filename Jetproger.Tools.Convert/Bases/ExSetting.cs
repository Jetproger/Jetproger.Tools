using System;
using System.Runtime.Serialization;

namespace Jetproger.Tools.Convert.Bases
{
    [Serializable]
    [DataContract]
    public abstract class ExSetting
    {
        private bool _isValid;
        private bool _isDeclare;
        private string _code;
        private string _name;
        private string _description;
        private string _shortcuts;
        private string _specName;
        private byte[] _picture;
        private string _assemblyName;

        protected ExSetting(string assemblyName)
        {
            _assemblyName = !string.IsNullOrWhiteSpace(assemblyName) ? _assemblyName : "Jetproger.Tools.Resource";
            _code = GetType().Name.Replace("ExSetting", string.Empty).Replace("Setting", string.Empty);
            _name = _code;
            _description = _code;
            _specName = _code;
            _isValid = true;
        }

        [DataMember]
        public bool IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
        }

        [DataMember]
        public bool IsDeclare
        {
            get { return _isDeclare; }
            set { _isDeclare = value; }
        }

        [DataMember]
        public string AssemblyName
        {
            get { return _assemblyName; }
            set { _assemblyName = value; }
        }

        [DataMember]
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [DataMember]
        public string SpecName
        {
            get { return _specName; }
            set { _specName = value; }
        }

        [DataMember]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [DataMember]
        public string Shortcut
        {
            get { return _shortcuts; }
            set { _shortcuts = value; }
        }

        [DataMember]
        public byte[] Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        public virtual void Validate() { }
    }
}