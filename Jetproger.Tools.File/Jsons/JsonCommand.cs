using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Files = Tools.File;
using Tools;

namespace Jetproger.Tools.File.Jsons
{
    [Serializable]
    public class JsonCommand
    {
        private readonly List<JsonFilter> _filters = new List<JsonFilter>();
        private JsonCommandType _commandType;
        private JsonWhere _currentWhere;
        private Type _itemType;
        private object _item;

        public JsonCommand()
        {
        }

        public JsonCommand(Type itemType)
        {
            _currentWhere = new JsonWhere(itemType);
            _itemType = itemType;
        }

        public object Item
        {
            get { return _item; }
            set { _item = value; }
        }

        public int CommandType
        {
            get { return (int)_commandType; }
            set { _commandType = (JsonCommandType)value; }
        }

        public string ItemTypeName
        {
            get { return _itemType?.AssemblyQualifiedName; }
            set { _itemType = Type.GetType(value); }
        }

        public JsonFilter[] Filters
        {
            get { return _filters.ToArray(); }
            set { Files.Copy(value, _filters); }
        }

        public JsonCommand And(string condition, params object[] values)
        {
            _currentWhere?.And(condition, values);
            return this;
        }

        public JsonCommand From<TMaster, TDetail>(Expression<Func<TMaster, IList<TDetail>>> listGetter)
        {
            if (_currentWhere != null) _filters.Add(new JsonFilter(_currentWhere));
            var from = JsonFrom.From(listGetter);
            _filters.Add(new JsonFilter(from));
            _currentWhere = new JsonWhere(typeof(TDetail));
            return this;
        }

        public JsonCommand Select()
        {
            if (_currentWhere != null) _filters.Add(new JsonFilter(_currentWhere));
            _currentWhere = new JsonWhere(_itemType);
            _commandType = JsonCommandType.Select;
            return new JsonCommand(_itemType);
        }

        public JsonCommand Delete()
        {
            if (_currentWhere != null) _filters.Add(new JsonFilter(_currentWhere));
            _currentWhere = new JsonWhere(_itemType);
            _commandType = JsonCommandType.Delete;
            return new JsonCommand(_itemType);
        }

        public JsonCommand Insert(object item)
        {
            if (_currentWhere != null) _filters.Add(new JsonFilter(_currentWhere));
            _currentWhere = new JsonWhere(_itemType);
            _commandType = JsonCommandType.Insert;
            _item = item;
            return new JsonCommand(_itemType);
        }

        public JsonCommand Update(object item)
        {
            if (_currentWhere != null) _filters.Add(new JsonFilter(_currentWhere));
            _currentWhere = new JsonWhere(_itemType);
            _commandType = JsonCommandType.Update;
            _item = item;
            return new JsonCommand(_itemType);
        }

        public JsonTable Execute(IEnumerable items)
        {
            SetLimits();
            var resultItems = GetResultItems(items);
            if (_commandType == JsonCommandType.Select) return Select(resultItems);
            var sourceList = GetSourceList(items);
            if (sourceList == null || resultItems == null) return null;

            var jsonFile = items as JsonFile;
            if (jsonFile != null) jsonFile.IsModified = true;
            if (_commandType == JsonCommandType.Delete) return Delete(resultItems, sourceList);

            var enumerator = resultItems.GetEnumerator();
            var exists = enumerator.MoveNext();
            var firstItem = exists ? enumerator.Current : null;

            if (_commandType == JsonCommandType.Update) return Update(sourceList, firstItem);
            if (_commandType == JsonCommandType.Insert) return Insert(sourceList, firstItem);
            return null;
        }

        private void SetLimits()
        {
            var prevFilter = (JsonFilter)null;
            foreach (var filter in Filters)
            {
                if (filter.From != null && prevFilter?.Where != null)
                {
                    prevFilter.Where.Limit = 1;
                }
                prevFilter = filter;
            }
        }

        private IList GetSourceList(IEnumerable items)
        {
            var lastFilter = _filters[_filters.Count - 1];
            foreach (var filter in _filters)
            {
                if (!ReferenceEquals(filter, lastFilter)) items = filter.Apply(items);
            }
            return items as IList;
        }

        private IEnumerable GetResultItems(IEnumerable items)
        {
            foreach (var filter in Filters)
            {
                items = filter.Apply(items);
            }
            return items;
        }

        private JsonTable Select(IEnumerable resultItems)
        {
            return new JsonTable(resultItems.Cast<object>().ToArray());
        }

        private JsonTable Delete(IEnumerable resultItems, IList sourceList)
        {
            return sourceList is JsonFile ? DeleteFromJsonFile(resultItems.Cast<object>().ToArray(), sourceList) : DeleteFromList(resultItems.Cast<object>().ToArray(), sourceList);
        }

        private JsonTable DeleteFromJsonFile(IEnumerable resultItems, IList sourceList)
        {
            foreach (var item in resultItems)
            {
                sourceList.Remove(item);
            }
            return null;
        }

        private JsonTable DeleteFromList(IEnumerable resultItems, IList sourceList)
        {
            lock (sourceList)
            {
                foreach (var item in resultItems) sourceList.Remove(item);
            }
            return null;
        }

        private JsonTable Update(IList sourceList, object firstItem)
        {
            if (firstItem != null)
            {
                lock (firstItem)
                {
                    _item?.Copy(firstItem);
                }
            }
            return null;
        }

        private JsonTable Insert(IList sourceList, object firstItem)
        {
            if (firstItem != null)
            {
                lock (firstItem)
                {
                    _item?.Copy(firstItem);
                }
            }
            else
            {
                if (_item != null)
                {
                    if (sourceList is JsonFile) sourceList.Add(_item.Copy()); else lock (sourceList) { sourceList.Add(_item.Copy()); }
                }
            }
            return null;
        }
    }
}