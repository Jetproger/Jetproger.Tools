using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Jetproger.Tools.File.Jsons
{
    [Serializable]
    public class JsonFrom
    {
        private string _listPropertyName;
        private Type _itemType;

        public JsonFrom() { }

        public JsonFrom(Type itemType, string listPropertyName)
        {
            _listPropertyName = listPropertyName;
            _itemType = itemType;
        }

        public string ItemTypeName
        {
            get { return _itemType?.AssemblyQualifiedName; }
            set { _itemType = Type.GetType(value); }
        }

        public string ListPropertyName
        {
            get { return _listPropertyName; }
            set { _listPropertyName = value; }
        }

        public IList Take(IList items)
        {
            if (items == null) return null;
            var enumerator = items.GetEnumerator();
            if (!enumerator.MoveNext()) return null;
            var item = enumerator.Current;
            var property = !string.IsNullOrWhiteSpace(_listPropertyName) ? _itemType.GetProperty(_listPropertyName) : null;
            return property?.GetValue(item, null) as IList;
        }

        public IEnumerable Apply(IEnumerable items)
        {
            return (from object item in items select (!string.IsNullOrWhiteSpace(_listPropertyName) ? _itemType.GetProperty(_listPropertyName) : null)?.GetValue(item, null) as IEnumerable).FirstOrDefault();
        }

        public static JsonFrom From<TMaster, TDetail>(Expression<Func<TMaster, IList<TDetail>>> listGetter)
        {
            return new JsonFrom(typeof(TMaster), GetMemberName(listGetter));
        }

        private static string GetMemberName(LambdaExpression memberSelector)
        {
            if (memberSelector == null) return null;
            var currentExpression = memberSelector.Body;
            while (true)
            {
                switch (currentExpression.NodeType)
                {
                    case ExpressionType.ArrayLength: return "Length";
                    case ExpressionType.Parameter: return ((ParameterExpression)currentExpression).Name;
                    case ExpressionType.Call: return ((MethodCallExpression)currentExpression).Method.Name;
                    case ExpressionType.MemberAccess: return ((MemberExpression)currentExpression).Member.Name;
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked: currentExpression = ((UnaryExpression)currentExpression).Operand; break;
                    case ExpressionType.Invoke: currentExpression = ((InvocationExpression)currentExpression).Expression; break;
                    default: return null;
                }
            }
        }
    }
}