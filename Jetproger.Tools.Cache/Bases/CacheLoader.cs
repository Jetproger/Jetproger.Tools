using System;
using System.Security.Permissions;

namespace Jetproger.Tools.Cache.Bases
{
    public class CacheLoader : MarshalByRefObject
    {
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Add(string[] keys, object value, int lifetime)
        {
            CacheCore.Add(keys, value, lifetime);
        }

        public bool Get(string[] keys, out object value)
        {
            return CacheCore.Get(keys, out value);
        }

        public void Off(string[] keys)
        {
            CacheCore.Off(keys);
        }
    }
}