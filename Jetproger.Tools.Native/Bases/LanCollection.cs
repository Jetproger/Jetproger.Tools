using System;
using System.Collections;
using System.Net;
using System.Runtime.InteropServices;

namespace Jetproger.Tools.WinApi
{
    public static unsafe partial class Native
    {
        public class LanCollection : IEnumerable
        {
            private uint _serverType;
            private string _domainName;

            public LanCollection()
            {
                Type = ServerTypes.SV_NONE;
            }

            public LanCollection(uint serverType)
            {
                Type = serverType;
            }

            public uint Type
            {
                get { return _serverType; }
                set { _serverType = value; }
            }

            public string DomainName
            {
                get { return _domainName; }
                set { _domainName = value; }
            }

            public IEnumerator GetEnumerator()
            {
                return new LanEnumerator(_serverType, _domainName);
            }

            public static uint GetServerType(string serverName)
            {
                var result = ServerTypes.SV_NONE;
                IntPtr serverInfoPtr = IntPtr.Zero;
                uint status = NetServerGetInfo(serverName, 101, ref serverInfoPtr);
                if (status != 0)
                {
                    var serverInfo = (SERVER_INFO_101)Marshal.PtrToStructure(serverInfoPtr, typeof(SERVER_INFO_101));
                    result = (uint) serverInfo.dwType;
                    NetApiBufferFree(serverInfoPtr);
                }
                return result;
            }

            private class LanEnumerator : IEnumerator
            {
                private IntPtr _serverInfoPtr;
                private int _currentItem;
                private readonly uint _itemCount;
                private string _currentServerName;
                private static readonly int _serverInfo101Size;

                static unsafe LanEnumerator()
                {
                    _serverInfo101Size = sizeof (SERVER_INFO_101);
                }

                protected internal LanEnumerator(uint serverType, string domainName = null)
                {
                    const uint level = 101;
                    const uint prefMaxLen = 0xFFFFFFFF;
                    uint entriesRead = 0;
                    uint totalEntries = 0;
                    Reset();
                    _serverInfoPtr = IntPtr.Zero;
                    NetServerEnum(
                        IntPtr.Zero, // Server Name: Reserved; must be NULL. 		
                        level,
                        // Return server names, types, and associated software. The bufptr parameter points to an array of SERVER_INFO_101 structures.			
                        ref _serverInfoPtr, // Pointer to the buffer that receives the data.			
                        prefMaxLen, // Specifies the preferred maximum length of returned data, in bytes.			
                        ref entriesRead, // count of elements actually enumerated.			
                        ref totalEntries, // total number of visible servers and workstations on the network			
                        serverType, // value that filters the server entries to return from the enumeration			
                        null,
                        // Pointer to a constant string that specifies the name of the domain for which a list of servers is to be returned.			
                        IntPtr.Zero); // Reserved; must be set to zero. 		
                    _itemCount = entriesRead;
                }

                public object Current
                {
                    get { return _currentServerName; }
                }

                public bool MoveNext()
                {
                    bool result = false;
                    if (++_currentItem < _itemCount)
                    {
                        int newOffset = _serverInfoPtr.ToInt32() + _serverInfo101Size*_currentItem;
                        var serverInfo = (SERVER_INFO_101)Marshal.PtrToStructure(new IntPtr(newOffset), typeof (SERVER_INFO_101));
                        _currentServerName = Marshal.PtrToStringAuto(serverInfo.lpszServerName);
                        result = true;
                    }
                    return result;
                }

                public void Reset()
                {
                    Dns.GetHostEntry("");
                    _currentItem = -1;
                    _currentServerName = null;
                }

                ~LanEnumerator()
                {
                    if (_serverInfoPtr.Equals(IntPtr.Zero)) return;
                    NetApiBufferFree(_serverInfoPtr);
                    _serverInfoPtr = IntPtr.Zero;
                }
            }
        }
    }
}