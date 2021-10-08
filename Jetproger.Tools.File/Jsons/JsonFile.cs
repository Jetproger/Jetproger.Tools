using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Files = Tools.File;

namespace Jetproger.Tools.File.Jsons
{
    [Serializable]
    public class JsonFile : MarshalByRefObject, IList
    {
        private readonly ConcurrentDictionary<Guid, object> _items;
        private readonly Type _entityType;
        private readonly string _fileName;
        private DateTime _timestamp;
        private bool _isModified;

        public JsonFile(Type entityType)
        {
            _entityType = entityType;
            _fileName = GetFileName(_entityType);
            _timestamp = DateTime.UtcNow;
            SyncRoot = new object();
            _items = Read();
        }

        public bool IsModified
        {
            get { return _isModified; }
            set
            {
                _isModified = value;
                if (_isModified) _timestamp = DateTime.UtcNow;
            }
        }

        public void Write()
        {
            if (!_isModified || (DateTime.UtcNow - _timestamp).TotalMilliseconds <= 3333) return;
            Files.ToJson(_items.Values, _fileName);
            _isModified = false;
        }

        private ConcurrentDictionary<Guid, object> Read()
        {
            Type generic = typeof(List<>);
            var type = generic.MakeGenericType(_entityType);
            var items = ((Files.OfJson(_fileName, type) ?? Activator.CreateInstance(type)) as IEnumerable) ?? new object[0];
            var data = new ConcurrentDictionary<Guid, object>();
            foreach (var item in items) data.TryAdd(Guid.NewGuid(), item);
            return data;
        }

        private static string GetFileName(Type type)
        {
            var name = type.Name;
            var sb = new StringBuilder();
            foreach (var currentChar in name)
            {
                var newChar = char.IsLower(currentChar) ? currentChar : char.ToLower(currentChar);
                if (sb.Length > 0 && char.IsUpper(currentChar)) sb.Append("-");
                sb.Append(newChar);
            }
            return Path.Combine(Files.FileNameAsPath(null), sb + ".json");
        }

        #region IList Implemented

        public int Count
        {
            get { return _items.Count; }
        }

        public IEnumerator GetEnumerator()
        {
            return GetItems().GetEnumerator();
        }

        public bool Contains(object value)
        {
            foreach (var pair in _items)
            {
                if (ReferenceEquals(pair.Value, value)) return true;
            }
            return false;
        }

        public void Clear()
        {
            _items.Clear();
        }

        public int Add(object value)
        {
            _items.TryAdd(Guid.NewGuid(), value);
            return 0;
        }

        public void Remove(object value)
        {
            var key = (Guid?)null;
            foreach (var pair in _items)
            {
                if (!ReferenceEquals(pair.Value, value)) continue;
                key = pair.Key;
                break;
            }
            if (key != null)
            {
                object item;
                _items.TryRemove(key.Value, out item);
            }
        }

        #endregion

        #region IList Not implemented

        public bool IsReadOnly => false;

        public bool IsFixedSize => false;

        public bool IsSynchronized => false;

        public object SyncRoot { get; }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public object this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        public IEnumerable<object> GetItems()
        {
            foreach (var pair in _items)
            {
                yield return pair.Value;
            }
        }
    }
}