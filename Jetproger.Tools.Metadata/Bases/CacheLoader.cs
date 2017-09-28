using System;
using System.Security.Permissions;

namespace Jetproger.Tools.Metadata.Bases
{
    public class CacheLoader : MarshalByRefObject
    {
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Add(string[] keys, object value)
        {
            CacheCore.Add(keys, value);
        }

        public bool Get(string[] keys, out object value)
        {
            return CacheCore.Get(keys, out value);
        }
    }
}