using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Structure.Bases
{
    [Serializable]
    [DataContract]
    public class User : IEntityId
    {
        private Guid _id;
        private string _code;
        private string _name;
        private string _note;
        private string _password;
        private bool _isSystem;
        private bool _isAdmin;
        private List<Role> _roles;

        public User()
        {
            _id = Guid.NewGuid();
            _roles = new List<Role>();
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
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        [DataMember]
        public bool IsSystem
        {
            get { return _isSystem; }
            set { _isSystem = value; }
        }

        [DataMember]
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { _isAdmin = value; }
        }

        [DataMember]
        public Role[] Roles
        {
            get { return _roles.ToArray(); }
            set { _roles = new List<Role>(value ?? new Role[0]); }
        }

        public override string ToString()
        {
            return _code;
        }

        public Guid GetEntityId()
        {
            return _id;
        }

        public void Add(Role item)
        {
            if (IndexOf(item) == -1) _roles.Add(item);
        }

        public void Remove(Role item)
        {
            RemoveAt(IndexOf(item));
        }

        public void Remove(Guid id)
        {
            RemoveAt(IndexOf(id));
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < _roles.Count) _roles.RemoveAt(index);
        }

        public int IndexOf(Role item)
        {
            var i = 0;
            foreach (var role in (_roles ?? new List<Role>()))
            {
                if (ReferenceEquals(item, role)) return i;
                i++;
            }
            return -1;
        }

        public int IndexOf(Guid id)
        {
            var i = 0;
            foreach (var role in (_roles ?? new List<Role>()))
            {
                if (role.Id == _id) return i;
                i++;
            }
            return -1;
        }
    }
}