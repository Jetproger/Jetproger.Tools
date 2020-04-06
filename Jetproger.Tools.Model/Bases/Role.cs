using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class Role : IEntityId
    {
        private Guid _id;
        private string _code;
        private string _name;
        private string _note;
        private bool _isSystem;
        private List<Permission> _permissions;

        public Role()
        {
            _id = Guid.NewGuid();
            _permissions = new List<Permission>();
        }

        [DataMember]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
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
        public bool IsSystem
        {
            get { return _isSystem; }
            set { _isSystem = value; }
        }

        [DataMember]
        public Permission[] Permissions
        {
            get { return _permissions.ToArray(); }
            set { _permissions = new List<Permission>(value ?? new Permission[0]); }
        }

        public override string ToString()
        {
            return _code;
        }

        public Guid GetEntityId()
        {
            return _id;
        }

        public void Add(Permission item)
        {
            if (IndexOf(item) == -1) _permissions.Add(item);
        }

        public void Remove(Permission item)
        {
            RemoveAt(IndexOf(item));
        }

        public void Remove(Guid id)
        {
            RemoveAt(IndexOf(id));
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < _permissions.Count) _permissions.RemoveAt(index);
        }

        public int IndexOf(Permission item)
        {
            var i = 0;
            foreach (var permission in (_permissions ?? new List<Permission>()))
            {
                if (ReferenceEquals(item, permission)) return i;
                i++;
            }
            return -1;
        }

        public int IndexOf(Guid id)
        {
            var i = 0;
            foreach (var permission in (_permissions ?? new List<Permission>()))
            {
                if (permission.Id == _id) return i;
                i++;
            }
            return -1;
        }
    }
}