namespace Jetproger.Tools.Convert.Traces
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class TraceAttribute : System.Attribute
    {
        public string Name { get; private set; }

        public TraceAttribute(string name) { Name = name; }
    }
}