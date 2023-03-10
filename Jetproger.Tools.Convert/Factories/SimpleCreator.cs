using System;

namespace Jetproger.Tools.Convert.Factories
{
    [Serializable]
    public class SimpleCreator
    {
        private Func<object> _func;

        public SimpleCreator()
        {

        }

        public SimpleCreator(Func<object> func)
        {
            _func = func;
        }

        public object Create()
        {
            return _func();
        }
    }
}