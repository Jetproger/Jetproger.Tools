using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.Native
{
    public delegate IntPtr WindowProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    public delegate IntPtr GetMsgProc(int code, IntPtr wParam, IntPtr lParam);

    public delegate void TimerProc(IntPtr hWnd, uint uMsg, IntPtr nIdEvent, uint dwTickCountMillis);

    public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    [StructLayout(LayoutKind.Sequential)]
    public struct BitmapInfoHeader
    {
        public uint Size;
        public int Width;
        public int Height;
        public ushort Planes;
        public ushort BitCount;
        public int CompressionMode;
        public uint SizeImage;
        public int XPxPerMeter;
        public int YPxPerMeter;
        public uint ClrUsed;
        public uint ClrImportant;
    }

    public struct BitmapInfo
    {
        public BitmapInfoHeader Header;
        public RgbQuad[] Colors;

        public static NativeBitmapInfoHandle NativeAlloc(ref BitmapInfo bitmapInfo)
        {
            return new NativeBitmapInfoHandle(ref bitmapInfo);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RgbQuad
    {
        public byte Blue;
        public byte Green;
        public byte Red;
        private byte Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Bitmap
    {
        public int Type;
        public int Width;
        public int Height;
        public int WidthBytes;
        public ushort Planes;
        public ushort BitsPerPixel;
        public IntPtr Bits;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SystemInfo
    {
        public ushort ProcessorArchitecture;
        ushort Reserved;
        public uint PageSize;
        public IntPtr MinimumApplicationAddress;
        public IntPtr MaximumApplicationAddress;
        public IntPtr ActiveProcessorMask;
        public uint NumberOfProcessors;
        public uint ProcessorType;
        public uint AllocationGranularity;
        public ushort ProcessorLevel;
        public ushort ProcessorRevision;
        public uint OemId => ((uint)this.ProcessorArchitecture << 8) | this.Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SecurityAttributes
    {
        public uint Length;
        public IntPtr SecurityDescriptor;
        public uint IsHandleInheritedValue;
        public bool IsHandleInherited => this.IsHandleInheritedValue > 0;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FileTime
    {
        public uint Low;
        public uint High;
        public ulong Value => ((ulong)this.High << 32) | this.Low;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SystemTime
    {
        public ushort Year;
        public ushort Month;
        public ushort DayOfWeek;
        public ushort Day;
        public ushort Hour;
        public ushort Minute;
        public ushort Second;
        public ushort Milliseconds;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FileAttributeData
    {
        public FileAttributes Attributes;
        public FileTime CreationTime;
        public FileTime LastAccessTime;
        public FileTime LastWriteTime;
        public uint FileSizeHigh;
        public uint FileSizeLow;
        public ulong FileSize => ((ulong)this.FileSizeHigh << 32) | this.FileSizeLow;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Message
    {
        public IntPtr Hwnd;
        public uint Value;
        public IntPtr WParam;
        public IntPtr LParam;
        public uint Time;
        public Point Point;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct PaintStruct
    {
        public IntPtr HandleDC;
        public int EraseBackgroundValue;
        public Rectangle PaintRect;
        private int ReservedInternalRestore;
        private int ReservedInternalIncUpdate;
        private fixed byte ReservedInternalRgb[32];

        public bool ShouldEraseBackground
        {
            get { return this.EraseBackgroundValue > 0; }
            set { this.EraseBackgroundValue = value ? 1 : 0; }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WindowClassEx
    {
        public uint Size;
        public int Styles;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WindowProc WindowProc;
        public int ClassExtraBytes;
        public int WindowExtraBytes;
        public IntPtr InstanceHandle;
        public IntPtr IconHandle;
        public IntPtr CursorHandle;
        public IntPtr BackgroundBrushHandle;
        public string MenuName;
        public string ClassName;
        public IntPtr SmallIconHandle;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct WindowClassExBlittable
    {
        public uint Size;
        public int Styles;
        public IntPtr WindowProc;
        public int ClassExtraBytes;
        public int WindowExtraBytes;
        public IntPtr InstanceHandle;
        public IntPtr IconHandle;
        public IntPtr CursorHandle;
        public IntPtr BackgroundBrushHandle;
        public IntPtr MenuName;
        public IntPtr ClassName;
        public IntPtr SmallIconHandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WindowInfo
    {
        public uint Size;
        public Rectangle WindowRect;
        public Rectangle ClientRect;
        public int Styles;
        public int ExStyles;
        public uint WindowStatus;
        public uint BorderX;
        public uint BorderY;
        public ushort WindowType;
        public ushort CreatorVersion;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CreateStruct
    {
        public IntPtr CreateParams;
        public IntPtr InstanceHandle;
        public IntPtr MenuHandle;
        public IntPtr ParentHwnd;
        public int Height;
        public int Width;
        public int Y;
        public int X;
        public int Styles;
        public IntPtr Name;
        public IntPtr ClassName;
        public int ExStyles;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WindowPlacement
    {
        public uint Size;
        public int Flags;
        public int ShowCmd;
        public Point MinPosition;
        public Point MaxPosition;
        public Rectangle NormalPosition;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BlendFunction
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AnimationInfo
    {
        public AnimationInfo(int iMinAnimate)
        {
            this.Size = (uint)Marshal.SizeOf<AnimationInfo>();
            this.MinAnimate = iMinAnimate;
        }
        public uint Size;
        public int MinAnimate;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MinimizedMetrics
    {
        public uint Size;
        public int Width;
        public int HorizontalGap;
        public int VerticalGap;
        public int Arrange;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TrackMouseEventOptions
    {
        public uint Size;
        public int Flags;
        public IntPtr TrackedHwnd;
        public uint HoverTime;

        public const uint DefaultHoverTime = 0xFFFFFFFF;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MinMaxInfo
    {
        Point Reserved;
        public Point MaxSize;
        public Point MaxPosition;
        public Point MinTrackSize;
        public Point MaxTrackSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WindowPosition
    {
        public IntPtr Hwnd;
        public IntPtr HwndZOrderInsertAfter;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public int Flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct NcCalcSizeParams
    {
        public NcCalcSizeRegionUnion Region;
        public WindowPosition* Position;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct NcCalcSizeRegionUnion
    {
        [FieldOffset(0)]
        public NcCalcSizeInput Input;
        [FieldOffset(0)]
        public NcCalcSizeOutput Output;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NcCalcSizeInput
    {
        public Rectangle TargetWindowRect;
        public Rectangle CurrentWindowRect;
        public Rectangle CurrentClientRect;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NcCalcSizeOutput
    {
        public Rectangle TargetClientRect;
        public Rectangle DestRect;
        public Rectangle SrcRect;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MonitorInfo
    {
        public uint Size;
        public Rectangle MonitorRect;
        public Rectangle WorkRect;
        public int Flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TitleBarInfo
    {
        public uint Size;
        public Rectangle TitleBarRect;
        public int TitleBarStates;
        private int Reserved;
        public int MinimizeButtonStates;
        public int MaximizeButtonStates;
        public int HelpButtonStates;
        public int CloseButtonStates;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WindowCompositionAttributeData
    {
        public int Attribute;
        public IntPtr Data;
        public int DataSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AccentPolicy
    {
        public int AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WindowThemeAttributeOptions
    {
        public uint Flags;
        public uint Mask;
    }

    public struct DrawThemeBackgroundOptions
    {
        public uint Size;
        public int Flags;
        public Rectangle ClipRect;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct RECT
    {
        [FieldOffset(0)]public int left;
        [FieldOffset(4)]public int top;
        [FieldOffset(8)]public int right;
        [FieldOffset(12)]public int bottom;
    }

    public struct POINT
    {
        public long x;
        public long y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WindowPosStruct
    {
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public uint flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CERT_PUBLIC_KEY_INFO
    {
        public CRYPT_ALGORITHM_IDENTIFIER Algorithm;
        public CRYPT_BIT_BLOB PublicKey;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CRYPT_ALGORITHM_IDENTIFIER
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public String pszObjId;
        public CRYPT_OBJID_BLOB Parameters;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CRYPT_OBJID_BLOB
    {
        public uint cbData;
        public IntPtr pbData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CRYPT_BIT_BLOB
    {
        public uint cbData;
        public IntPtr pbData;
        public uint cUnusedBits;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct MDICREATESTRUCT
    {
        public char* szClass;
        public char* szTitle;
        public IntPtr hOwner;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public uint style;
        public int lParam;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SERVER_INFO_101
    {
        public int dwPlatformID;
        public IntPtr lpszServerName;
        public int dwVersionMajor;
        public int dwVersionMinor;
        public int dwType;
        public IntPtr lpszComment;
    }

    public struct TVINSERTSTRUCT
    {
        public IntPtr hParent;
        public IntPtr hInsertAfter;
        public TVITEM item;
    }

    public unsafe struct TVITEM
    {
        public uint mask;
        public IntPtr hItem;
        public uint state;
        public uint stateMask;
        public char* pszText;
        public int cchTextMax;
        public int iImage;
        public int iSelectedImage;
        public int cChildren;
        public IntPtr lParam;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SHFILEINFO
    {
        public SHFILEINFO(bool b)
        {
            hIcon = IntPtr.Zero;
            iIcon = 0;
            dwAttributes = 0;
            szDisplayName = "";
            szTypeName = "";
        }
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]public string szTypeName;
    }

    public class NonScrollableMdiClient : NativeWindow
    {
        public void OnHandleDestroyed(object sender, EventArgs e)
        {
            ReleaseHandle();
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == Je.win.WM_SCROLL)
            {
                return;
            }
            base.WndProc(ref m);
        }
    }

    public class LanCollection : IEnumerable
    {
        private string _domainName;
        private uint _serverType;

        public LanCollection()
        {
            Type = (uint)Je.win.SV_NONE;
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
            var result = (uint)Je.win.SV_NONE;
            IntPtr serverInfoPtr = IntPtr.Zero;
            uint status = Je.win.NetServerGetInfo(serverName, 101, ref serverInfoPtr);
            if (status != 0)
            {
                var serverInfo = (SERVER_INFO_101)Marshal.PtrToStructure(serverInfoPtr, typeof(SERVER_INFO_101));
                result = (uint)serverInfo.dwType;
                Je.win.NetApiBufferFree(serverInfoPtr);
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
                _serverInfo101Size = sizeof(SERVER_INFO_101);
            }

            protected internal LanEnumerator(uint serverType, string domainName = null)
            {
                const uint level = 101;
                const uint prefMaxLen = 0xFFFFFFFF;
                uint entriesRead = 0;
                uint totalEntries = 0;
                Reset();
                _serverInfoPtr = IntPtr.Zero;
                Je.win.NetServerEnum(
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
                    int newOffset = _serverInfoPtr.ToInt32() + _serverInfo101Size * _currentItem;
                    var serverInfo = (SERVER_INFO_101)Marshal.PtrToStructure(new IntPtr(newOffset), typeof(SERVER_INFO_101));
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
                Je.win.NetApiBufferFree(_serverInfoPtr);
                _serverInfoPtr = IntPtr.Zero;
            }
        }
    }

    public class NativeBitmapInfoHandle : CriticalHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        public IntPtr GetDangerousHandle() => handle;

        protected override bool ReleaseHandle()
        {
            Marshal.FreeHGlobal(this.handle);
            return true;
        }

        public unsafe NativeBitmapInfoHandle(ref BitmapInfo bitmapInfo) : base(new IntPtr(0))
        {
            var quads = bitmapInfo.Colors;
            var quadsLength = quads.Length;
            if (quadsLength == 0) { quadsLength = 1; }
            var success = false;
            var ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(Marshal.SizeOf<BitmapInfoHeader>() + Marshal.SizeOf<RgbQuad>() * quadsLength);
                var headerPtr = (BitmapInfoHeader*)ptr.ToPointer();
                *headerPtr = bitmapInfo.Header;
                var quadPtr = (RgbQuad*)(headerPtr + 1);
                var i = 0;
                for (; i < quads.Length; i++) { *(quadPtr + i) = quads[i]; }
                if (i == 0) { *quadPtr = new RgbQuad(); }
                this.SetHandle(ptr);
                success = true;
            }
            finally
            {
                if (!success)
                {
                    SetHandleAsInvalid();
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardInputState
    {
        public uint Value;

        public KeyboardInputState(uint value)
        {
            this.Value = value;
        }

        public uint RepeatCount
        {
            get { return Value & 0x0000ffff; }
            set { Value = Je.win.LoWord(Value); }
        }

        public uint ScanCode
        {
            get { return (Value >> 16) & 0x000000ff; }
            set
            {
                var mask = 0x00ff0000U;
                var newValue = (value << 16) & mask;
                Value = Value & ~mask | newValue;
            }
        }

        public bool IsExtendedKey
        {
            get { return unchecked(((int)this.Value >> 24) & 0x1) == 1; }
            set
            {
                var mask = 0U;//0b0000_0001_0000_0000_0000_0000_0000_0000U;
                this.Value = this.Value & ~mask | (value ? mask : 0U);
            }
        }

        public bool IsContextual
        {
            get { return unchecked(((int)this.Value >> 29) & 0x1) == 1; }
            set
            {
                var mask = 0U;//b0010_0000_0000_0000_0000_0000_0000_0000U;
                this.Value = this.Value & ~mask | (value ? mask : 0U);
            }
        }

        public bool IsPreviousKeyStatePressed
        {
            get { return unchecked(((int)this.Value >> 30) & 0x1) == 1; }
            set
            {
                var mask = 0U;//0b0100_0000_0000_0000_0000_0000_0000_0000U;
                this.Value = this.Value & ~mask | (value ? mask : 0U);
            }
        }

        public bool IsKeyUpTransition
        {
            get { return unchecked(((int)this.Value >> 31) & 0x1) == 1; }
            set
            {
                var mask = 0U;//0b1000_0000_0000_0000_0000_0000_0000_0000U;
                this.Value = this.Value & ~mask | (value ? mask : 0U);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HardwareInput
    {
        public uint Message;
        public ushort Low;
        public ushort High;

        public uint WParam
        {
            get { return ((uint)this.High << 16) | this.Low; }
            set
            {
                this.Low = (ushort)value;
                this.High = (ushort)(value >> 16);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MouseInput
    {
        public int X;
        public int Y;
        public uint Data;
        public int Flags;
        public uint Time;
        public IntPtr ExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardInput
    {
        public ushort VirtualKeyCode;
        public ushort ScanCode;
        public int Flags;
        public uint Time;
        public IntPtr ExtraInfo;

        public int Key
        {
            get { return VirtualKeyCode; }
            set { VirtualKeyCode = (ushort)value; }
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct InputPacket
    {
        [FieldOffset(0)]
        public MouseInput MouseInput;

        [FieldOffset(0)]
        public KeyboardInput KeyboardInput;

        [FieldOffset(0)]
        public HardwareInput HardwareInput;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Input
    {
        public int Type;
        public InputPacket Packet;

        public static void InitHardwareInput(out Input input, uint message, ushort low, ushort high)
        {
            input = new Input
            {
                Type = Je.win.INPUT_HARDWARE,
                Packet = new InputPacket
                {
                    HardwareInput = new HardwareInput
                    {
                        Message = message,
                        Low = low,
                        High = high
                    }
                }
            };
        }

        public static void InitHardwareInput(out Input input, uint message, uint wParam)
        {
            InitHardwareInput(out input, message, (ushort)wParam, (ushort)(wParam >> 16));
        }

        public static void InitKeyboardInput(out Input input, ushort scanCode, bool isKeyUp, bool isExtendedKey = false, uint timestampMillis = 0)
        {
            input = new Input
            {
                Type = Je.win.INPUT_KEYBOARD,
                Packet = new InputPacket
                {
                    KeyboardInput =
                    {
                        Time = timestampMillis,
                        Flags = Je.win.KEYEVENTF_SCANCODE,
                        ScanCode = scanCode,
                        VirtualKeyCode = 0
                    }
                }
            };
            if (isKeyUp) input.Packet.KeyboardInput.Flags |= Je.win.KEYEVENTF_KEYUP;
            if (isExtendedKey) input.Packet.KeyboardInput.Flags |= Je.win.KEYEVENTF_EXTENDEDKEY;
        }

        public static void InitKeyboardInput(out Input input, char charCode, bool isKeyUp, uint timestampMillis = 0)
        {
            input = new Input
            {
                Type = Je.win.INPUT_KEYBOARD,
                Packet = new InputPacket
                {
                    KeyboardInput =
                    {
                        Time = timestampMillis,
                        Flags = Je.win.KEYEVENTF_UNICODE,
                        ScanCode = charCode,
                        VirtualKeyCode = 0
                    }
                }
            };
            if (isKeyUp) input.Packet.KeyboardInput.Flags |= Je.win.KEYEVENTF_KEYUP;
        }

        public static void InitKeyboardInput(out Input input, int key, bool isKeyUp, uint timestampMillis = 0)
        {
            input = new Input
            {
                Type = Je.win.INPUT_KEYBOARD,
                Packet = new InputPacket
                {
                    KeyboardInput =
                    {
                        Time = timestampMillis,
                        Key = key,
                        ScanCode = 0,
                        Flags = 0
                    }
                }
            };
            if (isKeyUp) input.Packet.KeyboardInput.Flags |= Je.win.KEYEVENTF_KEYUP;
        }

        public static void InitMouseInput(out Input input, int x, int y, int flags, uint data = 0, uint timestampMillis = 0)
        {
            input = new Input
            {
                Type = Je.win.INPUT_MOUSE,
                Packet = new InputPacket
                {
                    MouseInput =
                    {
                        Time = timestampMillis,
                        X = x,
                        Y = y,
                        Data = data,
                        Flags = flags
                    }
                }
            };
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LastInputInfo
    {
        public uint Size;
        public uint Time;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KeyState
    {
        public short Value;

        public KeyState(short value)
        {
            Value = value;
        }

        public bool IsPressed
        {
            get { return (this.Value & 0x8000) > 0; }
            set
            {
                if (value) this.Value = unchecked((short)(this.Value | 0x8000));
                else this.Value = unchecked((short)(this.Value & 0x7fff));
            }
        }

        public bool IsToggled
        {
            get { return (this.Value & 0x1) == 1; }
            set { if (value) Value = unchecked((short)(this.Value | 0x1)); else Value = unchecked((short)(this.Value & 0xfffe)); }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CursorInfo
    {
        public uint Size;
        public int Flags;
        public IntPtr CursorHandle;
        public Point ScreenPosition;
    }
}