using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Jetproger.Tools.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Converts;
using MssqlCommandException = Jetproger.Tools.Convert.Bases.MssqlCommandException;

namespace Jetproger.Tools.Convert.Commands
{
    #region MssqlCommand 

    public abstract class MssqlCommand : BaseMssqlCommand<MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore>
    {
        protected MssqlCommand(string commandString) : base(commandString) { }
    }

    public abstract class MssqlCommand<T> : BaseMssqlCommand<T, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore>
    {
        protected MssqlCommand(string commandString) : base(commandString) { }
    }

    public abstract class MssqlCommand<T, T0> : BaseMssqlCommand<T, T0, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore>
    {
        public void Execute(T0 p0) { Execute(p0, null, null, null, null, null, null, null, null, null); }
        protected MssqlCommand(string commandString) : base(commandString) { }
    }

    public abstract class MssqlCommand<T, T0, T1> : BaseMssqlCommand<T, T0, T1, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore>
    {
        public void Execute(T0 p0, T1 p1) { Execute(p0, p1, null, null, null, null, null, null, null, null); }
        protected MssqlCommand(string commandString) : base(commandString) { }
    }

    public abstract class MssqlCommand<T, T0, T1, T2> : BaseMssqlCommand<T, T0, T1, T2, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore>
    {
        public void Execute(T0 p0, T1 p1, T2 p2) { Execute(p0, p1, p2, null, null, null, null, null, null, null); }
        protected MssqlCommand(string commandString) : base(commandString) { }
    }

    public abstract class MssqlCommand<T, T0, T1, T2, T3> : BaseMssqlCommand<T, T0, T1, T2, T3, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore>
    {
        public void Execute(T0 p0, T1 p1, T2 p2, T3 p3) { Execute(p0, p1, p2, p3, null, null, null, null, null, null); }
        protected MssqlCommand(string commandString) : base(commandString) { }
    }

    public abstract class MssqlCommand<T, T0, T1, T2, T3, T4> : BaseMssqlCommand<T, T0, T1, T2, T3, T4, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore>
    {
        public void Execute(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4) { Execute(p0, p1, p2, p3, p4, null, null, null, null, null); }
        protected MssqlCommand(string commandString) : base(commandString) { }
    }

    public abstract class MssqlCommand<T, T0, T1, T2, T3, T4, T5> : BaseMssqlCommand<T, T0, T1, T2, T3, T4, T5, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore>
    {
        public void Execute(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5) { Execute(p0, p1, p2, p3, p4, p5, null, null, null, null); }
        protected MssqlCommand(string commandString) : base(commandString) { }
    }

    public abstract class MssqlCommand<T, T0, T1, T2, T3, T4, T5, T6> : BaseMssqlCommand<T, T0, T1, T2, T3, T4, T5, T6, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore>
    {
        public void Execute(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6) { Execute(p0, p1, p2, p3, p4, p5, p6, null, null, null); }
        protected MssqlCommand(string commandString) : base(commandString) { }
    }

    public abstract class MssqlCommand<T, T0, T1, T2, T3, T4, T5, T6, T7> : BaseMssqlCommand<T, T0, T1, T2, T3, T4, T5, T6, T7, MssqlCommandParameterIgnore, MssqlCommandParameterIgnore>
    {
        public void Execute(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7) { Execute(p0, p1, p2, p3, p4, p5, p6, p7, null, null); }
        protected MssqlCommand(string commandString) : base(commandString) { }
    }

    public abstract class MssqlCommand<T, T0, T1, T2, T3, T4, T5, T6, T7, T8> : BaseMssqlCommand<T, T0, T1, T2, T3, T4, T5, T6, T7, T8, MssqlCommandParameterIgnore>
    {
        public void Execute(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8) { Execute(p0, p1, p2, p3, p4, p5, p6, p7, p8, null); }
        protected MssqlCommand(string commandString) : base(commandString) { }
    }

    public abstract class MssqlCommand<T, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : BaseMssqlCommand<T, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        protected MssqlCommand(string commandString) : base(commandString) { }
    }

    #endregion

    public abstract class BaseMssqlCommand<T, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : Command<T, object>
    {
        private string _commandString;

        protected BaseMssqlCommand(string commandString)
        {
            _commandString = commandString;
        }

        protected T0 P0 { get; set; }
        protected T1 P1 { get; set; }
        protected T2 P2 { get; set; }
        protected T3 P3 { get; set; }
        protected T4 P4 { get; set; }
        protected T5 P5 { get; set; }
        protected T6 P6 { get; set; }
        protected T7 P7 { get; set; }
        protected T8 P8 { get; set; }
        protected T9 P9 { get; set; }

        protected override object GetValue()
        {
            return new object[] { P0, P1, P2, P3, P4, P5, P6, P7, P8, P9 };
        }

        protected override void SetValue(object value)
        {
            var enumerable = value as IEnumerable;
            if (enumerable == null)
            {
                P0 = value.As<T0>();
                return;
            }
            var enumerator = enumerable.GetEnumerator();
            var i = 0;
            while (enumerator.MoveNext())
            {
                if (i++ >= 9) break;
                switch (i)
                {
                    case 0: P0 = enumerator.Current.As<T0>(); break;
                    case 1: P1 = enumerator.Current.As<T1>(); break;
                    case 2: P2 = enumerator.Current.As<T2>(); break;
                    case 3: P3 = enumerator.Current.As<T3>(); break;
                    case 4: P4 = enumerator.Current.As<T4>(); break;
                    case 5: P5 = enumerator.Current.As<T5>(); break;
                    case 6: P6 = enumerator.Current.As<T6>(); break;
                    case 7: P7 = enumerator.Current.As<T7>(); break;
                    case 8: P8 = enumerator.Current.As<T8>(); break;
                    case 9: P9 = enumerator.Current.As<T9>(); break;
                }
            }
        }

        public void Execute(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
        {
            SetValues(p0, p1, p2, p3, p4, p5, p6, p7, p8, p9);
            InterfaceCommand.Execute();
        }

        protected void SetValues(T0 p0, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
        {
            P0 = p0; P1 = p1; P2 = p2; P3 = p3; P4 = p4; P5 = p5; P6 = p6; P7 = p7; P8 = p8; P9 = p9;
        }

        protected override void Execute()
        {
            var datasets = new List<MssqlCommandBulkCopyDataSet>();
            var cmd = BuildCommand(datasets);
            if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
            MssqlCommandBulkCopy.WriteToServer(cmd.Connection, datasets);
            var state = new MssqlCommandState(cmd);
            cmd.BeginExecuteReader(EndExecuteReader, state);
        }

        private void EndExecuteReader(IAsyncResult ar)
        {
            try
            {
                var state = ar.AsyncState as MssqlCommandState;
                if (state == null) return;
                var reader = state.Command.EndExecuteReader(ar);
                state.Reader = reader;
                CommandThreads.Run(() => Completing(state));
            }
            catch (Exception e)
            {
                Error = e;
            }
        }

        private void Completing(MssqlCommandState state)
        {
            try
            {
                var result = state.Reader != null ? GetResult(state.Reader) : default(T);
                var typeIgnore = typeof(MssqlCommandParameterIgnore);
                var ordinal = 0;
                P0 = GetParameterValue(state.Command, typeIgnore, P0, ref ordinal);
                P1 = GetParameterValue(state.Command, typeIgnore, P1, ref ordinal);
                P2 = GetParameterValue(state.Command, typeIgnore, P2, ref ordinal);
                P3 = GetParameterValue(state.Command, typeIgnore, P3, ref ordinal);
                P4 = GetParameterValue(state.Command, typeIgnore, P4, ref ordinal);
                P5 = GetParameterValue(state.Command, typeIgnore, P5, ref ordinal);
                P6 = GetParameterValue(state.Command, typeIgnore, P6, ref ordinal);
                P7 = GetParameterValue(state.Command, typeIgnore, P7, ref ordinal);
                P8 = GetParameterValue(state.Command, typeIgnore, P8, ref ordinal);
                P9 = GetParameterValue(state.Command, typeIgnore, P9, ref ordinal);
                state.Dispose();
                Result = result;
            }
            catch (Exception e)
            {
                state.Dispose();
                Error = e;
            }
        }

        private T GetResult(SqlDataReader reader)
        {
            var type = typeof(T);
            if (type == typeof(DataSet) || type == typeof(DataTable) || type == typeof(MssqlCommandBulkCopyDataSet))
            {
                var ds = new DataSet { EnforceConstraints = false };
                while (!reader.IsClosed) ds.Tables.Add().Load(reader);
                object result = ds;
                if (type == typeof(DataTable))
                {
                    result = ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
                }
                if (type == typeof(MssqlCommandBulkCopyDataSet))
                {
                    result = new MssqlCommandBulkCopyDataSet(FieldSizer.FieldSizerOf(ds), ds);
                }
                return (T)result;
            }
            return EntityWriter.To<T>(reader);
        }

        private TParameter GetParameterValue<TParameter>(SqlCommand cmd, Type typeIgnore, TParameter value, ref int ordinal)
        {
            var type = typeof(TParameter);
            if (type == typeIgnore) return value;
            var originalType = type;
            var isOutput = Je.sql.IsOutputType(ref type);
            if (!Je.sys.IsSimple(type)) return value;
            if (isOutput)
            {
                var parameterName = string.Format("P{0}", ordinal);
                var parameterValue = cmd.Parameters[parameterName].Value.As(type);
                value = Activator.CreateInstance<TParameter>();
                originalType.GetProperty("Value", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(value, parameterValue != DBNull.Value && parameterValue != null ? parameterValue : Je.sys.DefaultOf(originalType), null);
            }
            ordinal++;
            return value;
        }

        private SqlCommand BuildCommand(List<MssqlCommandBulkCopyDataSet> datasets)
        {
            var cmd = new SqlCommand
            {
                Connection = new SqlConnection(BuildConnectionString()),
                CommandText = BuildScript(datasets),
                CommandType = CommandType.Text,
                CommandTimeout = int.MaxValue,
            };
            cmd.Parameters.Add(new SqlParameter
            {
                Direction = ParameterDirection.Input,
                SqlDbType = SqlDbType.NVarChar,
                ParameterName = "@QUERY",
                Value = GetQuery(),
                Size = 4000,
            });
            var ordinal = 0;
            var typeIgnore = typeof(MssqlCommandParameterIgnore);
            AddParameter(cmd, typeIgnore, typeof(T0), P0, ref ordinal);
            AddParameter(cmd, typeIgnore, typeof(T1), P1, ref ordinal);
            AddParameter(cmd, typeIgnore, typeof(T2), P2, ref ordinal);
            AddParameter(cmd, typeIgnore, typeof(T3), P3, ref ordinal);
            AddParameter(cmd, typeIgnore, typeof(T4), P4, ref ordinal);
            AddParameter(cmd, typeIgnore, typeof(T5), P5, ref ordinal);
            AddParameter(cmd, typeIgnore, typeof(T6), P6, ref ordinal);
            AddParameter(cmd, typeIgnore, typeof(T7), P7, ref ordinal);
            AddParameter(cmd, typeIgnore, typeof(T8), P8, ref ordinal);
            AddParameter(cmd, typeIgnore, typeof(T9), P9, ref ordinal);
            cmd.Connection.InfoMessage -= MssqlConnectionInfoMessage;
            cmd.Connection.InfoMessage += MssqlConnectionInfoMessage;
            return cmd;
        }

        private void AddParameter(SqlCommand cmd, Type typeIgnore, Type type, object value, ref int ordinal)
        {
            if (type == typeIgnore) return;
            var isOutput = Je.sql.IsOutputType(ref type);
            if (!Je.sys.IsSimple(type)) return;
            var sqlType = Je.sql.SqlTypeOf(type);
            var direction = ParameterDirection.Input;
            if (isOutput)
            {
                direction = ParameterDirection.InputOutput;
                value = Je.sql.SqlValueOf(value);
            }
            var p = new SqlParameter
            {
                Direction = direction,
                ParameterName = string.Format("P{0}", ordinal),
                Value = value,
                SqlDbType = sqlType,
            };
            if (sqlType == SqlDbType.Char || sqlType == SqlDbType.VarChar
            || sqlType == SqlDbType.NChar || sqlType == SqlDbType.NVarChar
            || sqlType == SqlDbType.Binary || sqlType == SqlDbType.VarBinary) p.Size = int.MaxValue;
            cmd.Parameters.Add(p);
            ordinal++;
        }

        private string BuildConnectionString()
        {
            var csb = new SqlConnectionStringBuilder(GetConnectionString());
            csb.AsynchronousProcessing = true;
            csb.MultipleActiveResultSets = true;
            return csb.ToString();
        }

        protected virtual string GetConnectionString()
        {
            return J_<ConnectionString>.Sz;
        }

        private void MssqlConnectionInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Message))
            {
                Trace.WriteLine(e.Message);
            }
            foreach (SqlError error in e.Errors)
            {
                if (!string.IsNullOrWhiteSpace(error.Message) && error.Message != e.Message) Trace.WriteLine(new MssqlCommandException(_commandString, error.Message));
            }
        }

        private string GetQuery()
        {
            var commandString = _commandString.Trim(' ', '\t', '\r', '\n');
            var strings = commandString.Split(' ');
            strings = strings.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            if (strings.Length == 1)
            {
                var ordinal = 0;
                var sb = new StringBuilder();
                var typeIgnore = typeof(MssqlCommandParameterIgnore);
                AddQueryParameterScript(typeIgnore, typeof(T0), sb, ref ordinal);
                AddQueryParameterScript(typeIgnore, typeof(T1), sb, ref ordinal);
                AddQueryParameterScript(typeIgnore, typeof(T2), sb, ref ordinal);
                AddQueryParameterScript(typeIgnore, typeof(T3), sb, ref ordinal);
                AddQueryParameterScript(typeIgnore, typeof(T4), sb, ref ordinal);
                AddQueryParameterScript(typeIgnore, typeof(T5), sb, ref ordinal);
                AddQueryParameterScript(typeIgnore, typeof(T6), sb, ref ordinal);
                AddQueryParameterScript(typeIgnore, typeof(T7), sb, ref ordinal);
                AddQueryParameterScript(typeIgnore, typeof(T8), sb, ref ordinal);
                AddQueryParameterScript(typeIgnore, typeof(T9), sb, ref ordinal);
                commandString = "EXEC " + commandString + sb;
            }
            return commandString;
        }

        private void AddQueryParameterScript(Type typeIgnore, Type type, StringBuilder sb, ref int ordinal)
        {
            if (type == typeIgnore) return;
            var isOutput = Je.sql.IsOutputType(ref type);
            if (!Je.sys.IsSimple(type)) return;
            sb.AppendFormat("{0} @P{1}{2}", sb.Length > 0 ? "," : "", ordinal++, isOutput ? " OUTPUT" : "");
        }

        private string BuildScript(List<MssqlCommandBulkCopyDataSet> datasets)
        {
            var ordinal = 0;
            var sbDeclare = new StringBuilder();
            var sbBinding = new StringBuilder();
            var typeIgnore = typeof(MssqlCommandParameterIgnore);
            GetScripts(typeIgnore, typeof(T0), P0, datasets, sbDeclare, sbBinding, ref ordinal);
            GetScripts(typeIgnore, typeof(T1), P1, datasets, sbDeclare, sbBinding, ref ordinal);
            GetScripts(typeIgnore, typeof(T2), P2, datasets, sbDeclare, sbBinding, ref ordinal);
            GetScripts(typeIgnore, typeof(T3), P3, datasets, sbDeclare, sbBinding, ref ordinal);
            GetScripts(typeIgnore, typeof(T4), P4, datasets, sbDeclare, sbBinding, ref ordinal);
            GetScripts(typeIgnore, typeof(T5), P5, datasets, sbDeclare, sbBinding, ref ordinal);
            GetScripts(typeIgnore, typeof(T6), P6, datasets, sbDeclare, sbBinding, ref ordinal);
            GetScripts(typeIgnore, typeof(T7), P7, datasets, sbDeclare, sbBinding, ref ordinal);
            GetScripts(typeIgnore, typeof(T8), P8, datasets, sbDeclare, sbBinding, ref ordinal);
            GetScripts(typeIgnore, typeof(T9), P9, datasets, sbDeclare, sbBinding, ref ordinal);
            var declare = string.Format("N'{0}'", sbDeclare);
            return string.Format("EXEC SP_EXECUTESQL @QUERY{0}{1}", sbDeclare.Length > 0 ? ", " + declare : "", sbBinding.Length > 0 ? ", " + sbBinding : "");
        }

        private void GetScripts(Type typeIgnore, Type type, object value, List<MssqlCommandBulkCopyDataSet> datasets, StringBuilder sbDeclare, StringBuilder sbBinding, ref int ordinal)
        {
            if (type == typeIgnore) return;
            var isOutput = Je.sql.IsOutputType(ref type);
            if (!Je.sys.IsSimple(type))
            {
                GetDataset(type, value, datasets);
                return;
            }
            sbDeclare.AppendFormat("{0}{1}", sbDeclare.Length > 0 ? ", " : "", GetDeclareScript(type, ordinal, isOutput));
            sbBinding.AppendFormat("{0}{1}", sbBinding.Length > 0 ? ", " : "", GetBindingScript(type, ordinal, isOutput));
            ordinal++;
        }

        private void GetDataset(Type type, object value, List<MssqlCommandBulkCopyDataSet> datasets)
        {
            var bulkDs = GetBulkCopyDataset(type, value);
            if (bulkDs != null)
            {
                datasets.Add(bulkDs);
                return;
            }
            var ds = new DataSet { EnforceConstraints = false };
            var reader = new EntityReader(value, type);
            ds.Load(reader, LoadOption.PreserveChanges, reader.GetTableNames());
            datasets.Add(new MssqlCommandBulkCopyDataSet(reader.Sizer, ds));
        }

        private MssqlCommandBulkCopyDataSet GetBulkCopyDataset(Type type, object value)
        {
            if (type == typeof(MssqlCommandBulkCopyDataSet))
            {
                return value as MssqlCommandBulkCopyDataSet;
            }
            var ds = (DataSet)null;
            if (type == typeof(DataSet))
            {
                ds = value as DataSet;
            }
            if (type == typeof(DataTable))
            {
                var table = value as DataTable;
                if (table != null)
                {
                    ds = new DataSet { EnforceConstraints = false };
                    ds.Tables.Add(table);
                }
            }
            if (ds == null) return null;
            return new MssqlCommandBulkCopyDataSet(FieldSizer.FieldSizerOf(ds), ds);
        }

        private string GetDeclareScript(Type type, int ordinal, bool isOutput)
        {
            return string.Format("@P{0} {1}{2}", ordinal, GetSqlTypeText(type), isOutput ? " OUTPUT" : "");
        }

        private string GetBindingScript(Type type, int ordinal, bool isOutput)
        {
            return string.Format("@P{0} = @P{0}{1}", ordinal, isOutput ? " OUTPUT" : "");
        }

        private string GetSqlTypeText(Type type)
        {
            var sqlType = Je.sql.SqlTypeOf(type);
            switch (sqlType)
            {
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                case SqlDbType.Binary:
                case SqlDbType.VarBinary: return string.Format("[{0}](MAX)", sqlType).ToUpper();
                case SqlDbType.Timestamp: return "BIGINT";
                default: return string.Format("[{0}]", sqlType).ToUpper();
            }
        }
    }

    public class MssqlCommandBulkCopy
    {
        private MssqlCommandBulkCopy(SqlConnection connection, MssqlCommandBulkCopyDataSet dataset) : this(connection, new[] { dataset }) { }
        private readonly IEnumerable<MssqlCommandBulkCopyDataSet> _datasets;
        private readonly SqlConnection _connection;

        private MssqlCommandBulkCopy(SqlConnection connection, IEnumerable<MssqlCommandBulkCopyDataSet> datasets)
        {
            _connection = connection;
            _datasets = datasets;
        }

        public static void WriteToServer(SqlConnection connection, MssqlCommandBulkCopyDataSet dataset)
        {
            (new MssqlCommandBulkCopy(connection, dataset)).BulkCopy();
        }

        public static void WriteToServer(SqlConnection connection, IEnumerable<MssqlCommandBulkCopyDataSet> datasets)
        {
            (new MssqlCommandBulkCopy(connection, datasets)).BulkCopy();
        }

        private void BulkCopy()
        {
            if (_connection.State != ConnectionState.Open) _connection.Open();
            var tableNames = new HashSet<string>();
            foreach (MssqlCommandBulkCopyDataSet ds in _datasets)
            {
                for (int i = 0; i < ds.Source.Tables.Count; i++)
                {
                    var table = ds.Source.Tables[i];
                    if (!tableNames.Contains(table.TableName))
                    {
                        tableNames.Add(table.TableName);
                        CreateTempTable(table, ds.Sizer, i);
                    }
                    BulkCopy(table);
                }
            }
        }

        private void CreateTempTable(DataTable table, FieldSizer sizer, int index)
        {
            using (var cmd = new SqlCommand(GetTempTableQuery(table, sizer, index), _connection))
            {
                cmd.CommandTimeout = int.MaxValue;
                cmd.ExecuteNonQuery();
            }
        }

        private void BulkCopy(DataTable table)
        {
            using (var bulkCopy = new SqlBulkCopy(_connection, SqlBulkCopyOptions.Default, null))
            {
                bulkCopy.DestinationTableName = "#" + table.TableName;
                bulkCopy.BulkCopyTimeout = int.MaxValue;
                bulkCopy.WriteToServer(table);
            }
        }

        private string GetTempTableQuery(DataTable table, FieldSizer sizer, int index)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("IF OBJECT_ID('TEMPDB..#{0}') IS NOT NULL DROP TABLE #{0} ", table.TableName);
            sb.AppendFormat("CREATE TABLE #{0} (", table.TableName);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sb.AppendFormat("{0} [{1}] {2} NULL", i > 0 ? "," : "", table.Columns[i].ColumnName, GetSqlTypeText(table.Columns[i].DataType, sizer[index][i]));
            }
            sb.AppendFormat(")");
            return sb.ToString();
        }

        private string GetSqlTypeText(Type type, int size)
        {
            var sqlType = Je.sql.SqlTypeOf(type);
            switch (sqlType)
            {
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                case SqlDbType.Binary:
                case SqlDbType.VarBinary: return string.Format("[{0}]({1})", sqlType, size <= 4000 ? size.ToString() : "MAX");
                case SqlDbType.Timestamp: return string.Format("[{0}] NOT", sqlType);
                default: return string.Format("[{0}]", sqlType);
            }
        }
    }

    public class MssqlCommandBulkCopyDataSet
    {
        public readonly FieldSizer Sizer;
        public readonly DataSet Source;
        public MssqlCommandBulkCopyDataSet(FieldSizer sizer, DataSet source)
        {
            Sizer = sizer;
            Source = source;
        }
    }

    public class MssqlCommandParameterOutput<T>
    {
        private T Value { get; set; }

        public static explicit operator T(MssqlCommandParameterOutput<T> p)
        {
            return p.Value;
        }

        public static implicit operator MssqlCommandParameterOutput<T>(T value)
        {
            return new MssqlCommandParameterOutput<T> { Value = value };
        }
    }

    public class MssqlCommandState
    {
        public SqlCommand Command { get; private set; }
        public SqlDataReader Reader { get; set; }

        public MssqlCommandState(SqlCommand command)
        {
            Command = command;
        }

        public void Dispose()
        {
            DisposeReader();
            DisposeConnection();
            DisposeCommand();
        }

        public void DisposeReader()
        {
            try
            {
                Reader.Dispose();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        public void DisposeConnection()
        {
            try
            {
                Command.Connection.Dispose();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        public void DisposeCommand()
        {
            try
            {
                Command.Dispose();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }
    }
    public class MssqlCommandParameterIgnore { }
}