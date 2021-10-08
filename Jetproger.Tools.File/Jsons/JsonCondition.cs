using System;
using System.Linq.Expressions;
using Tools;

namespace Jetproger.Tools.File.Jsons
{
    [Serializable]
    public class JsonCondition
    {
        private string _condition;
        private string[] _parameterTypeNames;
        private string[] _stringParameters;

        public JsonCondition()
        {
        }

        public JsonCondition(string condition, object[] parameters)
        {
            _condition = condition;
            SetParameters(parameters);
        }

        public string Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

        public string[] TypeNames
        {
            get { return _parameterTypeNames; }
            set { _parameterTypeNames = value; }
        }

        public string[] StringParameters
        {
            get { return _stringParameters; }
            set { _stringParameters = value; }
        }

        public Delegate GetFunction(Type itemType)
        {
            var p = Expression.Parameter(itemType, itemType.Name);
            var e = Bases.DynamicExpression.ParseLambda(new[] { p }, typeof(bool), _condition, GetParameters());
            return e.Compile();
        }

        private void SetParameters(object[] parameters)
        {
            var i = 0;
            _parameterTypeNames = new string[parameters.Length];
            _stringParameters = new string[parameters.Length];
            foreach (var parameter in parameters)
            {
                var value = parameter != DBNull.Value ? parameter : null;
                _parameterTypeNames[i] = value?.GetType().AssemblyQualifiedName;
                _stringParameters[i++] = value.AsString();
            }
        }

        private object[] GetParameters()
        {
            var parameters = new object[_parameterTypeNames.Length];
            var i = 0;
            foreach (var parameterTypeName in _parameterTypeNames)
            {
                var type = Type.GetType(parameterTypeName);
                var stringParameter = _stringParameters[i];
                parameters[i++] = type != null ? stringParameter.As(type) : null;
            }
            return parameters;
        }
    }
}