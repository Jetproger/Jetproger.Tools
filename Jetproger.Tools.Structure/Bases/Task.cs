using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Structure.Bases
{
    [Serializable]
    [DataContract]
    public class Task : IEntityId
    {
        private Guid _id;
        private string _code;
        private string _name;
        private string _note;
        private string _permission;
        private string _assemblyName;
        private string _typeName;
        private bool _isolate;
        private bool _remote;
        private List<TaskItem> _items;

        public Task()
        {
            _items = new List<TaskItem>();
            _id = Guid.NewGuid();
            _isolate = true;
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
        public string AssemblyName
        {
            get { return _assemblyName; }
            set { _assemblyName = value; }
        }

        [DataMember]
        public string TypeName
        {
            get { return _typeName; }
            set { _typeName = value; }
        }

        [DataMember]
        public bool Isolate
        {
            get { return _isolate; }
            set { _isolate = value; }
        }

        [DataMember]
        public bool Remote
        {
            get { return _remote; }
            set { _remote = value; }
        }

        [DataMember]
        public TaskItem[] Items
        {
            get { return _items.ToArray(); }
            set { _items = new List<TaskItem>(value ?? new TaskItem[0]); }
        }

        public override string ToString()
        {
            return _code;
        }

        public Guid GetEntityId()
        {
            return _id;
        }

        public void Add(TaskItem item)
        {
            if (IndexOf(item) == -1) _items.Add(item);
        }

        public void Remove(TaskItem item)
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

        public int IndexOf(TaskItem item)
        {
            var i = 0;
            foreach (var taskItem in (_items ?? new List<TaskItem>()))
            {
                if (ReferenceEquals(item, taskItem)) return i;
                i++;
            }
            return -1;
        }

        public int IndexOf(Guid id)
        {
            var i = 0;
            foreach (var taskItem in (_items ?? new List<TaskItem>()))
            {
                if (taskItem.Id == _id) return i;
                i++;
            }
            return -1;
        }
    }
}