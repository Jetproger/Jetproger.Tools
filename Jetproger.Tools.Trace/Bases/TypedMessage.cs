namespace Jetproger.Tools.Trace.Bases
{
    public abstract class TypedMessage
    {
        public readonly string Message;

        protected TypedMessage(string message)
        {
            Message = (message ?? string.Empty).Trim(' ', '\t', '\r', '\n');
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}