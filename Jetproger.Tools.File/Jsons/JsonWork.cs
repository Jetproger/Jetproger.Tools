using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Files = Tools.File;

namespace Jetproger.Tools.File.Jsons
{
    public class JsonWorkWhere<T>
    {
        private readonly JsonWork _innerWork;

        public JsonWorkWhere()
        {
            _innerWork = new JsonWork(typeof(T));
        }

        public JsonWorkWhere(JsonWork innerWork)
        {
            _innerWork = innerWork;
        }

        public JsonWork<T> Where(string condition, params object[] values)
        {
            var jsonWork = new JsonWork<T>(_innerWork);
            jsonWork.And(condition, values);
            return jsonWork;
        }

        public JsonResult Execute()
        {
            return new JsonResult(Files.JsonDb.Execute(_innerWork.GetItemType(), _innerWork));
        }
    }

    public class JsonWork<T>
    {
        private readonly JsonWork _innerWork;

        public JsonWork()
        {
            _innerWork = new JsonWork(typeof(T));
        }

        public JsonWork(JsonWork innerWork)
        {
            _innerWork = innerWork;
        }

        public JsonWork<T> And(string condition, params object[] values)
        {
            _innerWork.And(condition, values);
            return this;
        }

        public JsonWorkWhere<TDetail> From<TDetail>(Expression<Func<T, IList<TDetail>>> listGetter)
        {
            _innerWork.From(listGetter);
            return new JsonWorkWhere<TDetail>(_innerWork);
        }

        public JsonWorkWhere<T> Select()
        {
            _innerWork.Select();
            return new JsonWorkWhere<T>(_innerWork);
        }

        public JsonWorkWhere<T> Delete()
        {
            _innerWork.Delete();
            return new JsonWorkWhere<T>(_innerWork);
        }

        public JsonWorkWhere<T> Insert(T item)
        {
            _innerWork.Insert(item);
            return new JsonWorkWhere<T>(_innerWork);
        }

        public JsonWorkWhere<T> Update(T item)
        {
            _innerWork.Update(item);
            return new JsonWorkWhere<T>(_innerWork);
        }

        public JsonResult Execute()
        {
            return new JsonResult(Files.JsonDb.Execute(typeof(T), _innerWork));
        }
    }

    public class JsonResult
    {
        private readonly JsonSet _jsonSet;

        public JsonResult()
        {
            _jsonSet = new JsonSet(new JsonTable[0]);
        }

        public JsonResult(JsonSet jsonSet)
        {
            _jsonSet = jsonSet ?? new JsonSet(new JsonTable[0]);
        }

        public IEnumerable<T> As<T>(int tableIndex = 0)
        {
            if (tableIndex < 0 || tableIndex >= _jsonSet.Tables.Length)
            {
                yield break;
            }
            foreach (var row in _jsonSet.Tables[tableIndex].Rows)
            {
                yield return (T)row;
            }
        }
    }

    [Serializable]
    public class JsonWork
    {
        private readonly List<JsonCommand> _commands = new List<JsonCommand>();
        private JsonCommand _currentCommand;
        private Type _itemType;
        public JsonWork() { }

        public JsonWork(Type itemType)
        {
            _currentCommand = new JsonCommand(itemType);
            _itemType = itemType;
        }

        public string ItemTypeName
        {
            get { return _itemType?.AssemblyQualifiedName; }
            set { _itemType = Type.GetType(value); }
        }

        public JsonCommand[] Commands
        {
            get { return _commands.ToArray(); }
            set { Files.Copy(value, _commands); }
        }

        public JsonWork And(string condition, params object[] values)
        {
            _currentCommand?.And(condition, values);
            return this;
        }

        public JsonWork From<TMaster, TDetail>(Expression<Func<TMaster, IList<TDetail>>> listGetter)
        {
            _currentCommand?.From(listGetter);
            return this;
        }

        public JsonWork Select()
        {
            if (_currentCommand == null) return this;
            _commands.Add(_currentCommand);
            _currentCommand = _currentCommand.Select();
            return this;
        }

        public JsonWork Delete()
        {
            if (_currentCommand == null) return this;
            _commands.Add(_currentCommand);
            _currentCommand = _currentCommand.Delete();
            return this;
        }

        public JsonWork Insert(object item)
        {
            if (_currentCommand == null) return this;
            _commands.Add(_currentCommand);
            _currentCommand = _currentCommand.Insert(item);
            return this;
        }

        public JsonWork Update(object item)
        {
            if (_currentCommand == null) return this;
            _commands.Add(_currentCommand);
            _currentCommand = _currentCommand.Update(item);
            return this;
        }

        public JsonSet Execute(IEnumerable items)
        {
            var tables = new List<JsonTable>();
            foreach (var command in _commands)
            {
                var table = command.Execute(items);
                if (table != null) tables.Add(table);
            }
            return new JsonSet(tables.ToArray());
        }

        public Type GetItemType()
        {
            return _itemType;
        }
    }
}