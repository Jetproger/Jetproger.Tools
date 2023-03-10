using System;

namespace Jetproger.Tools.Convert.Factories
{
    [Serializable]
    public class ParameterizedCreator
    {
        private Func<object, object> _func;

        public ParameterizedCreator()
        {

        }

        public ParameterizedCreator(Func<object, object> func)
        {
            _func = func;
        }

        public object Create(object parameter)
        {
            return _func(parameter);
        }
    }
}