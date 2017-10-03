using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows.Forms;

namespace Tools
{
    public unsafe static partial class Native
    {
        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern Int32 SetParent(IntPtr hWndChild, IntPtr hWndParent);

        [DllImport("User32", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern Int32 ShowWindow(IntPtr hWndChild, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "DefMDIChildProcW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr DefMDIChildProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "DefWindowProcW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr DefFrameProc(IntPtr hWnd, IntPtr hWndClient, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetAsyncKeyState(int vKey);

        [DllImport("shell32.dll", EntryPoint = "ShellAboutW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ShellAbout(IntPtr hWnd, char* szApp, char* szOtherStuff, IntPtr hIcon);

        [DllImport("shell32.dll", EntryPoint = "ShellExecuteW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ShellExecute(IntPtr hWnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetSystemMetrics(Int32 nIndex);

        [DllImport("kernel32.dll", EntryPoint = "LoadLibraryW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr LoadLibrary(string lpLibFileName);

        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("user32.dll", EntryPoint = "SendMessageW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "PostMessageW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "GetDC", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "FillRect", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 FillRect(IntPtr hDc, RECT* pRect, Int32 nColor);

        [DllImport("user32.dll", EntryPoint = "DrawTextW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 DrawText(IntPtr hDc, char* pString, Int32 nCount, RECT* pRect, uint uFormat);

        [DllImport("user32.dll", EntryPoint = "ClientToScreen", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 ClientToScreen(IntPtr hWnd, POINT* lpPoint);

        [DllImport("user32.dll", EntryPoint = "ScreenToClient", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 ScreenToClient(IntPtr hWnd, POINT* lpPoint);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, Int32 nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, Int32 nIndex, IntPtr dwNewLong);

        [DllImport("kernel32.dll", EntryPoint = "GetLastError", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetLastError();

        [DllImport("kernel32.dll", EntryPoint = "CreateFileW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CreateFile(string szFileName, uint dwDesiredAccess, uint dwShareMode, uint lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", EntryPoint = "GetFileSize", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetFileSize(IntPtr hFile, int lpFileSizeHigh);

        [DllImport("kernel32.dll", EntryPoint = "WriteFile", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WriteFile(IntPtr hFile, void* lpBuffer, int nNumberOfBytesToWrite, int* lpNumberOfBytesWritten, int lpOverlapped);

        [DllImport("kernel32.dll", EntryPoint = "ReadFile", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ReadFile(IntPtr hFile, void* pBuffer, int nNumberOfBytesToRead, int* lpNumberOfBytesRead, int lpOverlapped);

        [DllImport("kernel32.dll", EntryPoint = "LockFile", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int LockFile(IntPtr hFile, int dwFileOffsetLow, int dwFileOffsetHigh, int nNumberOfBytesToLockLow, int nNumberOfBytesToLockHigh);

        [DllImport("kernel32.dll", EntryPoint = "UnlockFile", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnlockFile(IntPtr hFile, int dwFileOffsetLow, int dwFileOffsetHigh, int nNumberOfBytesToUnlockLow, int nNumberOfBytesToUnlockHigh);

        [DllImport("kernel32.dll", EntryPoint = "SetFilePointer", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern uint SetFilePointer(IntPtr hFile, int lDistanceToMove, int lpDistanceToMoveHigh, uint dwMoveMethod);

        [DllImport("kernel32.dll", EntryPoint = "SetEndOfFile", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetEndOfFile(IntPtr hFile);

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int CloseHandle(IntPtr hObject);

        [DllImport("netapi32.dll", EntryPoint = "NetApiBufferFree")]
        public static extern void NetApiBufferFree(IntPtr pointerBuffer);

        [DllImport("netapi32.dll", EntryPoint = "NetServerEnum")]
        public static extern uint NetServerEnum(IntPtr serverName, uint level, ref IntPtr pointerBuffer, uint prefMaxLen, ref uint entriesRead, ref uint totalEntries, uint serverType, [MarshalAs(UnmanagedType.LPWStr)] string domain, IntPtr resumeHandle);

        [DllImport("netapi32.dll", EntryPoint = "NetServerGetInfo")]
        public static extern uint NetServerGetInfo([MarshalAs(UnmanagedType.LPWStr)] string serverName, int level, ref IntPtr pointerBuffer);

        [DllImport("user32.dll")]
        public static extern IntPtr OpenInputDesktop(int dwFlags, [MarshalAs(UnmanagedType.Bool)] bool fInherit, int dwDesiredAccess);

        [DllImport("user32.dll")]
        public static extern bool CloseDesktop(IntPtr hDesktop);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO shinfo, uint cbfileInfo, int uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowThreadProcessId([In] IntPtr hWnd, [Out, Optional] IntPtr lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern ushort GetKeyboardLayout([In] int idThread);

        public static int MakeLong(int loWord, int hiWord)
        {
            return (hiWord << 16) | (loWord & 0xffff);
        }

        public static IntPtr MakeLParam(int loWord, int hiWord)
        {
            return (IntPtr)((hiWord << 16) | (loWord & 0xffff));
        }

        public static int HiWord(int nNumber)
        {
            return (nNumber >> 16) & 0xffff;
        }

        public static int LoWord(int nNumber)
        {
            return nNumber & 0xffff;
        }

        public static int ShellAbout(IntPtr ownerHandle, string title, string appName, string companyName)
        {
            fixed (char* szApp = title + "#" + appName)
            {
                fixed (char* szOtherStuff = companyName)
                {
                    return ShellAbout(ownerHandle, szApp, szOtherStuff, IntPtr.Zero);
                }
            }
        }

        public static void ShowCalendar(IntPtr owner)
        {
            ShellExecute(owner, "open", "Rundll32.exe", "shell32.dll,Control_RunDLL TIMEDATE.CPL", "C:\\WINDOWS\\SYSTEM32\\", WindowShows.SW_SHOWNORMAL);
        }

        public static IntPtr CreateFile(string fileName)
        {
            return CreateFile(fileName,
                (uint)(FileCodes.FILE_GENERIC_READ| FileCodes.FILE_GENERIC_WRITE),
                (uint)(FileCodes.FILE_SHARE_READ | FileCodes.FILE_SHARE_WRITE),
                0,
                (uint)FileCodes.OPEN_ALWAYS,
                (uint)FileCodes.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
        }

        public static int GetFileSize(IntPtr hFile)
        {
            return GetFileSize(hFile, 0);
        }

        public static bool LockFile(IntPtr hFile, int offset, int nNumberOfBytesToLock)
        {
            return LockFile(hFile, offset, 0, LoWord(nNumberOfBytesToLock), HiWord(nNumberOfBytesToLock)) != 0;
        }

        public static bool UnlockFile(IntPtr hFile, int offset, int nNumberOfBytesToUnlock)
        {
            return UnlockFile(hFile, offset, 0, LoWord(nNumberOfBytesToUnlock), HiWord(nNumberOfBytesToUnlock));
        }

        public static bool SetFilePointer(IntPtr hFile, int offset, SeekOrigin seekOrigin)
        {
            uint result = SetFilePointer(hFile, offset, 0, (uint)(seekOrigin == SeekOrigin.Begin ? FileCodes.FILE_BEGIN : seekOrigin == SeekOrigin.End ? FileCodes.FILE_END : FileCodes.FILE_CURRENT));
            return result == (int)FileCodes.INVALID_SET_FILE_POINTER;
        }

        public static bool WriteFile(IntPtr hFile, object data)
        {
            int nNumberOfBytesToWrite = 0;
            int size = Marshal.SizeOf(data.GetType());
            IntPtr pData = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(data, pData, true);
                return WriteFile(hFile, pData.ToPointer(), size, &nNumberOfBytesToWrite, 0);
            }
            finally
            {
                Marshal.FreeHGlobal(pData);
            }
        }

        public static object ReadFile(IntPtr hFile, Type type)
        {
            int nNumberOfBytesRead = 0;
            int size = Marshal.SizeOf(type);
            IntPtr pData = Marshal.AllocHGlobal(size);
            try
            {
                ReadFile(hFile, pData.ToPointer(), size, &nNumberOfBytesRead, 0);
                return nNumberOfBytesRead > 0 ? Marshal.PtrToStructure(pData, type) : null;
            }
            finally
            {
                Marshal.FreeHGlobal(pData);
            }
        }

        public static int GetMdiCreateStruct(string className, string title, IntPtr owner, int x, int y, int cx, int cy, uint style)
        {
            fixed (char* pszClass = className)
            {
                fixed (char* pszTitle = title)
                {
                    MDICREATESTRUCT mdi;
                    mdi.szClass = pszClass;
                    mdi.szTitle = pszTitle;
                    mdi.hOwner = owner;
                    mdi.x = x;
                    mdi.y = y;
                    mdi.cx = cx;
                    mdi.cy = cy;
                    mdi.style = style;
                    mdi.lParam = 0;
                    MDICREATESTRUCT* pMdi = &mdi;
                    var intPointer = new IntPtr(pMdi);
                    return intPointer.ToInt32();
                }
            }
        }

        public static sbyte WindowVisible(IntPtr lParam)
        {
            var p = (WindowPosStruct*)lParam.ToPointer();
            WindowPosStruct wp = *p;
            var flags = wp.flags;
            if ((flags & WindowPositions.SWP_HIDEWINDOW) == WindowPositions.SWP_HIDEWINDOW) return -1;
            if ((flags & WindowPositions.SWP_SHOWWINDOW) == WindowPositions.SWP_SHOWWINDOW) return 1;
            return 0;
        }

        public static POINT ClientToScreen(IntPtr hWnd, POINT pOld)
        {
            POINT* lpPointer = &pOld;
            UInt32 retcode = ClientToScreen(hWnd, lpPointer);
            if (retcode == 0) return pOld;
            POINT pNew = *lpPointer;
            return pNew;
        }

        public static POINT ScreenToClient(IntPtr hWnd, POINT pOld)
        {
            POINT* lpPointer = &pOld;
            UInt32 retcode = ScreenToClient(hWnd, lpPointer);
            if (retcode == 0) return pOld;
            POINT pNew = *lpPointer;
            return pNew;
        }

        public static int DrawText(IntPtr hDc, object value, RECT rect, uint format)
        {
            var text = value.AsString();
            fixed (char* pString = text)
            {
                return DrawText(hDc, pString, text.Length, &rect, format);
            }
        }

        public static string[] GetSqlServers()
        {
            var sqlServers = new LanCollection(ServerTypes.SV_TYPE_SQLSERVER);
            var list = new List<string>();
            foreach (var sqlServer in sqlServers) list.Add(sqlServer.AsString());
            return list.ToArray();
        }

        public static SHFILEINFO ShellInfo(string fileName)
        {
            const int flags = (
                ShellFileInfos.SHGFI_DISPLAYNAME |
                ShellFileInfos.SHGFI_ICON |
                ShellFileInfos.SHGFI_TYPENAME |
                ShellFileInfos.SHGFI_USEFILEATTRIBUTES);
            const int fileAttributes = (int)FileCodes.FILE_ATTRIBUTE_NORMAL;
            var shellFileInfo = new SHFILEINFO(true);
            var size = (uint)Marshal.SizeOf(shellFileInfo);
            var fullName = FileNameAsPathFileExt(fileName);
            var ptr = SHGetFileInfo(fullName, fileAttributes, ref shellFileInfo, size, flags);
            if (ptr == IntPtr.Zero)
            {
                var ext = Path.GetExtension(fullName) ?? "";
                if (ext.StartsWith(".")) ext = ext.Substring(1);
                shellFileInfo.szTypeName = $"Файл \"{ext}\"";
                shellFileInfo.hIcon = IntPtr.Zero;
            }
            return shellFileInfo;
        }

        public static MdiClient MdiClientDisableScrolling(Form form)
        {
            var nonScrollableMdiClient = new NonScrollableMdiClient();
            foreach (Control control in form.Controls)
            {
                if (!(control is MdiClient)) continue;
                var oldWindowStyle = GetWindowLong(control.Handle, WindowLongs.GWL_EXSTYLE).ToInt64();
                var newWindowStyle = new IntPtr(oldWindowStyle ^ WindowStyles.WS_EX_CLIENTEDGE);
                SetWindowLong(control.Handle, WindowLongs.GWL_EXSTYLE, newWindowStyle);
                nonScrollableMdiClient.AssignHandle(control.Handle);
                control.HandleDestroyed += nonScrollableMdiClient.OnHandleDestroyed;
                return control as MdiClient;
            }
            return null;
        }

        public static string GetKeyboardLayout()
        {
            switch (GetKeyboardLayout(GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero)))
            {
                case 1049: return "RU";
                case 1033: return "EN";
                default: return "";
            }
        }

        private static readonly CultureInfo Formatter = new CultureInfo("en-us") {
            NumberFormat =
            {
                NumberGroupSeparator = string.Empty, NumberDecimalSeparator = "."
            },
            DateTimeFormat =
            {
                DateSeparator = "-", TimeSeparator = ":"
            }
        };

        private static string AsString(this object value)
        {
            if (value == null || value == DBNull.Value) return default(string);
            if (value is string) return (string)value;
            if (value is bool) return (bool)value ? "1" : "0";
            if (value is long) return ((long)value).ToString("#################0", Formatter);
            if (value is decimal) return ((decimal)value).ToString("#################0.00", Formatter);
            if (value is int) return ((int)value).ToString("#################0", Formatter);
            if (value is short) return ((short)value).ToString("#################0", Formatter);
            if (value is byte) return ((byte)value).ToString("#################0", Formatter);
            if (value is ulong) return ((ulong)value).ToString("#################0", Formatter);
            if (value is uint) return ((uint)value).ToString("#################0", Formatter);
            if (value is ushort) return ((ushort)value).ToString("#################0", Formatter);
            if (value is sbyte) return ((sbyte)value).ToString("#################0", Formatter);
            if (value is float) return ((float)value).ToString("#################0.00", Formatter);
            if (value is double) return ((double)value).ToString("#################0.00", Formatter);
            if (value is Guid) return ((Guid)value).ToString();
            if (value is DateTime) return ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.fff", Formatter);
            if (value is char) return ((char)value).ToString(CultureInfo.InvariantCulture);
            if (value is char[]) return string.Concat((char[])value);
            return value.ToString();
        }

        private static string FileNameAsPathFileExt(string value)
        {
            var pathFileExt = value;
            if (string.IsNullOrWhiteSpace(pathFileExt))
            {
                pathFileExt = Path.Combine(AppDir(), TempFile());
                return pathFileExt;
            }
            var file = Path.GetFileName(pathFileExt);
            if (file == pathFileExt)
            {
                pathFileExt = Path.Combine(AppDir(), file);
                return pathFileExt;
            }
            var path = !string.IsNullOrWhiteSpace(file) ? Path.GetDirectoryName(pathFileExt) : pathFileExt;
            file = !string.IsNullOrWhiteSpace(file) ? file : TempFile();
            if (string.IsNullOrWhiteSpace(path))
            {
                pathFileExt = Path.Combine(AppDir(), file);
                return pathFileExt;
            }
            pathFileExt = Path.Combine(path, file);
            return pathFileExt;
        }

        private static string AppDir()
        {
            var httpContext = HttpContext.Current;
            var path = httpContext == null ? AppDomain.CurrentDomain.BaseDirectory : httpContext.Server.MapPath("~");
            return path ?? string.Empty;
        }

        private static string TempFile()
        {
            return $"{Guid.NewGuid()}.tmp";
        }
    }
}