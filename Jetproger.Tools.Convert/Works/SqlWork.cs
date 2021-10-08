using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using Jetproger.Tools.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Factories;

namespace Jetproger.Tools.Convert.Works
{
    public class SqlWork : SqlWork<object>
    {
        public SqlWork(string commandText, CommandType commandType) : base(commandText, commandType)
        {
 
        }

        public new static SqlWork Op(string commandText)
        {
            return new SqlWork(commandText, CommandType.Text);
        }

        public new static SqlWork Sp(string commandText)
        {
            return new SqlWork(commandText, CommandType.StoredProcedure);
        }
    }

    public class SqlWork<TResult> : Work<TResult>
    {  
        private static string DefaultConnectionString { get { return Je.one.Get(DefaultConnectionStringHolder, GetAsyncConnectionString); } }
        private static readonly string[] DefaultConnectionStringHolder = { null };
        protected readonly Type GenericArgumentType;
        protected readonly SqlCommand Sql;
        protected Type[] ResolvingTypes;
        protected Type ResultType;
        private SqlState _sqlState;

        public SqlWork(string commandText, CommandType commandType)
        {
            GenericArgumentType = Je.sys.GenericOf(typeof(TResult)) ?? typeof(TResult);
            ResolvingTypes = DataEntity.Dependencies[GenericArgumentType];
            ResultType = typeof(TResult);
            Sql = new SqlCommand
            {
                Connection = new SqlConnection(DefaultConnectionString),
                CommandTimeout = int.MaxValue,
                CommandText = commandText,
                CommandType = commandType,
            };
        }

        protected override void Disposing()
        {
            Je.err.TryLess(() => Sql.Connection.Dispose());
            Je.err.TryLess(() => Sql.Dispose());
            Je.err.TryLess(() => (_sqlState != null).If(() => _sqlState.Dispose()));
            Je.err.TryLess(() => _sqlState = null);
        }

        public static SqlWork<TResult> Op(string commandText)
        {
            return new SqlWork<TResult>(commandText, CommandType.Text);
        }

        public static SqlWork<TResult> Sp(string commandText)
        {
            return new SqlWork<TResult>(commandText, CommandType.StoredProcedure);
        }

        public SqlWork<TResult> Param(string name, object value, SqlDbType sqlType)
        {
            Sql.Parameters.Add(new SqlParameter { Value = AsSqlValue(value), ParameterName = name, SqlDbType = sqlType, Direction = ParameterDirection.Input });
            return this;
        }

        public SqlWork<TResult> Param(string name, XmlDocument value)
        {
            var sqlValue = value != null ? (object)(new SqlXml(new XmlTextReader(value.InnerXml, XmlNodeType.Document, null))) : DBNull.Value;
            Sql.Parameters.Add(new SqlParameter { Value = sqlValue, ParameterName = name, SqlDbType = SqlDbType.Xml, Direction = ParameterDirection.Input });
            return this;
        }

        public SqlWork<TResult> Param(string name, object value)
        {
            Sql.Parameters.Add(new SqlParameter { Value = AsSqlValue(value), ParameterName = name, SqlDbType = AsSqlType(value), Direction = ParameterDirection.Input });
            return this;
        }

        protected override void OnStart()
        {
            try
            {
                if (Sql.Connection.State != ConnectionState.Open) Sql.Connection.Open();
                _sqlState = new SqlState();
                Sql.BeginExecuteReader(TryEndExecuteReader, _sqlState);
            }
            catch (Exception e)
            {
                ErrorAction(e);
            }
        }

        private void TryEndExecuteReader(IAsyncResult ar)
        {
            try
            {
                var reader = Sql.EndExecuteReader(ar);
                var state = ar.AsyncState as SqlState;
                if (state != null) state.Reader = reader;
                ThreadStock.Run(Completing);
            }
            catch (Exception e)
            {
                ErrorAction(e);
            }
        }

        private void Completing()
        {
            try
            {
                if (_sqlState.Reader != null) Result = GetResult(_sqlState.Reader);
            }
            catch (Exception e)
            {
                ErrorAction(e);
            }
            finally
            {
                Dispose();
            }
        }

        private TResult GetResult(SqlDataReader reader)
        { 
            if (ResultType == typeof(object)) return default(TResult);
            var list = ReadData(reader).ToList();
            if (list.Count == 0) return (TResult)Je.sys.DefaultOf(ResultType);
            if (!typeof(IList).IsAssignableFrom(ResultType)) return (TResult)list[0];
            var resultList = Activator.CreateInstance(ResultType) as IList;
            if (resultList == null) return (TResult)list[0];
            foreach (var item in list) resultList.Add(item);
            return (TResult)resultList;
        }

        private IEnumerable<object> ReadData(SqlDataReader reader)
        {
            if (string.IsNullOrWhiteSpace(Sql.Connection.ConnectionString)) yield break;
            var builder = new EntityBuilder();
            using (Sql.Connection)
            {
                Sql.Connection.InfoMessage -= ConnectionInfoMessage;
                Sql.Connection.InfoMessage += ConnectionInfoMessage;
                using (Sql)
                {
                    if (Sql.Connection.State != ConnectionState.Open) Sql.Connection.Open(); 
                    using (reader)
                    {
                        var tableNumber = 0;
                        var fields = GetFields(reader);
                        var currentType = tableNumber >= 0 && tableNumber < ResolvingTypes.Length ? ResolvingTypes[tableNumber] : null;
                        while (true)
                        {
                            if (reader.Read())
                            {
                                var obj = GetEntity(reader, currentType, fields);
                                builder.Add(obj);
                                if (tableNumber == 0) yield return obj;
                                continue;
                            }
                            if (reader.NextResult())
                            {
                                tableNumber++;
                                fields = GetFields(reader);
                                currentType = tableNumber >= 0 && tableNumber < ResolvingTypes.Length ? ResolvingTypes[tableNumber] : null;
                                continue;
                            }
                            break;
                        }
                    }
                }
            }
        }

        private void ConnectionInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Message))
            {
                Trace.WriteLine(new GTINInfo(e.Message));
            }
            foreach (SqlError error in e.Errors)
            {
                var s = error.ToString();
                if (!string.IsNullOrWhiteSpace(s)) Trace.WriteLine(new GTINFault(s));
            }
        }

        private static string[] GetFields(SqlDataReader reader)
        {
            var fields = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++) fields[i] = reader.GetName(i); 
            return fields;
        }

        private static object AsSqlValue(object value)
        {
            value = DbNullIf(value);
            if (value == DBNull.Value) return value;
            if (value is string) return value;
            if (value.GetType().IsValueType) return value;
            return Je.Xml<ClearXml>.AsString(value);
        }

        private static object DbNullIf(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            if (value is DateTime)
            {
                var date = (DateTime)value;
                if (SqlDateTime.MinValue.Value >= date || date >= SqlDateTime.MaxValue.Value) return DBNull.Value;
            }
            if (value.Equals(Je.sys.DefaultOf(value.GetType())))
            {
                return DBNull.Value;
            }
            return value;
        }

        private static SqlDbType AsSqlType(object value)
        {
            if (value == null) return SqlDbType.NVarChar;
            var type = value.GetType();
            if (type.IsEnum) return SqlDbType.Int;
            if (value is bool) return SqlDbType.Bit;
            if (value is byte) return SqlDbType.TinyInt;
            if (value is sbyte) return SqlDbType.TinyInt;
            if (value is byte[]) return SqlDbType.VarBinary;
            if (value is short) return SqlDbType.SmallInt;
            if (value is ushort) return SqlDbType.SmallInt;
            if (value is int) return SqlDbType.Int;
            if (value is uint) return SqlDbType.Int;
            if (value is long) return SqlDbType.BigInt;
            if (value is ulong) return SqlDbType.BigInt;
            if (value is Guid) return SqlDbType.UniqueIdentifier;
            if (value is float) return SqlDbType.Money;
            if (value is decimal) return SqlDbType.Money;
            if (value is double) return SqlDbType.Money;
            if (value is DateTime) return SqlDbType.DateTime;
            if (value is char) return SqlDbType.NVarChar;
            if (value is char[]) return SqlDbType.NVarChar;
            if (value is string) return SqlDbType.NVarChar;
            return SqlDbType.NText;
        }

        private static object GetEntity(SqlDataReader reader, Type entityType, string[] fields)
        {
            if (entityType == null) return null;
            if (entityType.IsValueType || entityType == typeof(string)) return !reader.IsDBNull(0) ? reader.GetValue(0) : Kz.Convert.GetDefault(entityType);
            var entity = Activator.CreateInstance(entityType);
            var properties = GetProperties(entityType);
            var i = 0;
            foreach (var field in fields)
            {
                var value = !reader.IsDBNull(i) ? reader.GetValue(i) : null;
                var p = properties.ContainsKey(field) ? properties[field] : null;
                if (p != null) p.SetValue(entity, value != null ? value.As(p.PropertyType) : Kz.Convert.GetDefault(p.PropertyType), null);
                i++;
            }
            return entity;
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

        private static string GetAsyncConnectionString()
        {
            var csb = new SqlConnectionStringBuilder(Je<ConnectionString>.As);
            csb.AsynchronousProcessing = true;
            csb.MultipleActiveResultSets = true;
            return csb.ToString();
        }

        private class EntityBuilder
        {
            private readonly Dictionary<string, EntityBuilderItem> _items = new Dictionary<string, EntityBuilderItem>();

            public void Add(object obj)
            {
                var entity = obj as IDataEntity;
                if (entity == null) return;
                var id = entity.GetKey();
                var builderItem = Get(id);
                foreach (string reference in entity.GetKeys())
                {
                    var item = Get(reference);
                    item.SetMany(builderItem);
                    builderItem.SetOne(item);
                }
                builderItem.SetInstance(entity);
            }

            private EntityBuilderItem Get(string id)
            {
                var item = _items.ContainsKey(id) ? _items[id] : null;
                if (item != null) return item;
                item = new EntityBuilderItem();
                _items.Add(id, item);
                return item;
            }
        }

        private class EntityBuilderItem
        {
            private List<EntityBuilderItem> _entities;
            private List<EntityBuilderItem> _items;
            private IDataEntity _instance;

            public void SetInstance(IDataEntity instance)
            {
                if (instance == null) return;
                if (_instance == null) _instance = instance;
                if (_items != null)
                {
                    var deleted = new List<EntityBuilderItem>();
                    foreach (EntityBuilderItem item in _items)
                    {
                        if (item._instance == null) continue;
                        item._instance.SetEntity(_instance);
                        _instance.AddEntity(item._instance);
                        deleted.Add(item);
                    }
                    Clear(_items, deleted);
                    if (_items.Count == 0) _items = null;
                }
                if (_entities != null)
                {
                    var deleted = new List<EntityBuilderItem>();
                    foreach (EntityBuilderItem entity in _entities)
                    {
                        if (entity._instance == null) continue;
                        _instance.SetEntity(entity._instance);
                        entity._instance.AddEntity(_instance);
                        deleted.Add(entity);
                    }
                    Clear(_entities, deleted);
                    if (_entities.Count == 0) _entities = null;
                }
            }

            public void SetMany(EntityBuilderItem item)
            {
                if (_instance != null) _instance.AddEntity(item._instance); else (_items = _items ?? new List<EntityBuilderItem>()).Add(item);
            }

            public void SetOne(EntityBuilderItem item)
            {
                if (_instance != null) _instance.SetEntity(item._instance); else (_entities = _entities ?? new List<EntityBuilderItem>()).Add(item);
            }

            private static void Clear(List<EntityBuilderItem> sourceItems, List<EntityBuilderItem> deletedItems)
            {
                foreach (var deleted in deletedItems)
                {
                    sourceItems.RemoveAll(item => ReferenceEquals(item, deleted));
                }
            }
        }
    }

    public class SqlState
    {
        public SqlDataReader Reader
        {
            get; set;
        }

        public void Dispose()
        {
            Je.err.TryLess(() => (Reader != null).If(() => Reader.Dispose()));
            Je.err.TryLess(() => Reader = null);
        }
    }
}