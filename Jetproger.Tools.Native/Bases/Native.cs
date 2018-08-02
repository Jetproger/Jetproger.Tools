using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Jetproger.Tools.Convert.Bases;

namespace Jetproger.Tools.WinApi
{
    public unsafe static partial class Native
    {
        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static bool ReleaseCapture(this INativeExpander expander) { return ReleaseCapture(); }
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public static IntPtr CallWindowProc(this INativeExpander expander, IntPtr lpPrevWndFunc, IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam) { return CallWindowProc(lpPrevWndFunc, hWnd, uMsg, wParam, lParam); }
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        public static Int32 SetParent(this INativeExpander expander, IntPtr hWndChild, IntPtr hWndParent) { return SetParent(hWndChild, hWndParent); }
        [DllImport("User32", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern Int32 SetParent(IntPtr hWndChild, IntPtr hWndParent);

        public static Int32 ShowWindow(this INativeExpander expander, IntPtr hWndChild, int nCmdShow) { return ShowWindow(hWndChild, nCmdShow); }
        [DllImport("User32", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern Int32 ShowWindow(IntPtr hWndChild, int nCmdShow);

        public static IntPtr DefMDIChildProc(this INativeExpander expander, IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam) { return DefMDIChildProc(hWnd, uMsg, wParam, lParam); }
        [DllImport("user32.dll", EntryPoint = "DefMDIChildProcW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr DefMDIChildProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        public static IntPtr DefWindowProc(this INativeExpander expander, IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam) { return DefWindowProc(hWnd, uMsg, wParam, lParam); }
        [DllImport("user32.dll", EntryPoint = "DefWindowProcW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam);

        public static IntPtr DefFrameProc(this INativeExpander expander, IntPtr hWnd, IntPtr hWndClient, int msg, IntPtr wParam, IntPtr lParam) { return DefFrameProc(hWnd, hWndClient, msg, wParam, lParam); }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr DefFrameProc(IntPtr hWnd, IntPtr hWndClient, int msg, IntPtr wParam, IntPtr lParam);

        public static bool IsWindow(this INativeExpander expander, IntPtr hWnd) { return IsWindow(hWnd); }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindow(IntPtr hWnd);

        public static bool IsWindowVisible(this INativeExpander expander, IntPtr hWnd) { return IsWindowVisible(hWnd); }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        public static bool GetAsyncKeyState(this INativeExpander expander, int vKey) { return GetAsyncKeyState(vKey); }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetAsyncKeyState(int vKey);

        [DllImport("shell32.dll", EntryPoint = "ShellAboutW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ShellAbout(IntPtr hWnd, char* szApp, char* szOtherStuff, IntPtr hIcon);

        public static Int32 ShellExecute(this INativeExpander expander, IntPtr hWnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd) { return ShellExecute(hWnd, lpOperation, lpFile, lpParameters, lpDirectory, nShowCmd); }
        [DllImport("shell32.dll", EntryPoint = "ShellExecuteW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ShellExecute(IntPtr hWnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        public static Int32 GetSystemMetrics(this INativeExpander expander, Int32 nIndex) { return GetSystemMetrics(nIndex); }
        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 GetSystemMetrics(Int32 nIndex);

        public static IntPtr LoadLibrary(this INativeExpander expander, string lpLibFileName) { return LoadLibrary(lpLibFileName); }
        [DllImport("kernel32.dll", EntryPoint = "LoadLibraryW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr LoadLibrary(string lpLibFileName);

        public static IntPtr FreeLibrary(this INativeExpander expander, IntPtr hModule) { return FreeLibrary(hModule); }
        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr FreeLibrary(IntPtr hModule);

        public static IntPtr GetProcAddress(this INativeExpander expander, IntPtr hModule, string lpProcName) { return GetProcAddress(hModule, lpProcName); }
        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        public static Int32 SendMessage(this INativeExpander expander, IntPtr hWnd, uint msg, int wParam, int lParam) { return SendMessage(hWnd, msg, wParam, lParam); }
        [DllImport("user32.dll", EntryPoint = "SendMessageW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        public static Int32 PostMessage(this INativeExpander expander, IntPtr hWnd, uint msg, int wParam, int lParam) { return PostMessage(hWnd, msg, wParam, lParam); }
        [DllImport("user32.dll", EntryPoint = "PostMessageW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        public static IntPtr GetDC(this INativeExpander expander, IntPtr hWnd) { return GetDC(hWnd); }
        [DllImport("user32.dll", EntryPoint = "GetDC", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        public static Int32 FillRect(this INativeExpander expander, IntPtr hDc, RECT* pRect, Int32 nColor) { return FillRect(hDc, pRect, nColor); }
        [DllImport("user32.dll", EntryPoint = "FillRect", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 FillRect(IntPtr hDc, RECT* pRect, Int32 nColor);

        public static Int32 DrawText(this INativeExpander expander, IntPtr hDc, char* pString, Int32 nCount, RECT* pRect, uint uFormat) { return DrawText(hDc, pString, nCount, pRect, uFormat); }
        [DllImport("user32.dll", EntryPoint = "DrawTextW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 DrawText(IntPtr hDc, char* pString, Int32 nCount, RECT* pRect, uint uFormat);

        public static UInt32 ClientToScreen(this INativeExpander expander, IntPtr hWnd, POINT* lpPoint) { return ClientToScreen(hWnd, lpPoint); }
        [DllImport("user32.dll", EntryPoint = "ClientToScreen", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 ClientToScreen(IntPtr hWnd, POINT* lpPoint);

        public static  UInt32 ScreenToClient(this INativeExpander expander, IntPtr hWnd, POINT* lpPoint) { return ScreenToClient(hWnd, lpPoint); }
        [DllImport("user32.dll", EntryPoint = "ScreenToClient", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 ScreenToClient(IntPtr hWnd, POINT* lpPoint);

        public static IntPtr GetWindowLong(this INativeExpander expander, IntPtr hWnd, Int32 nIndex) { return GetWindowLong(hWnd, nIndex); }
        [DllImport("user32.dll", EntryPoint = "GetWindowLongW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, Int32 nIndex);

        public static IntPtr SetWindowLong(this INativeExpander expander, IntPtr hWnd, Int32 nIndex, IntPtr dwNewLong) { return SetWindowLong(hWnd, nIndex, dwNewLong); }
        [DllImport("user32.dll", EntryPoint = "SetWindowLongW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, Int32 nIndex, IntPtr dwNewLong);

        [DllImport("kernel32.dll", EntryPoint = "GetLastError", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetLastError();


        public static IntPtr CreateFile(this INativeExpander expander, string szFileName, uint dwDesiredAccess, uint dwShareMode, uint lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile) { return CreateFile(szFileName, dwDesiredAccess, dwShareMode, lpSecurityAttributes, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile); }
        [DllImport("kernel32.dll", EntryPoint = "CreateFileW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CreateFile(string szFileName, uint dwDesiredAccess, uint dwShareMode, uint lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        public static int GetFileSize(this INativeExpander expander, IntPtr hFile, int lpFileSizeHigh) { return GetFileSize(hFile, lpFileSizeHigh); }
        [DllImport("kernel32.dll", EntryPoint = "GetFileSize", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetFileSize(IntPtr hFile, int lpFileSizeHigh);

        public static bool WriteFile(this INativeExpander expander, IntPtr hFile, void* lpBuffer, int nNumberOfBytesToWrite, int* lpNumberOfBytesWritten, int lpOverlapped) { return WriteFile(hFile, lpBuffer, nNumberOfBytesToWrite, lpNumberOfBytesWritten, lpOverlapped); }
        [DllImport("kernel32.dll", EntryPoint = "WriteFile", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WriteFile(IntPtr hFile, void* lpBuffer, int nNumberOfBytesToWrite, int* lpNumberOfBytesWritten, int lpOverlapped);

        public static bool ReadFile(this INativeExpander expander, IntPtr hFile, void* pBuffer, int nNumberOfBytesToRead, int* lpNumberOfBytesRead, int lpOverlapped) { return ReadFile(hFile, pBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, lpOverlapped); }
        [DllImport("kernel32.dll", EntryPoint = "ReadFile", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ReadFile(IntPtr hFile, void* pBuffer, int nNumberOfBytesToRead, int* lpNumberOfBytesRead, int lpOverlapped);

        public static int LockFile(this INativeExpander expander, IntPtr hFile, int dwFileOffsetLow, int dwFileOffsetHigh, int nNumberOfBytesToLockLow, int nNumberOfBytesToLockHigh) { return LockFile(hFile, dwFileOffsetLow, dwFileOffsetHigh, nNumberOfBytesToLockLow, nNumberOfBytesToLockHigh); }
        [DllImport("kernel32.dll", EntryPoint = "LockFile", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int LockFile(IntPtr hFile, int dwFileOffsetLow, int dwFileOffsetHigh, int nNumberOfBytesToLockLow, int nNumberOfBytesToLockHigh);

        public static bool UnlockFile(this INativeExpander expander, IntPtr hFile, int dwFileOffsetLow, int dwFileOffsetHigh, int nNumberOfBytesToUnlockLow, int nNumberOfBytesToUnlockHigh) { return UnlockFile(hFile, dwFileOffsetLow, dwFileOffsetHigh, nNumberOfBytesToUnlockLow, nNumberOfBytesToUnlockHigh); }
        [DllImport("kernel32.dll", EntryPoint = "UnlockFile", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnlockFile(IntPtr hFile, int dwFileOffsetLow, int dwFileOffsetHigh, int nNumberOfBytesToUnlockLow, int nNumberOfBytesToUnlockHigh);

        public static uint SetFilePointer(this INativeExpander expander, IntPtr hFile, int lDistanceToMove, int lpDistanceToMoveHigh, uint dwMoveMethod) { return SetFilePointer(hFile, lDistanceToMove, lpDistanceToMoveHigh, dwMoveMethod); }
        [DllImport("kernel32.dll", EntryPoint = "SetFilePointer", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern uint SetFilePointer(IntPtr hFile, int lDistanceToMove, int lpDistanceToMoveHigh, uint dwMoveMethod);

        public static bool SetEndOfFile(this INativeExpander expander, IntPtr hFile) { return SetEndOfFile(hFile); }
        [DllImport("kernel32.dll", EntryPoint = "SetEndOfFile", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetEndOfFile(IntPtr hFile);

        public static int CloseHandle(this INativeExpander expander, IntPtr hObject) { return CloseHandle(hObject); }
        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int CloseHandle(IntPtr hObject);

        public static void NetApiBufferFree(this INativeExpander expander, IntPtr pointerBuffer) { NetApiBufferFree(pointerBuffer); }
        [DllImport("netapi32.dll", EntryPoint = "NetApiBufferFree")]
        public static extern void NetApiBufferFree(IntPtr pointerBuffer);

        public static uint NetServerEnum(this INativeExpander expander, IntPtr serverName, uint level, ref IntPtr pointerBuffer, uint prefMaxLen, ref uint entriesRead, ref uint totalEntries, uint serverType, [MarshalAs(UnmanagedType.LPWStr)] string domain, IntPtr resumeHandle) { return NetServerEnum(serverName, level, ref pointerBuffer, prefMaxLen, ref entriesRead, ref totalEntries, serverType, domain, resumeHandle); }
        [DllImport("netapi32.dll", EntryPoint = "NetServerEnum")]
        public static extern uint NetServerEnum(IntPtr serverName, uint level, ref IntPtr pointerBuffer, uint prefMaxLen, ref uint entriesRead, ref uint totalEntries, uint serverType, [MarshalAs(UnmanagedType.LPWStr)] string domain, IntPtr resumeHandle);

        public static uint NetServerGetInfo(this INativeExpander expander, [MarshalAs(UnmanagedType.LPWStr)] string serverName, int level, ref IntPtr pointerBuffer) { return NetServerGetInfo(serverName, level, ref pointerBuffer); }
        [DllImport("netapi32.dll", EntryPoint = "NetServerGetInfo")]
        public static extern uint NetServerGetInfo([MarshalAs(UnmanagedType.LPWStr)] string serverName, int level, ref IntPtr pointerBuffer);

        public static IntPtr OpenInputDesktop(this INativeExpander expander, int dwFlags, [MarshalAs(UnmanagedType.Bool)] bool fInherit, int dwDesiredAccess) { return OpenInputDesktop(dwFlags, fInherit, dwDesiredAccess); }
        [DllImport("user32.dll")]
        public static extern IntPtr OpenInputDesktop(int dwFlags, [MarshalAs(UnmanagedType.Bool)] bool fInherit, int dwDesiredAccess);

        public static bool CloseDesktop(this INativeExpander expander, IntPtr hDesktop) { return CloseDesktop(hDesktop); }
        [DllImport("user32.dll")]
        public static extern bool CloseDesktop(IntPtr hDesktop);

        public static IntPtr SetWindowsHookEx(this INativeExpander expander, int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId) { return SetWindowsHookEx(idHook, lpfn, hMod, dwThreadId); }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        public static bool UnhookWindowsHookEx(this INativeExpander expander, IntPtr hhk) { return UnhookWindowsHookEx(hhk); }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        public static IntPtr CallNextHookEx(this INativeExpander expander, IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam) { return CallNextHookEx(hhk, nCode, wParam, lParam); }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        public static IntPtr GetModuleHandle(this INativeExpander expander, string lpModuleName) { return GetModuleHandle(lpModuleName); }
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        public static IntPtr SHGetFileInfo(this INativeExpander expander, string pszPath, int dwFileAttributes, ref SHFILEINFO shinfo, uint cbfileInfo, int uFlags) { return SHGetFileInfo(pszPath, dwFileAttributes, ref shinfo, cbfileInfo, uFlags); }
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO shinfo, uint cbfileInfo, int uFlags);

        public static int GetWindowThreadProcessId(this INativeExpander expander, [In] IntPtr hWnd, [Out, Optional] IntPtr lpdwProcessId) { return GetWindowThreadProcessId(hWnd, lpdwProcessId); }
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowThreadProcessId([In] IntPtr hWnd, [Out, Optional] IntPtr lpdwProcessId);

        public static IntPtr GetForegroundWindow(this INativeExpander expander) { return GetForegroundWindow(); }
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        public static ushort GetKeyboardLayout(this INativeExpander expander, int idThread) { return GetKeyboardLayout(idThread); }
        [DllImport("user32.dll", SetLastError = true)]
        public static extern ushort GetKeyboardLayout([In] int idThread);


        public static bool CryptAcquireContext(this INativeExpander expander, ref IntPtr hProv, string pszContainer, string pszProvider, uint dwProvType, uint dwFlags) { return CryptAcquireContext(ref hProv, pszContainer, pszProvider, dwProvType, dwFlags); }
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CryptAcquireContext(ref IntPtr hProv, string pszContainer, string pszProvider, uint dwProvType, uint dwFlags);

        public static bool CryptGetProvParam(this INativeExpander expander, IntPtr hProv, uint dwParam, [In, Out] byte[] pbData, ref uint dwDataLen, uint dwFlags) { return CryptGetProvParam(hProv, dwParam, pbData, ref dwDataLen, dwFlags); }
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool CryptGetProvParam(IntPtr hProv, uint dwParam, [In, Out] byte[] pbData, ref uint dwDataLen, uint dwFlags);

        public static bool CryptGetProvParam(this INativeExpander expander, IntPtr hProv, uint dwParam, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pbData, ref uint dwDataLen, uint dwFlags) { return CryptGetProvParam(hProv, dwParam, pbData, ref dwDataLen, dwFlags); }
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool CryptGetProvParam(IntPtr hProv, uint dwParam, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pbData, ref uint dwDataLen, uint dwFlags);

        public static bool CryptExportPublicKeyInfo(this INativeExpander expander, IntPtr hProv, uint dwKeySpec, uint dwCertEncodingType, IntPtr pInfo, ref uint pcbInfo) { return CryptExportPublicKeyInfo(hProv, dwKeySpec, dwCertEncodingType, pInfo, ref pcbInfo); }
        [DllImport("Crypt32.dll", EntryPoint = "CryptExportPublicKeyInfo", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CryptExportPublicKeyInfo(IntPtr hProv, uint dwKeySpec, uint dwCertEncodingType, IntPtr pInfo, ref uint pcbInfo);

        public static bool CryptReleaseContext(this INativeExpander expander, IntPtr hProv, Int32 dwFlags) { return CryptReleaseContext(hProv, dwFlags); }
        [DllImport("Advapi32.dll", EntryPoint = "CryptReleaseContext", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CryptReleaseContext(IntPtr hProv, Int32 dwFlags);

        public static int MakeLong(this INativeExpander expander, int loWord, int hiWord)
        {
            return (hiWord << 16) | (loWord & 0xffff);
        }

        public static IntPtr MakeLParam(this INativeExpander expander, int loWord, int hiWord)
        {
            return (IntPtr)((hiWord << 16) | (loWord & 0xffff));
        }

        public static int HiWord(this INativeExpander expander, int nNumber)
        {
            return (nNumber >> 16) & 0xffff;
        }

        public static int LoWord(this INativeExpander expander, int nNumber)
        {
            return nNumber & 0xffff;
        }

        public static int ShellAbout(this INativeExpander expander, IntPtr ownerHandle, string title, string appName, string companyName)
        {
            fixed (char* szApp = title + "#" + appName)
            {
                fixed (char* szOtherStuff = companyName)
                {
                    return ShellAbout(ownerHandle, szApp, szOtherStuff, IntPtr.Zero);
                }
            }
        }

        public static void ShowCalendar(this INativeExpander expander, IntPtr owner)
        {
            ShellExecute(owner, "open", "Rundll32.exe", "shell32.dll,Control_RunDLL TIMEDATE.CPL", "C:\\WINDOWS\\SYSTEM32\\", WindowShows.SW_SHOWNORMAL);
        }

        public static IntPtr CreateFile(this INativeExpander expander, string fileName)
        {
            return CreateFile(fileName,
                (uint)(FileCodes.FILE_GENERIC_READ| FileCodes.FILE_GENERIC_WRITE),
                (uint)(FileCodes.FILE_SHARE_READ | FileCodes.FILE_SHARE_WRITE),
                0,
                (uint)FileCodes.OPEN_ALWAYS,
                (uint)FileCodes.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
        }

        public static int GetFileSize(this INativeExpander expander, IntPtr hFile)
        {
            return GetFileSize(hFile, 0);
        }

        public static bool LockFile(this INativeExpander expander, IntPtr hFile, int offset, int nNumberOfBytesToLock)
        {
            return LockFile(hFile, offset, 0, LoWord(expander, nNumberOfBytesToLock), HiWord(expander, nNumberOfBytesToLock)) != 0;
        }

        public static bool UnlockFile(this INativeExpander expander, IntPtr hFile, int offset, int nNumberOfBytesToUnlock)
        {
            return UnlockFile(hFile, offset, 0, LoWord(expander, nNumberOfBytesToUnlock), HiWord(expander, nNumberOfBytesToUnlock));
        }

        public static bool SetFilePointer(this INativeExpander expander, IntPtr hFile, int offset, SeekOrigin seekOrigin)
        {
            uint result = SetFilePointer(hFile, offset, 0, (uint)(seekOrigin == SeekOrigin.Begin ? FileCodes.FILE_BEGIN : seekOrigin == SeekOrigin.End ? FileCodes.FILE_END : FileCodes.FILE_CURRENT));
            return result == (int)FileCodes.INVALID_SET_FILE_POINTER;
        }

        public static bool WriteFile(this INativeExpander expander, IntPtr hFile, object data)
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

        public static object ReadFile(this INativeExpander expander, IntPtr hFile, Type type)
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

        public static int GetMdiCreateStruct(this INativeExpander expander, string className, string title, IntPtr owner, int x, int y, int cx, int cy, uint style)
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

        public static sbyte WindowVisible(this INativeExpander expander, IntPtr lParam)
        {
            var p = (WindowPosStruct*)lParam.ToPointer();
            WindowPosStruct wp = *p;
            var flags = wp.flags;
            if ((flags & WindowPositions.SWP_HIDEWINDOW) == WindowPositions.SWP_HIDEWINDOW) return -1;
            if ((flags & WindowPositions.SWP_SHOWWINDOW) == WindowPositions.SWP_SHOWWINDOW) return 1;
            return 0;
        }

        public static POINT ClientToScreen(this INativeExpander expander, IntPtr hWnd, POINT pOld)
        {
            POINT* lpPointer = &pOld;
            UInt32 retcode = ClientToScreen(hWnd, lpPointer);
            if (retcode == 0) return pOld;
            POINT pNew = *lpPointer;
            return pNew;
        }

        public static POINT ScreenToClient(this INativeExpander expander, IntPtr hWnd, POINT pOld)
        {
            POINT* lpPointer = &pOld;
            UInt32 retcode = ScreenToClient(hWnd, lpPointer);
            if (retcode == 0) return pOld;
            POINT pNew = *lpPointer;
            return pNew;
        }

        public static int DrawText(this INativeExpander expander, IntPtr hDc, object value, RECT rect, uint format)
        {
            var text = value.AsString();
            fixed (char* pString = text)
            {
                return DrawText(hDc, pString, text.Length, &rect, format);
            }
        }

        public static string[] GetSqlServers(this INativeExpander expander)
        {
            var sqlServers = new LanCollection(ServerTypes.SV_TYPE_SQLSERVER);
            var list = new List<string>();
            foreach (var sqlServer in sqlServers) list.Add(sqlServer.AsString());
            return list.ToArray();
        }

        public static SHFILEINFO ShellInfo(this INativeExpander expander, string fileName)
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

        public static MdiClient MdiClientDisableScrolling(this INativeExpander expander, Form form)
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

        public static string GetKeyboardLayout(this INativeExpander expander)
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

        public static X509Certificate2 FindCertificate()
        {
            var containers = GetContainers();
            foreach (var container in containers)
            {
                var containerKey = GetPublicKeyOfContainer(container);
                var certificate = FindCertificate(containerKey);
                if (certificate != null) return certificate;
            }
            return null;
        }

        private static X509Certificate2 FindCertificate(byte[] containerKey)
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
            try
            {
                foreach (X509Certificate2 certificate in store.Certificates)
                {
                    var certificateKey = certificate.PublicKey.EncodedKeyValue.RawData;
                    if (IsEqualBytes(containerKey, certificateKey)) return certificate;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                store.Close();
            }
        }

        private static string[] GetContainers()
        {
            var containers = new List<string>();
            uint pdwDataLen = 0;
            var hProv = IntPtr.Zero;
            var dwFlags = CryptCodes.CRYPT_FIRST;
            StringBuilder sb = null;
            try
            {
                CryptAcquireContext(ref hProv, null, null, CryptCodes.CRYPT_PROV_TYPE, CryptCodes.CRYPT_VERIFYCONTEXT);
                CryptGetProvParam(hProv, CryptCodes.PP_ENUMCONTAINERS, sb, ref pdwDataLen, dwFlags);
                var buffsize = (int)(2 * pdwDataLen);
                sb = new StringBuilder(buffsize);
                while (CryptGetProvParam(hProv, CryptCodes.PP_ENUMCONTAINERS, sb, ref pdwDataLen, dwFlags | CryptCodes.CRYPT_FQCN))
                {
                    dwFlags = 0; //required to continue entire enumeration
                    containers.Add(sb.ToString());
                }
            }
            finally
            {
                if (hProv != IntPtr.Zero) CryptReleaseContext(hProv, 0);
            }
            return containers.ToArray();
        }

        private static byte[] GetPublicKeyOfContainer(string container)
        {
            var info = new CERT_PUBLIC_KEY_INFO();
            var pInfo = IntPtr.Zero;
            var hProv = IntPtr.Zero;
            uint pcbInfo = 0;
            try
            {
                var i = 0;
                CryptAcquireContext(ref hProv, container, null, CryptCodes.CRYPT_PROV_TYPE, 0);
                CryptExportPublicKeyInfo(hProv, CryptCodes.AT_KEYEXCHANGE, CryptCodes.X509_ASN_ENCODING | CryptCodes.PKCS_7_ASN_ENCODING, IntPtr.Zero, ref pcbInfo);
                pInfo = Marshal.AllocHGlobal((int)pcbInfo);
                Marshal.StructureToPtr(info, pInfo, false);
                CryptExportPublicKeyInfo(hProv, CryptCodes.AT_KEYEXCHANGE, CryptCodes.X509_ASN_ENCODING | CryptCodes.PKCS_7_ASN_ENCODING, pInfo, ref pcbInfo);
                info = (CERT_PUBLIC_KEY_INFO)Marshal.PtrToStructure(pInfo, typeof(CERT_PUBLIC_KEY_INFO));
                var bytes = new byte[66];
                Marshal.Copy(info.PublicKey.pbData, bytes, 0, bytes.Length);
                return bytes;
            }
            finally
            {
                if (hProv != IntPtr.Zero) CryptReleaseContext(hProv, 0);
                Marshal.FreeHGlobal(pInfo);
            }
        }

        private static bool IsEqualBytes(byte[] bytes1, byte[] bytes2)
        {
            if (bytes1 == null || bytes2 == null) return false;
            if (bytes1.Length != bytes2.Length) return false;
            for (int i = 0; i < bytes1.Length; i++)
            {
                if (bytes1[i] != bytes2[i]) return false;
            }
            return true;
        }
    }
}