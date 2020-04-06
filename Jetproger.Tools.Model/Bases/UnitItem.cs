using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class UnitItem : IEntityId, IDocumentId
    {
        private Guid _id;
        private Guid _unitId;
        private string _code;
        private string _name;
        private string _note;
        private string _permission;
        private bool _unique;
        private bool _index;
        private bool _null;
        private int _length;
        private CustomType _type;

        public UnitItem()
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
        public Guid UnitId
        {
            get { return _unitId; }
            set { _unitId = value; }
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
        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }

        [DataMember]
        public string Permission
        {
            get { return _permission; }
            set { _permission = value; }
        }

        [DataMember]
        public bool Unique
        {
            get { return _unique; }
            set { _unique = value; }
        }

        [DataMember]
        public bool Index
        {
            get { return _index; }
            set { _index = value; }
        }

        [DataMember]
        public bool Null
        {
            get { return _null; }
            set { _null = value; }
        }

        [DataMember]
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        [DataMember]
        public CustomType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public override string ToString()
        {
            return $"{_unitId}-{_code}";
        }

        public Guid GetDocumentId()
        {
            return _unitId;
        }

        public Guid GetEntityId()
        {
            return _id;
        }
    }
}