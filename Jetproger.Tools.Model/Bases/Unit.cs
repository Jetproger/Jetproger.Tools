using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class Unit : IEntityId
    {
        private Guid _id;
        private string _code;
        private string _name;
        private string _note;
        private string _permission;
        private List<UnitItem> _items;

        public Unit()
        {
            _id = Guid.NewGuid();
            _items = new List<UnitItem>();
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
        public string Permission
        {
            get { return _permission; }
            set { _permission = value; }
        }

        [DataMember]
        public UnitItem[] Items
        {
            get { return _items.ToArray(); }
            set { _items = new List<UnitItem>(value ?? new UnitItem[0]); }
        }

        public override string ToString()
        {
            return _code;
        }

        public Guid GetEntityId()
        {
            return _id;
        }

        public void Add(UnitItem item)
        {
            if (IndexOf(item) == -1) _items.Add(item);
        }

        public void Remove(UnitItem item)
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

        public int IndexOf(UnitItem item)
        {
            var i = 0;
            foreach (var unitItem in (_items ?? new List<UnitItem>()))
            {
                if (ReferenceEquals(item, unitItem)) return i;
                i++;
            }
            return -1;
        }

        public int IndexOf(Guid id)
        {
            var i = 0;
            foreach (var unitItem in (_items ?? new List<UnitItem>()))
            {
                if (unitItem.Id == _id) return i;
                i++;
            }
            return -1;
        }
    }
}