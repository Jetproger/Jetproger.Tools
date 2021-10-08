using System;
using System.Collections;
using System.Collections.Generic;
using Files = Tools.File;


namespace Jetproger.Tools.File.Jsons
{
    [Serializable]
    public class JsonWhere
    {
        private readonly List<JsonCondition> _conditions = new List<JsonCondition> ();
        private Type _itemType;
        private int _limit;

        public JsonWhere(Type itemType)
        {
            _itemType = itemType;
        }

        public int Limit
        {
            get { return _limit; }
            set { _limit = value; }
        }

        public string ItemTypeName
        {
            get { return _itemType?.AssemblyQualifiedName; }
            set { _itemType = Type.GetType(value); }
        }

        public JsonCondition[] Conditions
        {
            get { return _conditions.ToArray(); }
            set { Files.Copy(value, _conditions); }
        }

        public void And(string condition, params object[] values)
        {
            if (condition != null) _conditions.Add(new JsonCondition(condition, values));
        }

        public static JsonWhere Where(Type itemType, string condition, params object[] values)
        {
            var whereWhere = new JsonWhere(itemType);
            whereWhere.And(condition, values);
            return whereWhere;
        }

        public IList Take(IList items)
        {
            return items;
        }

        public IEnumerable Apply(IEnumerable items)
        {
            if (items == null) yield break;
            var funcs = new Delegate[_conditions.Count];
            var i = 0;
            foreach (var condition in _conditions)
            {
                funcs[i++] = condition.GetFunction(_itemType);
            }
            var counter = 0;
            var limit = _limit > 0 ? _limit : int.MaxValue;
            foreach (var item in items)
            {
                if (counter >= limit) yield break;
                var isMatch = true;
                foreach (var func in funcs)
                {
                    if ((bool)func.DynamicInvoke(item)) continue;
                    isMatch = false;
                    break;
                }
                if (isMatch)
                {
                    counter++;
                    yield return item;
                }
            }
        }
    }
}