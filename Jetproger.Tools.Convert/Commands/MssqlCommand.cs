using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Commanders;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Commands
{
    public abstract class MssqlCommand : MssqlCommand<object, object, object, object, object, object, object, object, object, object, object, object> { protected MssqlCommand(string commandString) : base(commandString) { } }
    public abstract class MssqlCommand<T> : MssqlCommand<T, T, object, object, object, object, object, object, object, object, object, object> { protected MssqlCommand(string commandString) : base(commandString) { } }
    public abstract class MssqlCommand<T, T_> : MssqlCommand<T, T_, object, object, object, object, object, object, object, object, object, object> { protected MssqlCommand(string commandString) : base(commandString) { } }
    public abstract class MssqlCommand<T, T_, T0> : MssqlCommand<T, T_, T0, object, object, object, object, object, object, object, object, object> { protected MssqlCommand(string commandString) : base(commandString) { } }
    public abstract class MssqlCommand<T, T_, T0, T1> : MssqlCommand<T, T_, T0, T1, object, object, object, object, object, object, object, object> { protected MssqlCommand(string commandString) : base(commandString) { } }
    public abstract class MssqlCommand<T, T_, T0, T1, T2> : MssqlCommand<T, T_, T0, T1, T2, object, object, object, object, object, object, object> { protected MssqlCommand(string commandString) : base(commandString) { } }
    public abstract class MssqlCommand<T, T_, T0, T1, T2, T3> : MssqlCommand<T, T_, T0, T1, T2, T3, object, object, object, object, object, object> { protected MssqlCommand(string commandString) : base(commandString) { } }
    public abstract class MssqlCommand<T, T_, T0, T1, T2, T3, T4> : MssqlCommand<T, T_, T0, T1, T2, T3, T4, object, object, object, object, object> { protected MssqlCommand(string commandString) : base(commandString) { } }
    public abstract class MssqlCommand<T, T_, T0, T1, T2, T3, T4, T5> : MssqlCommand<T, T_, T0, T1, T2, T3, T4, T5, object, object, object, object> { protected MssqlCommand(string commandString) : base(commandString) { } }
    public abstract class MssqlCommand<T, T_, T0, T1, T2, T3, T4, T5, T6> : MssqlCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, object, object, object> { protected MssqlCommand(string commandString) : base(commandString) { } }
    public abstract class MssqlCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7> : MssqlCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, object, object> { protected MssqlCommand(string commandString) : base(commandString) { } }
    public abstract class MssqlCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8> : MssqlCommand<T, T_, T0, T1, T2, T3, T4, T5, T6, T7, T8, object> { protected MssqlCommand(string commandString) : base(commandString) { } }
    public abstract class MssqlCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : DelayCommand<TResult, TValue, T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>, ICommand
    {
        protected MssqlCommand(string commandString) { _commandString = commandString; }
        protected virtual void BeforeExecute(SqlConnection sqlConnection) { }
        protected virtual void AfterExecute(SqlConnection sqlConnection) { }
        private readonly string _commandString;
        private _MssqlState _state;

        public void Execute()
        {
            try
            {
                if (State != ECommandState.None) return;
                State = ECommandState.Running;
                var datasets = new List<_BulkCopyDataSet>();
                var cnc = new SqlConnection(BuildConnectionString());
                if (cnc.State != ConnectionState.Open) cnc.Open();
                BeforeExecute(cnc);
                var cmd = BuildCommand(cnc, datasets);
                (new _BulkCopy { Connection = cmd.Connection, Datasets = datasets }).BulkCopy();
                var state = new _MssqlState { Command = cmd };
                cmd.BeginExecuteReader(EndExecuteReader, state);
            }
            catch (Exception e)
            {
                Error = e;
            }
        }

        private void EndExecuteReader(IAsyncResult ar)
        {
            try
            {
                var state = ar.AsyncState as _MssqlState;
                if (state == null) return;
                var reader = state.Command.EndExecuteReader(ar);
                state.Reader = reader;
                Delay(state);
            }
            catch (Exception e)
            {
                Error = e;
            }
        }

        private void Delay(_MssqlState state)
        {
            _state = state;
            if (((IDelay)this).CancelDelay) Completing(state); else SetState(ECommandState.Completed);
        }

        public override void Complete()
        {
            Completing(_state);
            base.Complete();
        }

        private void Completing(_MssqlState state)
        {
            try
            {
                var result = state.Reader != null ? GetResult(state.Reader) : default(TResult);
                var typeIgnore = typeof(object);
                var ordinal = 0;
                Value = GetParameterValue(state.Command, typeIgnore, Value, ref ordinal);
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
                AfterExecute(state.Command.Connection);
            }
            catch (Exception e)
            {
                if (state != null) state.Dispose();
                Error = e;
            }
        }

        private TResult GetResult(SqlDataReader reader)
        {
            var type = typeof(TResult);
            if (type == typeof(DataSet) || type == typeof(DataTable) || type == typeof(_BulkCopyDataSet))
            {
                var ds = new DataSet { EnforceConstraints = false };
                while (!reader.IsClosed) ds.Tables.Add().Load(reader);
                object result = ds;
                if (type == typeof(DataTable))
                {
                    result = ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
                }
                if (type == typeof(_BulkCopyDataSet))
                {
                    result = new _BulkCopyDataSet(FieldSizer.FieldSizerOf(ds), ds);
                }
                return (TResult)result;
            }
            return reader.As<TResult>();
        }

        private TParameter GetParameterValue<TParameter>(SqlCommand cmd, Type typeIgnore, TParameter value, ref int ordinal)
        {
            var type = typeof(TParameter);
            if (type == typeIgnore) return value;
            var originalType = type;
            var isOutput = f.sql.isout(ref type);
            if (!f.sys.issimple(type)) return value;
            if (isOutput)
            {
                var parameterName = string.Format("P{0}", ordinal);
                var parameterValue = cmd.Parameters[parameterName].Value.As(type);
                value = Activator.CreateInstance<TParameter>();
                originalType.GetProperty("Value", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(value, parameterValue != DBNull.Value && parameterValue != null ? parameterValue : f.sys.defof(originalType), null);
            }
            ordinal++;
            return value;
        }

        private SqlCommand BuildCommand(SqlConnection connection, List<_BulkCopyDataSet> datasets)
        {
            var cmd = new SqlCommand
            {
                Connection = connection,
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
            var typeIgnore = typeof(object);
            AddParameter(cmd, typeIgnore, typeof(TValue), Value, ref ordinal);
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
            var isOutput = f.sql.isout(ref type);
            if (!f.sys.issimple(type)) return;
            var sqlType = f.sql.classof(type);
            var direction = ParameterDirection.Input;
            if (isOutput)
            {
                direction = ParameterDirection.InputOutput;
                value = f.sql.valueof(value);
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
            return k<Jetproger.Tools.AppConfig.ConnectionString>.key();
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
                var typeIgnore = typeof(object);
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
            var isOutput = f.sql.isout(ref type);
            if (!f.sys.issimple(type)) return;
            sb.AppendFormat("{0} @P{1}{2}", sb.Length > 0 ? "," : "", ordinal++, isOutput ? " OUTPUT" : "");
        }

        private string BuildScript(List<_BulkCopyDataSet> datasets)
        {
            var ordinal = 0;
            var sbDeclare = new StringBuilder();
            var sbBinding = new StringBuilder();
            var typeIgnore = typeof(object);
            GetScripts(typeIgnore, typeof(TValue), Value, datasets, sbDeclare, sbBinding, ref ordinal);
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

        private void GetScripts(Type typeIgnore, Type type, object value, List<_BulkCopyDataSet> datasets, StringBuilder sbDeclare, StringBuilder sbBinding, ref int ordinal)
        {
            if (type == typeIgnore) return;
            var isOutput = f.sql.isout(ref type);
            if (!f.sys.issimple(type))
            {
                GetDataset(type, value, datasets);
                return;
            }
            sbDeclare.AppendFormat("{0}{1}", sbDeclare.Length > 0 ? ", " : "", GetDeclareScript(type, ordinal, isOutput));
            sbBinding.AppendFormat("{0}{1}", sbBinding.Length > 0 ? ", " : "", GetBindingScript(type, ordinal, isOutput));
            ordinal++;
        }

        private void GetDataset(Type type, object value, List<_BulkCopyDataSet> datasets)
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
            datasets.Add(new _BulkCopyDataSet(reader.Sizer, ds));
        }

        private _BulkCopyDataSet GetBulkCopyDataset(Type type, object value)
        {
            if (type == typeof(_BulkCopyDataSet))
            {
                return value as _BulkCopyDataSet;
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
            return new _BulkCopyDataSet(FieldSizer.FieldSizerOf(ds), ds);
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
            var sqlType = f.sql.classof(type);
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

        #region inner types

        private class _BulkCopy
        {
            public IEnumerable<_BulkCopyDataSet> Datasets;
            public SqlConnection Connection;

            public void BulkCopy()
            {
                if (Connection.State != ConnectionState.Open) Connection.Open();
                var tableNames = new HashSet<string>();
                foreach (_BulkCopyDataSet ds in Datasets)
                {
                    using (ds.Source)
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
            }

            private void CreateTempTable(DataTable table, FieldSizer sizer, int index)
            {
                using (var cmd = new SqlCommand(GetTempTableQuery(table, sizer, index), Connection))
                {
                    cmd.CommandTimeout = int.MaxValue;
                    cmd.ExecuteNonQuery();
                }
            }

            private void BulkCopy(DataTable table)
            {
                using (var bulkCopy = new SqlBulkCopy(Connection, SqlBulkCopyOptions.Default, null))
                {
                    using (table)
                    {
                        bulkCopy.DestinationTableName = "#" + table.TableName;
                        bulkCopy.BulkCopyTimeout = int.MaxValue;
                        bulkCopy.WriteToServer(table);
                    }
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
                var sqlType = f.sql.classof(type);
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

        private class _BulkCopyDataSet
        {
            public readonly DateTime MinDate = new DateTime(1900, 1, 1);
            public readonly DateTime MaxDate = new DateTime(2100, 1, 1);
            public readonly FieldSizer Sizer;
            public readonly DataSet Source;

            public _BulkCopyDataSet(FieldSizer sizer, DataSet source)
            {
                Sizer = sizer;
                Source = source;
                SqlNormDateTimeOf();
            }

            private void SqlNormDateTimeOf()
            {
                foreach (DataTable table in Source.Tables)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.DataType != typeof(DateTime) && column.DataType != typeof(DateTime?)) continue;
                        foreach (DataRow row in table.Rows)
                        {
                            var value = row[column];
                            if (value != null && value != DBNull.Value) row[column] = SqlNormDateTimeOf((DateTime)value, column.AllowDBNull);
                        }
                    }
                }
            }

            private object SqlNormDateTimeOf(DateTime d, bool allowDBNull)
            {
                d = d < MaxDate ? d : MaxDate;
                d = d > MinDate ? d : MinDate;
                return d > MinDate && d < MaxDate ? d : (allowDBNull ? DBNull.Value : (object)d);
            }
        }

        private class _MssqlState
        {
            public SqlDataReader Reader;
            public SqlCommand Command;

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
                    if (Reader == null) return;
                    Reader.Dispose();
                    Reader = null;
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
                    if (Command == null) return;
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
                    if (Command == null) return;
                    Command.Dispose();
                    Command = null;
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                }
            }
        }

        #endregion
    }

    public class MssqlCommandParameterOutput<T>
    {
        public T Value { get; set; }

        public static explicit operator T(MssqlCommandParameterOutput<T> p)
        {
            return p.Value;
        }

        public static implicit operator MssqlCommandParameterOutput<T>(T value)
        {
            return new MssqlCommandParameterOutput<T> { Value = value };
        }
    }
}