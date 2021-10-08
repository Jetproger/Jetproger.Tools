using System;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class PermissionRole : IEntityId, IParentId, IDocumentId
    {
        private Guid _id;
        private Guid _permissionId;
        private Guid _roleId;
        private bool _access;

        public PermissionRole()
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
        public Guid PermissionId
        {
            get { return _permissionId; }
            set { _permissionId = value; }
        }

        [DataMember]
        public Guid RoleId
        {
            get { return _roleId; }
            set { _roleId = value; }
        }

        [DataMember]
        public bool Access
        {
            get { return _access; }
            set { _access = value; }
        }

        public override string ToString()
        {
            return $"{_permissionId}-{_roleId}";
        }

        public Guid GetDocumentId()
        {
            return _permissionId;
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