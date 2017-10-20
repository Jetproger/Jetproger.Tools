namespace Jetproger.Tools.Resource.Bases
{
    public class ResourceItem<T>
    {
        public string Key { get; private set; }
        public string Text { get; private set; }
        public bool IsDeclared { get; private set; }
        public bool IsValid { get; private set; }
        public T Value { get; private set; }

        public ResourceItem(string key, string text, bool isDeclared, bool isValid, T value)
        {
            Key = key;
            Text = text;
            IsDeclared = isDeclared;
            IsValid = isValid;
            Value = value;
        }
    }
}