using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Commands;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Converts
{
    public static class SqlExtensions
    {
        public static DataSet of(this f.ISqlExpander exp, object value)
        {
            return t<SqlConverter>.one().Serialize(value);
        }

        public static T to<T>(this f.ISqlExpander exp, DataSet ds)
        {
            return (T)t<SqlConverter>.one().Deserialize(ds, typeof(T));
        }

        public static object to(this f.ISqlExpander exp, DataSet ds, Type type)
        {
            return t<SqlConverter>.one().Deserialize(ds, type);
        }

        public static SqlDbType classof(this f.ISqlExpander exp, Type type)
        {
            if (type.IsEnum
                || type == typeof(bool)
                || type == typeof(bool?)
                || type == typeof(byte)
                || type == typeof(byte?)
                || type == typeof(sbyte)
                || type == typeof(sbyte?)
                || type == typeof(short)
                || type == typeof(short?)
                || type == typeof(ushort)
                || type == typeof(ushort?)
                || type == typeof(int)
                || type == typeof(int?)
                || type == typeof(uint)
                || type == typeof(uint?)
                || type == typeof(long)
                || type == typeof(long?)
                || type == typeof(ulong)
                || type == typeof(ulong?)) return SqlDbType.BigInt;
            if (type == typeof(decimal)
                || type == typeof(decimal?)
                || type == typeof(float)
                || type == typeof(float?)
                || type == typeof(double)
                || type == typeof(double?)) return SqlDbType.Money;
            if (type == typeof(Guid) || type == typeof(Guid?)) return SqlDbType.UniqueIdentifier;
            if (type == typeof(DateTime) || type == typeof(DateTime?)) return SqlDbType.DateTime;
            if (type == typeof(char) || type == typeof(char?)) return SqlDbType.NChar;
            if (type == typeof(byte[])) return SqlDbType.VarBinary;
            if (type == typeof(string)) return SqlDbType.NVarChar;
            return SqlDbType.NVarChar;
        }

        public static object valueof(this f.ISqlExpander exp, object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            var type = value.GetType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(MssqlCommandParameterOutput<>))
            {
                return type.GetProperty("Value", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(value, null);
            }
            if (!f.sys.IsSimple(type))
            {
                return f.xml.of(value);
            }
            if (value is Guid)
            {
                var g = (Guid)value;
                if (g == Guid.Empty) return DBNull.Value;
            }
            if (value is DateTime)
            {
                var d = (DateTime)value;
                if (SqlDateTime.MinValue.Value >= d || d >= SqlDateTime.MaxValue.Value) return DBNull.Value;
            }
            if (value.Equals(f.sys.defaultof(value.GetType())))
            {
                return DBNull.Value;
            }
            if (type.IsEnum || type == typeof(bool) || type == typeof(bool?) || type == typeof(byte) || type == typeof(byte?) || type == typeof(sbyte) || type == typeof(sbyte?) || type == typeof(short) || type == typeof(short?) || type == typeof(ushort) || type == typeof(ushort?) || type == typeof(int) || type == typeof(int?) || type == typeof(uint) || type == typeof(uint?) || type == typeof(long) || type == typeof(long?) || type == typeof(ulong) || type == typeof(ulong?))
            {
                return (long)value;
            }
            if (type == typeof(decimal) || type == typeof(decimal?) || type == typeof(float) || type == typeof(float?) || type == typeof(double) || type == typeof(double?))
            {
                return (decimal)value;
            }
            return value;
        }

        public static bool IsOutputType(this f.ISqlExpander exp, ref Type type)
        {
            var geType = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(MssqlCommandParameterOutput<>) ? f.sys.genericof(type) : null;
            var isOutput = geType != null;
            type = geType ?? type;
            return isOutput;
        }
    }

    public class SqlConverter
    {
        public virtual DataSet Serialize(object value)
        {
            var ds = new DataSet();
            var reader = new EntityReader(value);
            ds.Load(reader, LoadOption.PreserveChanges, reader.GetTableNames());
            return ds;
        }

        public virtual object Deserialize(DataSet ds, Type type)
        { 
            var method = typeof(EntityWriter).GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(x => x.Name == "Write");
            method = method != null ? method.MakeGenericMethod(type) : null;
            if (method == null) return null;
            var reader = ds.CreateDataReader();
            return method.Invoke(null, new[] { reader });
        } 
    }
}
namespace Jetproger.Tools.AppConfig
{
    public class ConnectionString : ConfigSetting
    {
        public ConnectionString() : base("")
        {
            var connectionString = Value;
            try
            {
                var csb = new SqlConnectionStringBuilder(connectionString);
                csb.AsynchronousProcessing = true;
                csb.MultipleActiveResultSets = true;
                Value = csb.ToString();
            }
            catch
            {
                Value = connectionString;
            }
        }
    }
}