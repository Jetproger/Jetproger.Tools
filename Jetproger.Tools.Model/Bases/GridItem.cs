using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class GridItem : IEntityId, IDocumentId
    {
        private Guid _id;
        private Guid _gridId;
        private string _code;
        private string _name;
        private string _note;
        private string _category;
        private string _beforechange;
        private string _afterchange;
        private CustomType _type;
        private int _ordinal;
        private int _width;
        private int _colspan;
        private bool _visible;
        private bool _readonly;
        private bool _null;
        private bool _stored;
        private string _permission;

        public GridItem()
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
        public Guid GridId
        {
            get { return _gridId; }
            set { _gridId = value; }
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
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        [DataMember]
        public string Beforechange
        {
            get { return _beforechange; }
            set { _beforechange = value; }
        }

        [DataMember]
        public string Afterchange
        {
            get { return _afterchange; }
            set { _afterchange = value; }
        }

        [DataMember]
        public CustomType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        [DataMember]
        public int Ordinal
        {
            get { return _ordinal; }
            set { _ordinal = value; }
        }

        [DataMember]
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        [DataMember]
        public int Colspan
        {
            get { return _colspan; }
            set { _colspan = value; }
        }

        [DataMember]
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        [DataMember]
        public bool Readonly
        {
            get { return _readonly; }
            set { _readonly = value; }
        }

        [DataMember]
        public bool Null
        {
            get { return _null; }
            set { _null = value; }
        }

        [DataMember]
        public bool Stored
        {
            get { return _stored; }
            set { _stored = value; }
        }

        [DataMember]
        public string Permission
        {
            get { return _permission; }
            set { _permission = value; }
        }

        public override string ToString()
        {
            return $"{_gridId}-{_code}";
        }

        public Guid GetDocumentId()
        {
            return _gridId;
        }

        public Guid GetEntityId()
        {
            return _id;
        }
    }
}