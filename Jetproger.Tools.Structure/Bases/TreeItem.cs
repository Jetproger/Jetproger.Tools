using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Structure.Bases
{
    [Serializable]
    [DataContract]
    public class TreeItem : IEntityId, IParentId, IDocumentId
    {
        private Guid _id;
        private Guid _treeId;
        private Guid _taskId;
        private Guid _parentId;
        private string _code;
        private string _name;
        private string _note;
        private string _icon;
        private string _spec;
        private string _loop;
        private int _ordinal;
        private bool _aggregator;
        private string _shortcut;
        private string _location;
        private string _permission;
        private List<TreeItem> _items;

        public TreeItem()
        {
            _id = Guid.NewGuid();
            _items = new List<TreeItem>();
        }

        [DataMember]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [DataMember]
        public Guid TreeId
        {
            get { return _treeId; }
            set { _treeId = value; }
        }

        [DataMember]
        public Guid TaskId
        {
            get { return _taskId; }
            set { _taskId = value; }
        }

        [DataMember]
        public Guid ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
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
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        [DataMember]
        public string Spec
        {
            get { return _spec; }
            set { _spec = value; }
        }

        [DataMember]
        public string Loop
        {
            get { return _loop; }
            set { _loop = value; }
        }

        [DataMember]
        public int Ordinal
        {
            get { return _ordinal; }
            set { _ordinal = value; }
        }

        [DataMember]
        public bool Aggregator
        {
            get { return _aggregator; }
            set { _aggregator = value; }
        }

        [DataMember]
        public string Shortcut
        {
            get { return _shortcut; }
            set { _shortcut = value; }
        }

        [DataMember]
        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        [DataMember]
        public string Permission
        {
            get { return _permission; }
            set { _permission = value; }
        }

        [DataMember]
        public TreeItem[] Items
        {
            get { return _items.ToArray(); }
            set { _items = new List<TreeItem>(value ?? new TreeItem[0]); }
        }

        public override string ToString()
        {
            return $"{_treeId}-{_code}";
        }

        public Guid GetDocumentId()
        {
            return _treeId;
        }

        public Guid GetParentId()
        {
            return _parentId;
        }

        public Guid GetEntityId()
        {
            return _id;
        }

        public void Add(TreeItem item)
        {
            if (!ReferenceEquals(this, item) && IndexOf(item) == -1) _items.Add(item);
        }

        public void Remove(TreeItem item)
        {
            RemoveAt(IndexOf(item));
        }

        public void Remove(Guid id)
        {
            RemoveAt(IndexOf(id));
        }

        public void RemoveAt(int index)
        {
            if (index >= 0 && index < _items.Count) _items.RemoveAt(index);
        }

        public int IndexOf(TreeItem item)
        {
            var i = 0;
            foreach (var treeItem in (_items ?? new List<TreeItem>()))
            {
                if (ReferenceEquals(item, treeItem)) return i;
                i++;
            }
            return -1;
        }

        public int IndexOf(Guid id)
        {
            var i = 0;
            foreach (var treeItem in (_items ?? new List<TreeItem>()))
            {
                if (treeItem.Id == _id) return i;
                i++;
            }
            return -1;
        }

        public IEnumerable<TreeItem> AllItems()
        {
            foreach (var x in (_items ?? new List<TreeItem>()))
            {
                yield return x;
                foreach (var y in x.AllItems())
                {
                    yield return y;
                }
            }
        }
    }
}