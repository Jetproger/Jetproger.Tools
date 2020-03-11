namespace Jetproger.Tools.Cache.Bases
{
    public class CacheRemote : Jc.RemoteCaller
    {
        public void Write(object[] keys, object value, int lifetime)
        {
            CacheCore.Write(keys, value, lifetime);
        }

        public bool Read(object[] keys, out object value)
        {
            return CacheCore.Read(keys, out value);
        }

        public void Clear(object[] keys)
        {
            CacheCore.Clear(keys);
        }
    }
}