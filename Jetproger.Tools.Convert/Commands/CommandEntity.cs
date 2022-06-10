using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commands
{
    public class EntityReader : IDataReader
    {
        object IDataRecord.this[string name] { get { return GetValue(GetOrdinal(name)); } }
        public int FieldCount { get { return _fields != null ? _fields.Length : 1; } }
        object IDataRecord.this[int i] { get { return GetValue(i); } }
        public FieldSizer Sizer { get { return _fieldSizer; } }
        public DataTable GetSchemaTable() { return null; }
        public int RecordsAffected { get { return 0; } }
        public bool IsClosed { get { return false; } }
        public int Depth { get { return 0; } }
        private readonly FieldSizer _fieldSizer;
        private readonly List<object> _source;
        private readonly List<object> _buffer;
        private readonly object _entity;
        private int _bufferCounter = -1;
        private PropertyInfo[] _fields;
        private readonly Type _geType;
        private readonly Type[] _map;
        private readonly Type _type;
        private int _mapCounter;
        public void Dispose() { }
        public void Close() { }

        public EntityReader(object entity, Type geType = null)
        {
            _fieldSizer = new FieldSizer();
            _source = new List<object>();
            _buffer = new List<object>();
            _entity = entity;
            if (_entity == null) return;
            _type = _entity.GetType();
            _geType = Je.sys.GenericOf(_type) ?? _type;
            _geType = geType != null && geType.IsAssignableFrom(_geType) ? geType : _geType;
            _map = CommandEntity.GetMap(_geType);
            if (Je.sys.IsSimple(_type))
            {
                _source.Add(_entity);
                _fields = null;
            }
            else if (_entity is IDictionary)
            {
                _source.Add(_entity);
                _fields = GetFields(_type);
            }
            else if (_entity is IEnumerable)
            {
                foreach (var item in (IEnumerable)_entity) _source.Add(item);
                _fields = GetFields(_geType);
            }
            else
            {
                _source.Add(_entity);
                _fields = GetFields(_geType);
            }
            var currentType = _map[_mapCounter++];
            _fields = GetFields(currentType);
            foreach (var item in _source)
            {
                _buffer.Add(item);
            }
        }

        public bool NextResult()
        {
            _buffer.Clear();
            _bufferCounter = -1;
            if (_map == null || _map.Length < 1 || _mapCounter >= _map.Length) return false;
            if (_map.Length > 1 && !typeof(ICommandEntity).IsAssignableFrom(_geType)) return false;
            var currentType = _map[_mapCounter++];
            _fields = GetFields(currentType);
            foreach (object obj in _source)
            {
                var entity = obj as ICommandEntity;
                if (entity == null) continue;
                foreach (var item in entity.GetEntities(currentType)) _buffer.Add(item);
            }
            return true;
        }

        public bool Read()
        {
            return ++_bufferCounter < _buffer.Count;
        }

        public string[] GetTableNames()
        {
            return _map.Select(x => x.Name).ToArray();
        }

        public IDataReader GetData(int i)
        {
            throw new NotSupportedException();
        }

        public bool IsDBNull(int i)
        {
            return Nullable.GetUnderlyingType(_fields != null ? _fields[i].PropertyType : _geType) != null;
        }

        public string GetName(int i)
        {
            return _fields != null ? _fields[i].Name : string.Format("Column{0}", i);
        }

        public string GetDataTypeName(int i)
        {
            return _fields != null ? GetNullableUnderlyingType(_fields[i].PropertyType).FullName : _geType.FullName;
        }

        public Type GetFieldType(int i)
        {
            return _fields != null ? GetNullableUnderlyingType(_fields[i].PropertyType) : _geType;
        }

        private Type GetNullableUnderlyingType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public int GetOrdinal(string name)
        {
            if (_fields == null) return 0;
            for (int i = 0; i < _fields.Length; i++)
            {
                if (_fields[i].Name == name) return i;
            }
            return -1;
        }

        public int GetValues(object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = i < FieldCount ? GetValue(i) : null;
            }
            return values.Length;
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            var bytes = (byte[])GetValue(i);
            Array.Copy(bytes, fieldOffset, buffer, bufferoffset, length);
            return bytes.Length;
        }


        public long GetChars(int i, long fieldOffset, char[] buffer, int bufferoffset, int length)
        {
            var chars = (char[])GetValue(i);
            Array.Copy(chars, fieldOffset, buffer, bufferoffset, length);
            return chars.Length;
        }

        public bool GetBoolean(int i) { return GetValue(i).As<bool>(); }
        public byte GetByte(int i) { return GetValue(i).As<byte>(); }
        public char GetChar(int i) { return GetValue(i).As<char>(); }
        public Guid GetGuid(int i) { return GetValue(i).As<Guid>(); }
        public short GetInt16(int i) { return GetValue(i).As<short>(); }
        public int GetInt32(int i) { return GetValue(i).As<int>(); }
        public long GetInt64(int i) { return GetValue(i).As<long>(); }
        public float GetFloat(int i) { return GetValue(i).As<float>(); }
        public double GetDouble(int i) { return GetValue(i).As<double>(); }
        public string GetString(int i) { return GetValue(i).As<string>(); }
        public decimal GetDecimal(int i) { return GetValue(i).As<decimal>(); }
        public DateTime GetDateTime(int i) { return GetValue(i).As<DateTime>(); } 
        public object GetValue(int i)
        {
            var value = _fields != null ? _fields[i].GetValue(_buffer[_bufferCounter], null) : _buffer[_bufferCounter];
            _fieldSizer.SetSize(_mapCounter - 1, i, value);
            return value;
        }

        private static PropertyInfo[] GetFields(Type type)
        {
            return Je.sys.IsSimple(type) ? null : type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => Je.sys.IsSimple(p.PropertyType)).ToArray();
        }

        public static DataTable ToDataTable(object entity, Type geType = null)
        {
            var ds = ToDataSet(entity, geType);
            return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }

        public static DataSet ToDataSet(object entity, Type geType = null)
        {
            var ds = new DataSet { EnforceConstraints = false };
            var reader = new EntityReader(entity, geType);
            ds.Load(reader, LoadOption.PreserveChanges, reader.GetTableNames());
            return ds;
        }
    }

    public class FieldSizer
    {
        private readonly List<FieldSizerTable> _tables = new List<FieldSizerTable>();

        public FieldSizerTable this[int i]
        {
            get { return i >= 0 && i < _tables.Count ? _tables[i] : new FieldSizerTable(); }
            set { if (i >= 0 && i < _tables.Count) _tables[i] = value; else _tables.Add(value); }
        }

        public int SetSize(int table, int position, object value)
        {
            var size = 1;
            if (value != null && value != DBNull.Value)
            {
                if (value is string) size = ((string) value).Length;
                else
                if (value is byte[]) size = ((byte[])value).Length;
            }
            size = size > 1 ? size : 1;
            table = table >= 0 && table < _tables.Count ? table : _tables.Count;
            if (table == _tables.Count) _tables.Add(new FieldSizerTable());
            _tables[table].SetSize(position, size);
            return table;
        }

        public class FieldSizerTable
        {
            private readonly List<int> _sizes = new List<int>();

            public int this[int i]
            {
                get { return i >= 0 && i < _sizes.Count ? _sizes[i] : 1; }
                set { if (i >= 0 && i < _sizes.Count) _sizes[i] = value; else _sizes.Add(value); }
            }

            public int SetSize(int position, int size)
            {
                size = size > 1 ? size : 1;
                position = position >= 0 && position < _sizes.Count ? position : _sizes.Count;
                if (position == _sizes.Count) _sizes.Add(1);
                var oldSize = _sizes[position];
                _sizes[position] = size > oldSize ? size : oldSize;
                return position;
            }
        }

        public static FieldSizer FieldSizerOf(DataSet ds)
        {
            var sizer = new FieldSizer();
            for (int tabs = 0; tabs < ds.Tables.Count; tabs++)
            {
                var tab = ds.Tables[tabs];
                for (int rows = 0; rows < tab.Rows.Count; rows++)
                {
                    var row = tab.Rows[rows];
                    for (int cols = 0; cols < tab.Columns.Count; cols++)
                    {
                        sizer.SetSize(tabs, cols, row[cols]);
                    }
                }
            }
            return sizer;
        }
    }

    public static class EntityWriter
    {
        public static T To<T>(IDataReader reader)
        {
            return (new EntityWriter<T>(reader)).Write();
        }

        public static T To<T>(DataSet ds)
        {
            return (new EntityWriter<T>(ds.CreateDataReader())).Write();
        }
    }

    public class EntityWriter<T>
    {
        private readonly Dictionary<string, EntityHolder> _holders = new Dictionary<string, EntityHolder>();
        private readonly IDataReader _reader;
        private readonly Type _geType;
        private readonly Type[] _map;
        private readonly Type _type;

        public EntityWriter(IDataReader reader)
        { 
            _reader = reader;
            _type = typeof(T);
            _geType = Je.sys.GenericOf(_type) ?? _type;
            _map = CommandEntity.GetMap(_geType);
        }

        public T Write()
        { 
            var list = ReadData().ToList();
            if (list.Count == 0)
            {
                if (_type.IsArray) return (T)(object)Array.CreateInstance(_geType, 0);
                if (typeof(IList).IsAssignableFrom(_type)) return (T)Activator.CreateInstance(_type);
                return (T)Je.sys.DefaultOf(_type);
            }
            if (_type.IsArray)
            {
                var array = Array.CreateInstance(_geType, list.Count);
                for (int i = 0; i < array.Length; i++) array.SetValue(list[i], i);
                return (T)(object)array;
            }
            if (typeof(IList).IsAssignableFrom(_type))
            {
                var resultList = Activator.CreateInstance(_type) as IList;
                if (resultList == null) return (T)list[0];
                foreach (var item in list) resultList.Add(item);
                return (T)resultList;
            }
            return (T)list[0];
        }

        private IEnumerable<object> ReadData()
        {
            using (_reader)
            {
                var tableNumber = 0;
                var fields = GetFields();
                var currentType = GetCurrentType(tableNumber);
                while (true)
                {
                    if (_reader.Read())
                    {
                        var obj = CreateInstance(currentType, fields);
                        TrySaveEntity(obj);
                        if (tableNumber == 0) yield return obj;
                        continue;
                    }
                    if (_reader.NextResult())
                    {
                        tableNumber++;
                        fields = GetFields();
                        currentType = GetCurrentType(tableNumber);
                        continue;
                    }
                    break;
                }
            }
        }

        private Type GetCurrentType(int i)
        {
            return i >= 0 && i < _map.Length ? _map[i] : null;
        }

        private string[] GetFields()
        {
            var fields = new string[_reader.FieldCount];
            for (int i = 0; i < _reader.FieldCount; i++) fields[i] = _reader.GetName(i);
            return fields;
        }

        private object CreateInstance(Type type, string[] fields)
        {
            if (type == null) return null;
            if (Je.sys.IsSimple(type)) return !_reader.IsDBNull(0) ? _reader.GetValue(0) : Je.sys.DefaultOf(type);
            var obj = Activator.CreateInstance(type);
            var schema = CommandEntity.GetSchema(type);
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                var value = !_reader.IsDBNull(i) ? _reader.GetValue(i) : null;
                var p = schema.ContainsKey(field) ? schema[field] : null;
                if (p != null) p.SetValue(obj, value != null ? value.As(p.PropertyType) : Je.sys.DefaultOf(p.PropertyType), null);
            }
            return obj;
        }

        private void TrySaveEntity(object obj)
        {
            var entity = obj as ICommandEntity;
            if (entity == null) return;
            var id = entity.GetKey();
            var currentHolder = GetOrCreateHolder(id);
            foreach (string key in entity.GetKeys())
            {
                var holder = GetOrCreateHolder(key);
                holder.AddEntity(currentHolder);
                currentHolder.SetEntity(holder);
            }
            currentHolder.NewEntity(entity);
        }

        private EntityHolder GetOrCreateHolder(string id)
        {
            var holder = _holders.ContainsKey(id) ? _holders[id] : null;
            if (holder != null) return holder;
            holder = new EntityHolder();
            _holders.Add(id, holder);
            return holder;
        }

        private class EntityHolder
        {
            private List<EntityHolder> _masters;
            private List<EntityHolder> _details;
            private ICommandEntity _entity;

            public void NewEntity(ICommandEntity entity)
            {
                if (entity == null) return;
                if (_entity == null) _entity = entity;
                if (_details != null)
                {
                    var deleted = new List<EntityHolder>();
                    foreach (EntityHolder detail in _details)
                    {
                        if (detail._entity == null) continue;
                        detail._entity.SetEntity(_entity);
                        _entity.AddEntity(detail._entity);
                        deleted.Add(detail);
                    }
                    Clear(_details, deleted);
                    if (_details.Count == 0) _details = null;
                }
                if (_masters != null)
                {
                    var deleted = new List<EntityHolder>();
                    foreach (EntityHolder master in _masters)
                    {
                        if (master._entity == null) continue;
                        _entity.SetEntity(master._entity);
                        master._entity.AddEntity(_entity);
                        deleted.Add(master);
                    }
                    Clear(_masters, deleted);
                    if (_masters.Count == 0) _masters = null;
                }
            }

            public void AddEntity(EntityHolder holder)
            {
                if (_entity != null) _entity.AddEntity(holder._entity); else (_details = _details ?? new List<EntityHolder>()).Add(holder);
            }

            public void SetEntity(EntityHolder holder)
            {
                if (_entity != null) _entity.SetEntity(holder._entity); else (_masters = _masters ?? new List<EntityHolder>()).Add(holder);
            }

            private static void Clear(List<EntityHolder> sourceItems, List<EntityHolder> deletedItems)
            {
                foreach (var deleted in deletedItems)
                {
                    sourceItems.RemoveAll(x => ReferenceEquals(x, deleted));
                }
            }
        }
    }

    public class CommandEntity : ICommandEntity
    {
        private readonly Guid _id = Guid.NewGuid();
        string ICommandEntity.GetKey() { return GetKey(); }
        protected virtual string GetKey() { return _id.ToString(); }

        IEnumerable<string> ICommandEntity.GetKeys() { return GetKeys(); }
        protected virtual IEnumerable<string> GetKeys() { yield break; }

        IEnumerable<ICommandEntity> ICommandEntity.GetEntities(Type type) { return GetEntities(type); }
        protected virtual IEnumerable<ICommandEntity> GetEntities(Type type) { yield break; }

        void ICommandEntity.SetEntity(ICommandEntity item) { SetEntity(item); }
        protected virtual void SetEntity(ICommandEntity item) { }

        void ICommandEntity.AddEntity(ICommandEntity entity) { AddEntity(entity); }
        protected virtual void AddEntity(ICommandEntity entity) { }

        private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> Schemas = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
        private static readonly Dictionary<Type, Type[]> Maps = new Dictionary<Type, Type[]>();

        public static Dictionary<string, PropertyInfo> GetSchema(Type type)
        {
            if (!Schemas.ContainsKey(type))
            {
                lock (Schemas)
                {
                    if (!Schemas.ContainsKey(type)) Schemas.Add(type, GetProperties(type));
                }
            }
            return Schemas[type];
        }

        private static Dictionary<string, PropertyInfo> GetProperties(Type type)
        {
            var dict = new Dictionary<string, PropertyInfo>();
            foreach (var p in (type.GetProperties(BindingFlags.Instance | BindingFlags.Public)))
            {
                if (!dict.ContainsKey(p.Name)) dict.Add(p.Name, p);
            }
            return dict;
        }

        public static Type[] GetMap(Type type)
        {
            if (!Maps.ContainsKey(type))
            {
                lock (Maps)
                {
                    if (!Maps.ContainsKey(type)) Maps.Add(type, MapOf(type));
                }
            }
            return Maps[type];
        }

        private static Type[] MapOf(Type type)
        {
            var list = new List<Type> { type };
            foreach (var attribute in type.GetCustomAttributes(true))
            {
                var typeMapAttrubute = attribute as TypeMapAttribute;
                if (typeMapAttrubute == null) continue;
                list.AddRange(typeMapAttrubute.Types);
                break;
            }
            return list.ToArray();
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TypeMapAttribute : Attribute
    {
        public Type[] Types { get; private set; }

        public TypeMapAttribute(params Type[] types)
        {
            Types = types;
        }
    }

    public interface ICommandEntity
    {
        string GetKey();
        IEnumerable<string> GetKeys();
        void SetEntity(ICommandEntity entity);
        void AddEntity(ICommandEntity entity);
        IEnumerable<ICommandEntity> GetEntities(Type type);
    }
}