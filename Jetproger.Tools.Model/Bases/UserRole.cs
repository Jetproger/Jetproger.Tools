using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class UserRole : IEntityId, IParentId, IDocumentId
    {
        private Guid _id;
        private Guid _userId;
        private Guid _roleId;

        public UserRole()
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
        public Guid UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        [DataMember]
        public Guid RoleId
        {
            get { return _roleId; }
            set { _roleId = value; }
        }

        public override string ToString()
        {
            return $"{_userId}-{_roleId}";
        }

        public Guid GetDocumentId()
        {
            return _userId;
        }

        public Guid GetParentId()
        {
            return _roleId;
        }

        public Guid GetEntityId()
        {
            return _id;
        }
    }
}