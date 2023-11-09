namespace Jetproger.Tools.Convert.Factories
{
    [System.Serializable]
    public class SimpleCreator
    {
        public SimpleCreator(System.Func<object> func) { _func = func; }
        public object Create() { return _func(); }
        private System.Func<object> _func;
        public SimpleCreator() { }
    }

    [System.Serializable]
    public class ParamCreator
    {
        public ParamCreator(System.Func<object, object> func) { _func = func; }
        public object Create(object param) { return _func(param); }
        private System.Func<object, object> _func;
        public ParamCreator() { }
    }
}