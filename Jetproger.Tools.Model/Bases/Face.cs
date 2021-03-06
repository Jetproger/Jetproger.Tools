﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Model.Bases
{
    [Serializable]
    [DataContract]
    public class Face : IEntityId
    {
        private Guid _id;
        private string _code;
        private string _name;
        private string _note;
        private string _permission;
        private List<FaceItem> _items;

        public Face()
        {
            _id = Guid.NewGuid();
            _items = new List<FaceItem>();
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
        public FaceItem[] Items
        {
            get { return _items.ToArray(); }
            set { _items = new List<FaceItem>(value ?? new FaceItem[0]); }
        }

        public override string ToString()
        {
            return _code;
        }

        public Guid GetEntityId()
        {
            return _id;
        }

        public void Add(FaceItem item)
        {
            if (IndexOf(item) == -1) _items.Add(item);
        }

        public void Remove(FaceItem item)
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

        public int IndexOf(FaceItem item)
        {
            var i = 0;
            foreach (var faceItem in (_items ?? new List<FaceItem>()))
            {
                if (ReferenceEquals(item, faceItem)) return i;
                i++;
            }
            return -1;
        }

        public int IndexOf(Guid id)
        {
            var i = 0;
            foreach (var faceItem in (_items ?? new List<FaceItem>()))
            {
                if (faceItem.Id == _id) return i;
                i++;
            }
            return -1;
        }

        public IEnumerable<FaceItem> AllItems()
        {
            foreach (var x in (_items ?? new List<FaceItem>()))
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