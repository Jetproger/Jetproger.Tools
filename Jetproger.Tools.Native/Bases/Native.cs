using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Native
{
    public unsafe static class NativeMethods
    {
        public static IntPtr LoadIcon(this WinExpander expander, IntPtr hInstance, string lpIconName) { return LoadIcon(hInstance, lpIconName); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr LoadIcon(IntPtr hInstance, string lpIconName);

        public static IntPtr LoadIcon(this WinExpander expander, IntPtr hInstance, IntPtr lpIconResource) { return LoadIcon(hInstance, lpIconResource); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconResource);

        public static IntPtr LoadCursor(this WinExpander expander, IntPtr hInstance, string lpCursorName) { return LoadCursor(hInstance, lpCursorName); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr LoadCursor(IntPtr hInstance, string lpCursorName);

        public static IntPtr LoadCursor(this WinExpander expander, IntPtr hInstance, IntPtr lpCursorResource) { return LoadCursor(hInstance, lpCursorResource); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr LoadCursor(IntPtr hInstance, IntPtr lpCursorResource);

        public static IntPtr LoadImage(this WinExpander expander, IntPtr hInstance, string lpszName, int uType, int cxDesired, int cyDesired, int fuLoad) { return LoadImage(hInstance, lpszName, uType, cxDesired, cyDesired, fuLoad); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr LoadImage(IntPtr hInstance, string lpszName, int uType, int cxDesired, int cyDesired, int fuLoad);

        public static IntPtr LoadImage(this WinExpander expander, IntPtr hInstance, IntPtr resourceId, int uType, int cxDesired, int cyDesired, int fuLoad) { return LoadImage(hInstance, resourceId, uType, cxDesired, cyDesired, fuLoad); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr LoadImage(IntPtr hInstance, IntPtr resourceId, int uType, int cxDesired, int cyDesired, int fuLoad);

        public static IntPtr LoadBitmap(this WinExpander expander, IntPtr hInstance, string lpBitmapName) { return LoadBitmap(hInstance, lpBitmapName); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr LoadBitmap(IntPtr hInstance, string lpBitmapName);

        public static IntPtr LoadBitmap(this WinExpander expander, IntPtr hInstance, IntPtr resourceId) { return LoadBitmap(hInstance, resourceId); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr LoadBitmap(IntPtr hInstance, IntPtr resourceId);

        public static IntPtr BeginPaint(this WinExpander expander, IntPtr hwnd, out PaintStruct lpPaint) { return BeginPaint(hwnd, out lpPaint); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr BeginPaint(IntPtr hwnd, out PaintStruct lpPaint);

        public static void EndPaint(this WinExpander expander, IntPtr hwnd, [In] ref PaintStruct lpPaint) { EndPaint(hwnd, ref lpPaint); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern void EndPaint(IntPtr hwnd, [In] ref PaintStruct lpPaint);

        public static IntPtr GetDesktopWindow(this WinExpander expander) { return GetDesktopWindow(); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr GetDesktopWindow();

        public static IntPtr MonitorFromPoint(this WinExpander expander, Point pt, int dwFlags) { return MonitorFromPoint(pt, dwFlags); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr MonitorFromPoint(Point pt, int dwFlags);

        public static IntPtr MonitorFromWindow(this WinExpander expander, IntPtr hwnd, int dwFlags) { return MonitorFromWindow(hwnd, dwFlags); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

        public static int DrawText(this WinExpander expander, IntPtr hdc, string lpString, int nCount, [In] ref Rectangle lpRect, uint uFormat) { return DrawText(hdc, lpString, nCount, ref lpRect, uFormat); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int DrawText(IntPtr hdc, string lpString, int nCount, [In] ref Rectangle lpRect, uint uFormat);

        public static bool RegisterHotKey(this WinExpander expander, IntPtr hWnd, int id, int fsModifiers, int vk) { return RegisterHotKey(hWnd, id, fsModifiers, vk); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        public static bool UnregisterHotKey(this WinExpander expander, IntPtr hWnd, int id) { return UnregisterHotKey(hWnd, id); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static uint SendInput(this WinExpander expander, uint nInputs, IntPtr pInputs, int cbSize) { return SendInput(nInputs, pInputs, cbSize); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern uint SendInput(uint nInputs, IntPtr pInputs, int cbSize);

        public static uint SendInput(this WinExpander expander, uint nInputs, [In] Input[] pInputs, int cbSize) { return SendInput(nInputs, pInputs, cbSize); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern uint SendInput(uint nInputs, [In] Input[] pInputs, int cbSize);

        public static IntPtr SetTimer(this WinExpander expander, IntPtr hWnd, IntPtr nIdEvent, uint uElapseMillis, TimerProc lpTimerFunc) { return SetTimer(hWnd, nIdEvent, uElapseMillis, lpTimerFunc); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr SetTimer(IntPtr hWnd, IntPtr nIdEvent, uint uElapseMillis, TimerProc lpTimerFunc);

        public static bool KillTimer(this WinExpander expander, IntPtr hwnd, IntPtr uIdEvent) { return KillTimer(hwnd, uIdEvent); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool KillTimer(IntPtr hwnd, IntPtr uIdEvent);

        public static bool ValidateRect(this WinExpander expander, IntPtr hWnd, [In] ref Rectangle lpRect) { return ValidateRect(hWnd, ref lpRect); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool ValidateRect(IntPtr hWnd, [In] ref Rectangle lpRect);

        public static bool ValidateRect(this WinExpander expander, IntPtr hWnd, IntPtr lpRect) { return ValidateRect(hWnd, lpRect); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool ValidateRect(IntPtr hWnd, IntPtr lpRect);

        public static bool InvalidateRect(this WinExpander expander, IntPtr hWnd, [In] ref Rectangle lpRect, bool bErase) { return InvalidateRect(hWnd, ref lpRect, bErase); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool InvalidateRect(IntPtr hWnd, [In] ref Rectangle lpRect, bool bErase);

        public static bool InvalidateRect(this WinExpander expander, IntPtr hWnd, IntPtr lpRect, bool bErase) { return InvalidateRect(hWnd, lpRect, bErase); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        public static bool SystemParametersInfo(this WinExpander expander, uint uiAction, uint uiParam, IntPtr pvParam, int fWinIni) { return SystemParametersInfo(uiAction, uiParam, pvParam, fWinIni); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, int fWinIni);

        public static IntPtr GetWindowDC(this WinExpander expander, IntPtr hwnd) { return GetWindowDC(hwnd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        public static IntPtr WindowFromDC(this WinExpander expander, IntPtr hdc) { return WindowFromDC(hdc); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr WindowFromDC(IntPtr hdc);

        public static bool ReleaseDC(this WinExpander expander, IntPtr hwnd, IntPtr hdc) { return ReleaseDC(hwnd, hdc); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool ReleaseDC(IntPtr hwnd, IntPtr hdc);

        public static bool InvertRect(this WinExpander expander, IntPtr hdc, [In] ref Rectangle lprc) { return InvertRect(hdc, ref lprc); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool InvertRect(IntPtr hdc, [In] ref Rectangle lprc);

        public static bool SetRectEmpty(this WinExpander expander, out Rectangle lprc) { return SetRectEmpty(out lprc); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool SetRectEmpty(out Rectangle lprc);

        public static bool AdjustWindowRect(this WinExpander expander, [In] [Out] ref Rectangle lpRect, int dwStyle, bool hasMenu) { return AdjustWindowRect(ref lpRect, dwStyle, hasMenu); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool AdjustWindowRect([In] [Out] ref Rectangle lpRect, int dwStyle, bool hasMenu);

        public static bool AdjustWindowRectEx(this WinExpander expander, [In] [Out] ref Rectangle lpRect, int dwStyle, bool hasMenu, int dwExStyle) { return AdjustWindowRectEx(ref lpRect, dwStyle, hasMenu, dwExStyle); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool AdjustWindowRectEx([In] [Out] ref Rectangle lpRect, int dwStyle, bool hasMenu, int dwExStyle);

        public static bool CopyRect(this WinExpander expander, out Rectangle lprcDst, [In] ref Rectangle lprcSrc) { return CopyRect(out lprcDst, ref lprcSrc); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool CopyRect(out Rectangle lprcDst, [In] ref Rectangle lprcSrc);

        public static bool IntersectRect(this WinExpander expander, out Rectangle lprcDst, [In] ref Rectangle lprcSrc1, [In] ref Rectangle lprcSrc2) { return IntersectRect(out lprcDst, ref lprcSrc1, ref lprcSrc2); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool IntersectRect(out Rectangle lprcDst, [In] ref Rectangle lprcSrc1, [In] ref Rectangle lprcSrc2);

        public static bool UnionRect(this WinExpander expander, out Rectangle lprcDst, [In] ref Rectangle lprcSrc1, [In] ref Rectangle lprcSrc2) { return UnionRect(out lprcDst, ref lprcSrc1, ref lprcSrc2); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool UnionRect(out Rectangle lprcDst, [In] ref Rectangle lprcSrc1, [In] ref Rectangle lprcSrc2);

        public static bool IsRectEmpty(this WinExpander expander, [In] ref Rectangle lprc) { return IsRectEmpty(ref lprc); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool IsRectEmpty([In] ref Rectangle lprc);

        public static bool PtInRect(this WinExpander expander, [In] ref Rectangle lprc, Point pt) { return PtInRect(ref lprc, pt); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool PtInRect([In] ref Rectangle lprc, Point pt);

        public static bool OffsetRect(this WinExpander expander, [In] [Out] ref Rectangle lprc, int dx, int dy) { return OffsetRect(ref lprc, dx, dy); }
        [DllImport("user32", ExactSpelling = true)]
        public static extern bool OffsetRect([In] [Out] ref Rectangle lprc, int dx, int dy);

        public static bool InflateRect(this WinExpander expander, [In] [Out] ref Rectangle lprc, int dx, int dy) { return InflateRect(ref lprc, dx, dy); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool InflateRect([In] [Out] ref Rectangle lprc, int dx, int dy);

        public static bool FrameRect(this WinExpander expander, IntPtr hdc, [In] ref Rectangle lprc, IntPtr hbr) { return FrameRect(hdc, ref lprc, hbr); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool FrameRect(IntPtr hdc, [In] ref Rectangle lprc, IntPtr hbr);

        public static bool FillRect(this WinExpander expander, IntPtr hdc, [In] ref Rectangle lprc, IntPtr hbr) { return FillRect(hdc, ref lprc, hbr); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool FillRect(IntPtr hdc, [In] ref Rectangle lprc, IntPtr hbr);

        public static int GetWindowRgn(this WinExpander expander, IntPtr hWnd, IntPtr hRgn) { return GetWindowRgn(hWnd, hRgn); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);

        public static int GetWindowRgnBox(this WinExpander expander, IntPtr hWnd, out Rectangle lprc) { return GetWindowRgnBox(hWnd, out lprc); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern int GetWindowRgnBox(IntPtr hWnd, out Rectangle lprc);

        public static bool SetLayeredWindowAttributes(this WinExpander expander, IntPtr hwnd, uint crKey, byte bAlpha, int dwFlags) { return SetLayeredWindowAttributes(hwnd, crKey, bAlpha, dwFlags); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, int dwFlags);

        public static bool WinHelp(this WinExpander expander, IntPtr hWndMain, string lpszHelp, uint uCommand, uint dwData) { return WinHelp(hWndMain, lpszHelp, uCommand, dwData); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool WinHelp(IntPtr hWndMain, string lpszHelp, uint uCommand, uint dwData);

        public static bool SetWindowRgn(this WinExpander expander, IntPtr hWnd, IntPtr hRgn, bool bRedraw) { return SetWindowRgn(hWnd, hRgn, bRedraw); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        public static bool InvalidateRgn(this WinExpander expander, IntPtr hWnd, IntPtr hRgn, bool bErase) { return InvalidateRgn(hWnd, hRgn, bErase); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool InvalidateRgn(IntPtr hWnd, IntPtr hRgn, bool bErase);

        public static bool GetUpdateRect(this WinExpander expander, IntPtr hwnd, out Rectangle rect, bool bErase) { return GetUpdateRect(hwnd, out rect, bErase); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool GetUpdateRect(IntPtr hwnd, out Rectangle rect, bool bErase);

        public static bool ValidateRgn(this WinExpander expander, IntPtr hWnd, IntPtr hRgn) { return ValidateRgn(hWnd, hRgn); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool ValidateRgn(IntPtr hWnd, IntPtr hRgn);

        public static IntPtr GetDCEx(this WinExpander expander, IntPtr hWnd, IntPtr hrgnClip, int flags) { return GetDCEx(hWnd, hrgnClip, flags); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, int flags);

        public static void DisableProcessWindowsGhosting(this WinExpander expander) { DisableProcessWindowsGhosting(); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern void DisableProcessWindowsGhosting();

        public static bool UpdateLayeredWindow(this WinExpander expander, IntPtr hwnd, IntPtr hdcDst, [In] ref Point pptDst, [In] ref Size psize, IntPtr hdcSrc, [In] ref Point pptSrc, uint crKey, [In] ref BlendFunction pblend, uint dwFlags) { return UpdateLayeredWindow(hwnd, hdcDst, ref pptDst, ref psize, hdcSrc, ref pptSrc, crKey, ref pblend, dwFlags); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, [In] ref Point pptDst, [In] ref Size psize, IntPtr hdcSrc, [In] ref Point pptSrc, uint crKey, [In] ref BlendFunction pblend, uint dwFlags);

        public static int MapWindowPoints(this WinExpander expander, IntPtr hWndFrom, IntPtr hWndTo, IntPtr lpPoints, int cPoints) { return MapWindowPoints(hWndFrom, hWndTo, lpPoints, cPoints); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, IntPtr lpPoints, int cPoints);

        public static bool ScreenToClient(this WinExpander expander, IntPtr hWnd, [In] [Out] ref Point lpPoint) { return ScreenToClient(hWnd, ref  lpPoint); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool ScreenToClient(IntPtr hWnd, [In] [Out] ref Point lpPoint);

        public static IntPtr WindowFromPhysicalPoint(this WinExpander expander, Point point) { return WindowFromPhysicalPoint(point); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr WindowFromPhysicalPoint(Point point);

        public static IntPtr WindowFromPoint(this WinExpander expander, Point point) { return WindowFromPoint(point); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr WindowFromPoint(Point point);

        public static bool OpenIcon(this WinExpander expander, IntPtr hwnd) { return OpenIcon(hwnd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool OpenIcon(IntPtr hwnd);

        public static bool IsHungAppWindow(this WinExpander expander, IntPtr hwnd) { return IsHungAppWindow(hwnd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool IsHungAppWindow(IntPtr hwnd);

        public static bool IsZoomed(this WinExpander expander, IntPtr hwnd) { return IsZoomed(hwnd); ; }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool IsZoomed(IntPtr hwnd);

        public static bool IsIconic(this WinExpander expander, IntPtr hwnd) { return IsIconic(hwnd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool IsIconic(IntPtr hwnd);

        public static bool LogicalToPhysicalPoint(this WinExpander expander, IntPtr hwnd, [In] [Out] ref Point point) { return LogicalToPhysicalPoint(hwnd, ref point); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool LogicalToPhysicalPoint(IntPtr hwnd, [In] [Out] ref Point point);

        public static IntPtr ChildWindowFromPoint(this WinExpander expander, IntPtr hwndParent, Point point) { return ChildWindowFromPoint(hwndParent, point); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr ChildWindowFromPoint(IntPtr hwndParent, Point point);

        public static IntPtr ChildWindowFromPointEx(this WinExpander expander, IntPtr hwndParent, Point point, int flags) { return ChildWindowFromPointEx(hwndParent, point, flags); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr ChildWindowFromPointEx(IntPtr hwndParent, Point point, int flags);

        public static IntPtr GetMenu(this WinExpander expander, IntPtr hwnd) { return GetMenu(hwnd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr GetMenu(IntPtr hwnd);

        public static int MessageBox(this WinExpander expander, IntPtr hWnd, string lpText, string lpCaption, uint type) { return MessageBox(hWnd, lpText, lpCaption, type); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint type);

        public static int MessageBoxEx(this WinExpander expander, IntPtr hWnd, string lpText, string lpCaption, uint type, ushort wLanguageId) { return MessageBoxEx(hWnd, lpText, lpCaption, type, wLanguageId); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int MessageBoxEx(IntPtr hWnd, string lpText, string lpCaption, uint type, ushort wLanguageId);

        public static IntPtr GetSystemMenu(this WinExpander expander, IntPtr hWnd, bool bRevert) { return GetSystemMenu(hWnd, bRevert); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        public static bool SetThreadDesktop(this WinExpander expander, IntPtr hDesk) { return SetThreadDesktop(hDesk); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool SetThreadDesktop(IntPtr hDesk);

        public static IntPtr GetThreadDesktop(this WinExpander expander, uint threadId) { return GetThreadDesktop(threadId); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr GetThreadDesktop(uint threadId);

        public static bool GetMonitorInfo(this WinExpander expander, IntPtr hMonitor, ref MonitorInfo lpmi) { return GetMonitorInfo(hMonitor, ref lpmi); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo lpmi);

        public static bool IsWindowEnabled(this WinExpander expander, IntPtr hWnd) { return IsWindowEnabled(hWnd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool IsWindowEnabled(IntPtr hWnd);

        public static IntPtr SetCapture(this WinExpander expander, IntPtr hWnd) { return SetCapture(hWnd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr SetCapture(IntPtr hWnd);

        public static IntPtr GetFocus(this WinExpander expander) { return GetFocus(); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr GetFocus();

        public static IntPtr SetFocus(this WinExpander expander, IntPtr hWnd) { return SetFocus(hWnd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr SetFocus(IntPtr hWnd);

        public static IntPtr SetActiveWindow(this WinExpander expander, IntPtr hWnd) { return SetActiveWindow(hWnd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr SetActiveWindow(IntPtr hWnd);

        public static IntPtr GetActiveWindow(this WinExpander expander) { return GetActiveWindow(); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr GetActiveWindow();

        public static bool BlockInput(this WinExpander expander, bool fBlockIt) { return BlockInput(fBlockIt); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool BlockInput(bool fBlockIt);

        public static uint WaitForInputIdle(this WinExpander expander, IntPtr hProcess, uint dwMilliseconds) { return WaitForInputIdle(hProcess,  dwMilliseconds); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern uint WaitForInputIdle(IntPtr hProcess, uint dwMilliseconds);

        public static bool AttachThreadInput(this WinExpander expander, uint idAttach, uint idAttachTo, bool fAttach) { return AttachThreadInput(idAttach, idAttachTo, fAttach); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        public static bool DragDetect(this WinExpander expander, IntPtr hwnd, Point point) { return DragDetect(hwnd, point); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool DragDetect(IntPtr hwnd, Point point);

        public static bool ClientToScreen(this WinExpander expander, IntPtr hwnd, [In] [Out] ref Point point) { return ClientToScreen(hwnd, ref point); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool ClientToScreen(IntPtr hwnd, [In] [Out] ref Point point);

        public static bool ClipCursor(this WinExpander expander, [In] ref Rectangle rect) { return ClipCursor(ref rect); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool ClipCursor([In] ref Rectangle rect);

        public static bool ClipCursor(this WinExpander expander, IntPtr ptr) { return ClipCursor(ptr); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool ClipCursor(IntPtr ptr);

        public static bool TrackMouseEvent(this WinExpander expander, [In] [Out] ref TrackMouseEventOptions lpEventTrack) { return TrackMouseEvent(ref lpEventTrack); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool TrackMouseEvent([In] [Out] ref TrackMouseEventOptions lpEventTrack);

        public static bool GetLastInputInfo(this WinExpander expander, out LastInputInfo plii) { return GetLastInputInfo(out plii); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool GetLastInputInfo(out LastInputInfo plii);

        public static uint MapVirtualKey(this WinExpander expander, uint uCode, int uMapType) { return MapVirtualKey(uCode, uMapType); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern uint MapVirtualKey(uint uCode, int uMapType);

        public static uint MapVirtualKey(this WinExpander expander, int uCode, int uMapType) { return MapVirtualKey(uCode, uMapType); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern uint MapVirtualKey(int uCode, int uMapType);

        public static uint MapVirtualKeyEx(this WinExpander expander, uint uCode, int uMapType, IntPtr dwhkl) { return MapVirtualKeyEx(uCode, uMapType, dwhkl); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern uint MapVirtualKeyEx(uint uCode, int uMapType, IntPtr dwhkl);

        public static uint MapVirtualKeyEx(this WinExpander expander, int uCode, int uMapType, IntPtr dwhkl) { return MapVirtualKeyEx(uCode, uMapType, dwhkl); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern uint MapVirtualKeyEx(int uCode, int uMapType, IntPtr dwhkl);

        public static KeyState GetKeyState(this WinExpander expander, int vKey) { return GetKeyState(vKey); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern KeyState GetKeyState(int vKey);

        public static bool GetKeyboardState(this WinExpander expander, [MarshalAs(UnmanagedType.LPArray, SizeConst = 256)] out byte[] lpKeyState) { return GetKeyboardState(out lpKeyState); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool GetKeyboardState([MarshalAs(UnmanagedType.LPArray, SizeConst = 256)] out byte[] lpKeyState);

        public static bool GetKeyboardState(this WinExpander expander, IntPtr lpKeyState) { return GetKeyboardState(lpKeyState); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool GetKeyboardState(IntPtr lpKeyState);

        public static bool SetKeyboardState(this WinExpander expander, [MarshalAs(UnmanagedType.LPArray, SizeConst = 256)] [In] byte[] lpKeyState) { return SetKeyboardState(lpKeyState); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool SetKeyboardState([MarshalAs(UnmanagedType.LPArray, SizeConst = 256)] [In] byte[] lpKeyState);

        public static bool SetKeyboardState(this WinExpander expander, IntPtr lpKeyState) { return SetKeyboardState(lpKeyState); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool SetKeyboardState(IntPtr lpKeyState);

        public static bool GetTitleBarInfo(this WinExpander expander, IntPtr hwnd, IntPtr pti) { return GetTitleBarInfo(hwnd, pti); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool GetTitleBarInfo(IntPtr hwnd, IntPtr pti);

        public static bool GetCursorPos(this WinExpander expander, out Point point) { return GetCursorPos(out point); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool GetCursorPos(out Point point);

        public static bool SetCursorPos(this WinExpander expander, int x, int y) { return SetCursorPos(x, y); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool SetCursorPos(int x, int y);

        public static bool SetPhysicalCursorPos(this WinExpander expander, int x, int y) { return SetPhysicalCursorPos(x, y); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool SetPhysicalCursorPos(int x, int y);

        public static bool SetSystemCursor(this WinExpander expander, IntPtr cursor, int id) { return SetSystemCursor(cursor, id); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool SetSystemCursor(IntPtr cursor, int id);

        public static int ShowCursor(this WinExpander expander, bool bShow) { return ShowCursor(bShow); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern int ShowCursor(bool bShow);

        public static IntPtr SetCursor(this WinExpander expander, IntPtr hCursor) { return SetCursor(hCursor); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr SetCursor(IntPtr hCursor);

        public static IntPtr CopyCursor(this WinExpander expander, IntPtr hCursor) { return CopyCursor(hCursor); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr CopyCursor(IntPtr hCursor);

        public static bool GetCursorInfo(this WinExpander expander, ref CursorInfo info) { return GetCursorInfo(ref info); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool GetCursorInfo(ref CursorInfo info);

        public static bool AnimateWindow(this WinExpander expander, IntPtr hwnd, int dwTime, int dwFlags) { return AnimateWindow(hwnd, dwTime, dwFlags); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);

        public static IntPtr FindWindow(this WinExpander expander, string lpClassName, string lpWindowName) { return FindWindow(lpClassName, lpWindowName); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static bool UpdateWindow(this WinExpander expander, IntPtr hwnd) { return UpdateWindow(hwnd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool UpdateWindow(IntPtr hwnd);

        public static IntPtr CreateWindowEx(this WinExpander expander, int dwExStyle, string lpClassName, string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hwndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam) { return CreateWindowEx(dwExStyle, lpClassName, lpWindowName, dwStyle, x, y, nWidth, nHeight, hwndParent, hMenu, hInstance, lpParam); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr CreateWindowEx(int dwExStyle, string lpClassName, string lpWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hwndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        public static bool GetWindowRect(this WinExpander expander, IntPtr hwnd, out Rectangle lpRect) { return GetWindowRect(hwnd, out lpRect); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool GetWindowRect(IntPtr hwnd, out Rectangle lpRect);

        public static bool GetClientRect(this WinExpander expander, IntPtr hwnd, out Rectangle lpRect) { return GetClientRect(hwnd, out lpRect); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool GetClientRect(IntPtr hwnd, out Rectangle lpRect);

        public static bool EnumWindows(this WinExpander expander, EnumWindowsProc lpEnumFunc, IntPtr lParam) { return EnumWindows(lpEnumFunc, lParam); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        public static IntPtr FindWindowEx(this WinExpander expander, IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow) { return FindWindowEx(hwndParent, hwndChildAfter, lpszClass, lpszWindow); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        public static IntPtr GetTopWindow(this WinExpander expander) { return GetTopWindow(); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr GetTopWindow();

        public static IntPtr GetNextWindow(this WinExpander expander, IntPtr hwnd, uint wCmd) { return GetNextWindow(hwnd, wCmd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr GetNextWindow(IntPtr hwnd, uint wCmd);

        public static IntPtr GetWindow(this WinExpander expander, IntPtr hwnd, uint wCmd) { return GetWindow(hwnd, wCmd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr GetWindow(IntPtr hwnd, uint wCmd);

        public static bool AllowSetForegroundWindow(this WinExpander expander, uint dwProcessId) { return AllowSetForegroundWindow(dwProcessId); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool AllowSetForegroundWindow(uint dwProcessId);

        public static bool SetForegroundWindow(this WinExpander expander, IntPtr hwnd) { return SetForegroundWindow(hwnd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool SetForegroundWindow(IntPtr hwnd);

        public static bool BringWindowToTop(this WinExpander expander, IntPtr hwnd) { return BringWindowToTop(hwnd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool BringWindowToTop(IntPtr hwnd);

        public static bool SetWindowPos(this WinExpander expander, IntPtr hwnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags) { return SetWindowPos(hwnd, hWndInsertAfter, x, y, cx, cy, flags); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);

        public static int GetWindowText(this WinExpander expander, IntPtr hWnd, StringBuilder lpString, int nMaxCount) { return GetWindowText(hWnd, lpString, nMaxCount); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public static int GetWindowTextLength(this WinExpander expander, IntPtr hWnd) { return GetWindowTextLength(hWnd); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        public static bool SetWindowText(this WinExpander expander, IntPtr hwnd, string lpString) { return SetWindowText(hwnd, lpString); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool SetWindowText(IntPtr hwnd, string lpString);

        public static bool MoveWindow(this WinExpander expander, IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint) { return MoveWindow(hWnd, x, y, nWidth, nHeight, bRepaint); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        public static bool GetWindowInfo(this WinExpander expander, IntPtr hwnd, [In] [Out] ref WindowInfo pwi) { return GetWindowInfo(hwnd, ref pwi); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool GetWindowInfo(IntPtr hwnd, [In] [Out] ref WindowInfo pwi);

        public static bool SetWindowPlacement(this WinExpander expander, IntPtr hWnd, [In] ref WindowPlacement lpwndpl) { return SetWindowPlacement(hWnd, ref lpwndpl); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WindowPlacement lpwndpl);

        public static bool GetWindowPlacement(this WinExpander expander, IntPtr hWnd, out WindowPlacement lpwndpl) { return GetWindowPlacement(hWnd, out lpwndpl); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool GetWindowPlacement(IntPtr hWnd, out WindowPlacement lpwndpl);

        public static bool RedrawWindow(this WinExpander expander, IntPtr hWnd, [In] ref Rectangle lprcUpdate, IntPtr hrgnUpdate, int flags) { return RedrawWindow(hWnd, ref lprcUpdate, hrgnUpdate, flags); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool RedrawWindow(IntPtr hWnd, [In] ref Rectangle lprcUpdate, IntPtr hrgnUpdate, int flags);

        public static bool DestroyWindow(this WinExpander expander, IntPtr hwnd) { return DestroyWindow(hwnd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool DestroyWindow(IntPtr hwnd);

        public static bool CloseWindow(this WinExpander expander, IntPtr hwnd) { return CloseWindow(hwnd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool CloseWindow(IntPtr hwnd);

        public static ushort RegisterClassEx(this WinExpander expander, [In] ref WindowClassEx lpwcx) { return RegisterClassEx(ref lpwcx); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern ushort RegisterClassEx([In] ref WindowClassEx lpwcx);

        public static ushort RegisterClassEx(this WinExpander expander, [In] ref WindowClassExBlittable lpwcx) { return RegisterClassEx(ref lpwcx); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern ushort RegisterClassEx([In] ref WindowClassExBlittable lpwcx);

        public static bool UnregisterClass(this WinExpander expander, string lpClassName, IntPtr hInstance) { return UnregisterClass(lpClassName, hInstance); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool UnregisterClass(string lpClassName, IntPtr hInstance);

        public static bool GetClassInfoEx(this WinExpander expander, IntPtr hInstance, string lpClassName, out WindowClassExBlittable lpWndClass) { return GetClassInfoEx(hInstance, lpClassName, out lpWndClass); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool GetClassInfoEx(IntPtr hInstance, string lpClassName, out WindowClassExBlittable lpWndClass);

        public static int GetClassName(this WinExpander expander, IntPtr hWnd, StringBuilder lpClassName, int nMaxCount) { return GetClassName(hWnd, lpClassName, nMaxCount); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public static IntPtr DefWindowProc(this WinExpander expander, IntPtr hwnd, uint uMsg, IntPtr wParam, IntPtr lParam) { return DefWindowProc(hwnd, uMsg, wParam, lParam); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr DefWindowProc(IntPtr hwnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        public static bool PeekMessage(this WinExpander expander, out Message lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg) { return PeekMessage(out lpMsg, hWnd, wMsgFilterMin, wMsgFilterMax, wRemoveMsg); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool PeekMessage(out Message lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

        public static IntPtr DispatchMessage(this WinExpander expander, [In] ref Message lpMsg) { return DispatchMessage(ref lpMsg); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr DispatchMessage([In] ref Message lpMsg);

        public static bool TranslateMessage(this WinExpander expander, [In] ref Message lpMsg) { return TranslateMessage(ref lpMsg); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool TranslateMessage([In] ref Message lpMsg);

        public static int GetMessage(this WinExpander expander, out Message lpMsg, IntPtr hwnd, uint wMsgFilterMin, uint wMsgFilterMax) { return GetMessage(out lpMsg, hwnd, wMsgFilterMin, wMsgFilterMax); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int GetMessage(out Message lpMsg, IntPtr hwnd, uint wMsgFilterMin, uint wMsgFilterMax);

        public static void PostQuitMessage(this WinExpander expander, int nExitCode) { PostQuitMessage(nExitCode); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern void PostQuitMessage(int nExitCode);

        public static IntPtr GetMessageExtraInfo(this WinExpander expander) { return GetMessageExtraInfo(); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr GetMessageExtraInfo();

        public static IntPtr SetMessageExtraInfo(this WinExpander expander, IntPtr lParam) { return SetMessageExtraInfo(lParam); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr SetMessageExtraInfo(IntPtr lParam);

        public static bool WaitMessage(this WinExpander expander) { return WaitMessage(); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool WaitMessage();

        public static IntPtr SendMessage(this WinExpander expander, IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam) { return SendMessage(hwnd, msg, wParam, lParam); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        public static bool SendNotifyMessage(this WinExpander expander, IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam) { return SendNotifyMessage(hwnd, msg, wParam, lParam); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool SendNotifyMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        public static bool PostMessage(this WinExpander expander, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam) { return PostMessage(hWnd, msg, wParam, lParam); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        public static bool ReplyMessagethis(WinExpander expander, IntPtr lResult) { return ReplyMessage(lResult); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool ReplyMessage(IntPtr lResult);

        public static bool GetInputState(this WinExpander expander) { return GetInputState(); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool GetInputState();

        public static uint GetMessagePos(this WinExpander expander) { return GetMessagePos(); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern uint GetMessagePos();

        public static uint GetMessageTime(this WinExpander expander) { return GetMessageTime(); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern uint GetMessageTime();

        public static bool InSendMessage(this WinExpander expander) { return InSendMessage(); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool InSendMessage();

        public static uint GetQueueStatus(this WinExpander expander, int flags) { return GetQueueStatus(flags); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern uint GetQueueStatus(int flags);

        public static bool PostThreadMessage(this WinExpander expander, uint threadId, uint msg, IntPtr wParam, IntPtr lParam) { return PostThreadMessage(threadId, msg, wParam, lParam); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool PostThreadMessage(uint threadId, uint msg, IntPtr wParam, IntPtr lParam);

        public static bool OpenClipboard(this WinExpander expander, IntPtr hWndNewOwner) { return OpenClipboard(hWndNewOwner); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        public static bool CloseClipboard(this WinExpander expander) { return CloseClipboard(); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool CloseClipboard();

        public static IntPtr GetClipboardData(this WinExpander expander, uint uFormat) { return GetClipboardData(uFormat); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr GetClipboardData(uint uFormat);

        public static IntPtr SetClipboardData(this WinExpander expander, uint uFormat, IntPtr handle) { return SetClipboardData(uFormat, handle); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr SetClipboardData(uint uFormat, IntPtr handle);

        public static bool EmptyClipboard(this WinExpander expander) { return EmptyClipboard(); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool EmptyClipboard();

        public static IntPtr SetClipboardViewer(this WinExpander expander, IntPtr hWndNewViewer) { return SetClipboardViewer(hWndNewViewer); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        public static bool ChangeClipboardChain(this WinExpander expander, IntPtr hWndRemove, IntPtr hWndNewNext) { return ChangeClipboardChain(hWndRemove, hWndNewNext); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        public static bool AddClipboardFormatListener(this WinExpander expander, IntPtr hwnd) { return AddClipboardFormatListener(hwnd); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool AddClipboardFormatListener(IntPtr hwnd);

        public static int GetPriorityClipboardFormat(this WinExpander expander, IntPtr paFormatPriorityList, int cFormats) { return GetPriorityClipboardFormat(paFormatPriorityList, cFormats); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern int GetPriorityClipboardFormat(IntPtr paFormatPriorityList, int cFormats);

        public static uint EnumClipboardFormats(this WinExpander expander, uint format) { return EnumClipboardFormats(format); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern uint EnumClipboardFormats(uint format);

        public static bool CompareObjectHandles(this WinExpander expander, IntPtr hFirstObjectHandle, IntPtr hSecondObjectHandle) { return CompareObjectHandles(hFirstObjectHandle, hSecondObjectHandle); }
        [DllImport("kernelbase", ExactSpelling = true)]
        private static extern bool CompareObjectHandles(IntPtr hFirstObjectHandle, IntPtr hSecondObjectHandle);

        public static bool ReleaseCapture(this WinExpander expander) { return ReleaseCapture(); }
        [DllImport("user32", ExactSpelling = true)]
        private static extern bool ReleaseCapture();

        public static IntPtr CallWindowProc(this WinExpander expander, IntPtr lpPrevWndFunc, IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam) { return CallWindowProc(lpPrevWndFunc, hWnd, uMsg, wParam, lParam); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        public static IntPtr CallWindowProc(this WinExpander expander, WindowProc lpPrevWndFunc, IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam) { return CallWindowProc(lpPrevWndFunc, hWnd, uMsg, wParam, lParam); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr CallWindowProc(WindowProc lpPrevWndFunc, IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        public static IntPtr SetParent(this WinExpander expander, IntPtr hWndChild, IntPtr hWndParent) { return SetParent(hWndChild, hWndParent); }
        [DllImport("User32", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndParent);

        public static bool ShowWindow(this WinExpander expander, IntPtr hWndChild, int nCmdShow) { return ShowWindow(hWndChild, nCmdShow); }
        [DllImport("User32", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool ShowWindow(IntPtr hWndChild, int nCmdShow);

        public static IntPtr DefMDIChildProc(this WinExpander expander, IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam) { return DefMDIChildProc(hWnd, uMsg, wParam, lParam); }
        [DllImport("user32.dll", EntryPoint = "DefMDIChildProcW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr DefMDIChildProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        public static IntPtr DefWindowProc(this WinExpander expander, IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam) { return DefWindowProc(hWnd, uMsg, wParam, lParam); }
        [DllImport("user32.dll", EntryPoint = "DefWindowProcW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr DefWindowProc(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam);

        public static IntPtr DefFrameProc(this WinExpander expander, IntPtr hWnd, IntPtr hWndClient, int msg, IntPtr wParam, IntPtr lParam) { return DefFrameProc(hWnd, hWndClient, msg, wParam, lParam); }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr DefFrameProc(IntPtr hWnd, IntPtr hWndClient, int msg, IntPtr wParam, IntPtr lParam);

        public static bool IsWindow(this WinExpander expander, IntPtr hWnd) { return IsWindow(hWnd); }
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern bool IsWindow(IntPtr hWnd);

        public static bool IsWindowVisible(this WinExpander expander, IntPtr hWnd) { return IsWindowVisible(hWnd); }
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        public static KeyState GetAsyncKeyState(this WinExpander expander, int vKey) { return GetAsyncKeyState(vKey); }
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern KeyState GetAsyncKeyState(int vKey);

        [DllImport("shell32.dll", EntryPoint = "ShellAboutW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 ShellAbout(IntPtr hWnd, char* szApp, char* szOtherStuff, IntPtr hIcon);

        public static Int32 ShellExecute(this WinExpander expander, IntPtr hWnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd) { return ShellExecute(hWnd, lpOperation, lpFile, lpParameters, lpDirectory, nShowCmd); }
        [DllImport("shell32.dll", EntryPoint = "ShellExecuteW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 ShellExecute(IntPtr hWnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        public static IntPtr GetDC(this WinExpander expander, IntPtr hWnd) { return GetDC(hWnd); }
        [DllImport("user32.dll", EntryPoint = "GetDC", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetDC(IntPtr hWnd);

        public static Int32 GetSystemMetrics(this WinExpander expander, Int32 nIndex) { return GetSystemMetrics(nIndex); }
        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int GetSystemMetrics(Int32 nIndex);

        public static IntPtr LoadLibrary(this WinExpander expander, string lpLibFileName) { return LoadLibrary(lpLibFileName); }
        [DllImport("kernel32.dll", EntryPoint = "LoadLibraryW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr LoadLibrary(string lpLibFileName);

        public static IntPtr FreeLibrary(this WinExpander expander, IntPtr hModule) { return FreeLibrary(hModule); }
        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr FreeLibrary(IntPtr hModule);

        public static IntPtr GetProcAddress(this WinExpander expander, IntPtr hModule, string lpProcName) { return GetProcAddress(hModule, lpProcName); }
        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        public static Int32 SendMessage(this WinExpander expander, IntPtr hWnd, uint msg, int wParam, int lParam) { return SendMessage(hWnd, msg, wParam, lParam); }
        [DllImport("user32.dll", EntryPoint = "SendMessageW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        public static Int32 PostMessage(this WinExpander expander, IntPtr hWnd, uint msg, int wParam, int lParam) { return PostMessage(hWnd, msg, wParam, lParam); }
        [DllImport("user32.dll", EntryPoint = "PostMessageW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        public static Int32 FillRect(this WinExpander expander, IntPtr hDc, RECT* pRect, Int32 nColor) { return FillRect(hDc, pRect, nColor); }
        [DllImport("user32.dll", EntryPoint = "FillRect", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 FillRect(IntPtr hDc, RECT* pRect, Int32 nColor);

        public static Int32 DrawText(this WinExpander expander, IntPtr hDc, char* pString, Int32 nCount, RECT* pRect, uint uFormat) { return DrawText(hDc, pString, nCount, pRect, uFormat); }
        [DllImport("user32.dll", EntryPoint = "DrawTextW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern Int32 DrawText(IntPtr hDc, char* pString, Int32 nCount, RECT* pRect, uint uFormat);

        public static UInt32 ClientToScreen(this WinExpander expander, IntPtr hWnd, POINT* lpPoint) { return ClientToScreen(hWnd, lpPoint); }
        [DllImport("user32.dll", EntryPoint = "ClientToScreen", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 ClientToScreen(IntPtr hWnd, POINT* lpPoint);

        public static  UInt32 ScreenToClient(this WinExpander expander, IntPtr hWnd, POINT* lpPoint) { return ScreenToClient(hWnd, lpPoint); }
        [DllImport("user32.dll", EntryPoint = "ScreenToClient", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 ScreenToClient(IntPtr hWnd, POINT* lpPoint);

        public static int GetLastError(this WinExpander expander) { return GetLastError(); }
        [DllImport("kernel32.dll", EntryPoint = "GetLastError", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int GetLastError();

        public static int GetWindowLong(this WinExpander expander, IntPtr hWnd, int nIndex) { return GetWindowLong(hWnd, nIndex); }
        public static IntPtr GetWindowLongPtr(this WinExpander expander, IntPtr hwnd, int nIndex) { return IntPtr.Size > 4 ? GetWindowLongPtr_x64(hwnd, nIndex) : new IntPtr(GetWindowLong(hwnd, nIndex)); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int GetWindowLong(IntPtr hwnd, int nIndex);
        [DllImport("user32", CharSet = CharSet.Auto, EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr_x64(IntPtr hwnd, int nIndex);

        public static int SetWindowLong(this WinExpander expander, IntPtr hWnd, int nIndex, int dwNewLong) { return SetWindowLong(hWnd, nIndex, dwNewLong); }
        public static IntPtr SetWindowLongPtr(this WinExpander expander, IntPtr hwnd, int nIndex, IntPtr dwNewLong) { return IntPtr.Size > 4 ? SetWindowLongPtr_x64(hwnd, nIndex, dwNewLong) : new IntPtr(SetWindowLong(hwnd, nIndex, dwNewLong.ToInt32())); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr_x64(IntPtr hwnd, int nIndex, IntPtr dwNewLong);

        public static IntPtr GetClassLongPtr(this WinExpander expander, IntPtr hwnd, int nIndex) { return IntPtr.Size > 4 ? GetClassLongPtr_x64(hwnd, nIndex) : new IntPtr(unchecked((int)GetClassLong(hwnd, nIndex))); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern uint GetClassLong(IntPtr hWnd, int nIndex);
        [DllImport("user32", CharSet = CharSet.Auto, EntryPoint = "GetClassLongPtr")]
        private static extern IntPtr GetClassLongPtr_x64(IntPtr hWnd, int nIndex);

        public static IntPtr SetClassLongPtr(this WinExpander expander, IntPtr hWnd, int nIndex, IntPtr dwNewLong) { return IntPtr.Size > 4 ? SetClassLongPtr_x64(hWnd, nIndex, dwNewLong) : new IntPtr(unchecked((int)SetClassLong(hWnd, nIndex, dwNewLong.ToInt32()))); }
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern uint SetClassLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32", CharSet = CharSet.Auto, EntryPoint = "SetClassLongPtr")]
        private static extern IntPtr SetClassLongPtr_x64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        public static IntPtr CreateFile(this WinExpander expander, string szFileName, uint dwDesiredAccess, uint dwShareMode, uint lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile) { return CreateFile(szFileName, dwDesiredAccess, dwShareMode, lpSecurityAttributes, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile); }
        [DllImport("kernel32.dll", EntryPoint = "CreateFileW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr CreateFile(string szFileName, uint dwDesiredAccess, uint dwShareMode, uint lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        public static int GetFileSize(this WinExpander expander, IntPtr hFile, int lpFileSizeHigh) { return GetFileSize(hFile, lpFileSizeHigh); }
        [DllImport("kernel32.dll", EntryPoint = "GetFileSize", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int GetFileSize(IntPtr hFile, int lpFileSizeHigh);

        public static bool WriteFile(this WinExpander expander, IntPtr hFile, void* lpBuffer, int nNumberOfBytesToWrite, int* lpNumberOfBytesWritten, int lpOverlapped) { return WriteFile(hFile, lpBuffer, nNumberOfBytesToWrite, lpNumberOfBytesWritten, lpOverlapped); }
        [DllImport("kernel32.dll", EntryPoint = "WriteFile", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool WriteFile(IntPtr hFile, void* lpBuffer, int nNumberOfBytesToWrite, int* lpNumberOfBytesWritten, int lpOverlapped);

        public static bool ReadFile(this WinExpander expander, IntPtr hFile, void* pBuffer, int nNumberOfBytesToRead, int* lpNumberOfBytesRead, int lpOverlapped) { return ReadFile(hFile, pBuffer, nNumberOfBytesToRead, lpNumberOfBytesRead, lpOverlapped); }
        [DllImport("kernel32.dll", EntryPoint = "ReadFile", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool ReadFile(IntPtr hFile, void* pBuffer, int nNumberOfBytesToRead, int* lpNumberOfBytesRead, int lpOverlapped);

        public static int LockFile(this WinExpander expander, IntPtr hFile, int dwFileOffsetLow, int dwFileOffsetHigh, int nNumberOfBytesToLockLow, int nNumberOfBytesToLockHigh) { return LockFile(hFile, dwFileOffsetLow, dwFileOffsetHigh, nNumberOfBytesToLockLow, nNumberOfBytesToLockHigh); }
        [DllImport("kernel32.dll", EntryPoint = "LockFile", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int LockFile(IntPtr hFile, int dwFileOffsetLow, int dwFileOffsetHigh, int nNumberOfBytesToLockLow, int nNumberOfBytesToLockHigh);

        public static bool UnlockFile(this WinExpander expander, IntPtr hFile, int dwFileOffsetLow, int dwFileOffsetHigh, int nNumberOfBytesToUnlockLow, int nNumberOfBytesToUnlockHigh) { return UnlockFile(hFile, dwFileOffsetLow, dwFileOffsetHigh, nNumberOfBytesToUnlockLow, nNumberOfBytesToUnlockHigh); }
        [DllImport("kernel32.dll", EntryPoint = "UnlockFile", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnlockFile(IntPtr hFile, int dwFileOffsetLow, int dwFileOffsetHigh, int nNumberOfBytesToUnlockLow, int nNumberOfBytesToUnlockHigh);

        public static uint SetFilePointer(this WinExpander expander, IntPtr hFile, int lDistanceToMove, int lpDistanceToMoveHigh, uint dwMoveMethod) { return SetFilePointer(hFile, lDistanceToMove, lpDistanceToMoveHigh, dwMoveMethod); }
        [DllImport("kernel32.dll", EntryPoint = "SetFilePointer", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern uint SetFilePointer(IntPtr hFile, int lDistanceToMove, int lpDistanceToMoveHigh, uint dwMoveMethod);

        public static bool SetEndOfFile(this WinExpander expander, IntPtr hFile) { return SetEndOfFile(hFile); }
        [DllImport("kernel32.dll", EntryPoint = "SetEndOfFile", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool SetEndOfFile(IntPtr hFile);

        public static int CloseHandle(this WinExpander expander, IntPtr hObject) { return CloseHandle(hObject); }
        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int CloseHandle(IntPtr hObject);

        public static void NetApiBufferFree(this WinExpander expander, IntPtr pointerBuffer) { NetApiBufferFree(pointerBuffer); }
        [DllImport("netapi32.dll", EntryPoint = "NetApiBufferFree")]
        private static extern void NetApiBufferFree(IntPtr pointerBuffer);

        public static uint NetServerEnum(this WinExpander expander, IntPtr serverName, uint level, ref IntPtr pointerBuffer, uint prefMaxLen, ref uint entriesRead, ref uint totalEntries, uint serverType, [MarshalAs(UnmanagedType.LPWStr)] string domain, IntPtr resumeHandle) { return NetServerEnum(serverName, level, ref pointerBuffer, prefMaxLen, ref entriesRead, ref totalEntries, serverType, domain, resumeHandle); }
        [DllImport("netapi32.dll", EntryPoint = "NetServerEnum")]
        private static extern uint NetServerEnum(IntPtr serverName, uint level, ref IntPtr pointerBuffer, uint prefMaxLen, ref uint entriesRead, ref uint totalEntries, uint serverType, [MarshalAs(UnmanagedType.LPWStr)] string domain, IntPtr resumeHandle);

        public static uint NetServerGetInfo(this WinExpander expander, [MarshalAs(UnmanagedType.LPWStr)] string serverName, int level, ref IntPtr pointerBuffer) { return NetServerGetInfo(serverName, level, ref pointerBuffer); }
        [DllImport("netapi32.dll", EntryPoint = "NetServerGetInfo")]
        private static extern uint NetServerGetInfo([MarshalAs(UnmanagedType.LPWStr)] string serverName, int level, ref IntPtr pointerBuffer);

        public static IntPtr OpenInputDesktop(this WinExpander expander, int dwFlags, [MarshalAs(UnmanagedType.Bool)] bool fInherit, int dwDesiredAccess) { return OpenInputDesktop(dwFlags, fInherit, dwDesiredAccess); }
        [DllImport("user32.dll")]
        private static extern IntPtr OpenInputDesktop(int dwFlags, [MarshalAs(UnmanagedType.Bool)] bool fInherit, int dwDesiredAccess);

        public static bool CloseDesktop(this WinExpander expander, IntPtr hDesktop) { return CloseDesktop(hDesktop); }
        [DllImport("user32.dll")]
        private static extern bool CloseDesktop(IntPtr hDesktop);

        public static IntPtr SetWindowsHookEx(this WinExpander expander, int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId) { return SetWindowsHookEx(idHook, lpfn, hMod, dwThreadId); }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        public static bool UnhookWindowsHookEx(this WinExpander expander, IntPtr hhk) { return UnhookWindowsHookEx(hhk); }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        public static IntPtr CallNextHookEx(this WinExpander expander, IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam) { return CallNextHookEx(hhk, nCode, wParam, lParam); }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        public static IntPtr GetModuleHandle(this WinExpander expander, string lpModuleName) { return GetModuleHandle(lpModuleName); }
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public static IntPtr SHGetFileInfo(this WinExpander expander, string pszPath, int dwFileAttributes, ref SHFILEINFO shinfo, uint cbfileInfo, int uFlags) { return SHGetFileInfo(pszPath, dwFileAttributes, ref shinfo, cbfileInfo, uFlags); }
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO shinfo, uint cbfileInfo, int uFlags);

        public static uint GetWindowThreadProcessId(this WinExpander expander, [In] IntPtr hWnd, [Out, Optional] IntPtr lpdwProcessId) { return GetWindowThreadProcessId(hWnd, lpdwProcessId); }
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern uint GetWindowThreadProcessId([In] IntPtr hWnd, [Out, Optional] IntPtr lpdwProcessId);

        public static IntPtr GetForegroundWindow(this WinExpander expander) { return GetForegroundWindow(); }
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        public static ushort GetKeyboardLayout(this WinExpander expander, int idThread) { return GetKeyboardLayout(idThread); }
        [DllImport("user32.dll", SetLastError = true)]
        private static extern ushort GetKeyboardLayout([In] int idThread);

        public static bool CryptAcquireContext(this WinExpander expander, ref IntPtr hProv, string pszContainer, string pszProvider, uint dwProvType, uint dwFlags) { return CryptAcquireContext(ref hProv, pszContainer, pszProvider, dwProvType, dwFlags); }
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptAcquireContext(ref IntPtr hProv, string pszContainer, string pszProvider, uint dwProvType, uint dwFlags);

        public static bool CryptGetProvParam(this WinExpander expander, IntPtr hProv, uint dwParam, [In, Out] byte[] pbData, ref uint dwDataLen, uint dwFlags) { return CryptGetProvParam(hProv, dwParam, pbData, ref dwDataLen, dwFlags); }
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptGetProvParam(IntPtr hProv, uint dwParam, [In, Out] byte[] pbData, ref uint dwDataLen, uint dwFlags);

        public static bool CryptGetProvParam(this WinExpander expander, IntPtr hProv, uint dwParam, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pbData, ref uint dwDataLen, uint dwFlags) { return CryptGetProvParam(hProv, dwParam, pbData, ref dwDataLen, dwFlags); }
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptGetProvParam(IntPtr hProv, uint dwParam, [MarshalAs(UnmanagedType.LPStr)] StringBuilder pbData, ref uint dwDataLen, uint dwFlags);

        public static bool CryptExportPublicKeyInfo(this WinExpander expander, IntPtr hProv, uint dwKeySpec, uint dwCertEncodingType, IntPtr pInfo, ref uint pcbInfo) { return CryptExportPublicKeyInfo(hProv, dwKeySpec, dwCertEncodingType, pInfo, ref pcbInfo); }
        [DllImport("Crypt32.dll", EntryPoint = "CryptExportPublicKeyInfo", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CryptExportPublicKeyInfo(IntPtr hProv, uint dwKeySpec, uint dwCertEncodingType, IntPtr pInfo, ref uint pcbInfo);

        public static bool CryptReleaseContext(this WinExpander expander, IntPtr hProv, Int32 dwFlags) { return CryptReleaseContext(hProv, dwFlags); }
        [DllImport("Advapi32.dll", EntryPoint = "CryptReleaseContext", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CryptReleaseContext(IntPtr hProv, Int32 dwFlags);

        public static int MakeLong(this WinExpander expander, int loWord, int hiWord)
        {
            return (hiWord << 16) | (loWord & 0xffff);
        }

        public static IntPtr MakeLParam(this WinExpander expander, int loWord, int hiWord)
        {
            return (IntPtr)((hiWord << 16) | (loWord & 0xffff));
        }

        public static uint HiWord(this WinExpander expander, uint nNumber)
        {
            return (nNumber >> 16) & 0xffff;
        }

        public static uint LoWord(this WinExpander expander, uint nNumber)
        {
            return nNumber & 0xffff;
        }

        public static int ShellAbout(this WinExpander expander, IntPtr ownerHandle, string title, string appName, string companyName)
        {
            fixed (char* szApp = title + "#" + appName)
            {
                fixed (char* szOtherStuff = companyName)
                {
                    return ShellAbout(ownerHandle, szApp, szOtherStuff, IntPtr.Zero);
                }
            }
        }

        public static void ShowCalendar(this WinExpander expander, IntPtr owner)
        {
            ShellExecute(owner, "open", "Rundll32.exe", "shell32.dll,Control_RunDLL TIMEDATE.CPL", "C:\\WINDOWS\\SYSTEM32\\", Je.win.SW_SHOWNORMAL);
        }

        public static IntPtr CreateFile(this WinExpander expander, string fileName)
        {
            return CreateFile(fileName,
                (uint)(Je.win.FILE_GENERIC_READ | Je.win.FILE_GENERIC_WRITE),
                (uint)(Je.win.FILE_SHARE_READ | Je.win.FILE_SHARE_WRITE),
                0,
                (uint)Je.win.OPEN_ALWAYS,
                (uint)Je.win.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
        }

        public static int GetFileSize(this WinExpander expander, IntPtr hFile)
        {
            return GetFileSize(hFile, 0);
        }

        public static bool LockFile(this WinExpander expander, IntPtr hFile, int offset, int nNumberOfBytesToLock)
        {
            return LockFile(hFile, offset, 0, (int)LoWord(expander, (uint)nNumberOfBytesToLock), (int)HiWord(expander, (uint)nNumberOfBytesToLock)) != 0;
        }

        public static bool UnlockFile(this WinExpander expander, IntPtr hFile, int offset, int nNumberOfBytesToUnlock)
        {
            return UnlockFile(hFile, offset, 0, (int)LoWord(expander, (uint)nNumberOfBytesToUnlock), (int)HiWord(expander, (uint)nNumberOfBytesToUnlock));
        }

        public static bool SetFilePointer(this WinExpander expander, IntPtr hFile, int offset, SeekOrigin seekOrigin)
        {
            uint result = SetFilePointer(hFile, offset, 0, (uint)(seekOrigin == SeekOrigin.Begin ? Je.win.FILE_BEGIN : seekOrigin == SeekOrigin.End ? Je.win.FILE_END : Je.win.FILE_CURRENT));
            return result == (int)Je.win.INVALID_SET_FILE_POINTER;
        }

        public static bool WriteFile(this WinExpander expander, IntPtr hFile, object data)
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

        public static object ReadFile(this WinExpander expander, IntPtr hFile, Type type)
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

        public static int GetMdiCreateStruct(this WinExpander expander, string className, string title, IntPtr owner, int x, int y, int cx, int cy, uint style)
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

        public static sbyte WindowVisible(this WinExpander expander, IntPtr lParam)
        {
            var p = (WindowPosStruct*)lParam.ToPointer();
            WindowPosStruct wp = *p;
            var flags = wp.flags;
            if ((flags & Je.win.SWP_HIDEWINDOW) == Je.win.SWP_HIDEWINDOW) return -1;
            if ((flags & Je.win.SWP_SHOWWINDOW) == Je.win.SWP_SHOWWINDOW) return 1;
            return 0;
        }

        public static POINT ClientToScreen(this WinExpander expander, IntPtr hWnd, POINT pOld)
        {
            POINT* lpPointer = &pOld;
            UInt32 retcode = ClientToScreen(hWnd, lpPointer);
            if (retcode == 0) return pOld;
            POINT pNew = *lpPointer;
            return pNew;
        }

        public static POINT ScreenToClient(this WinExpander expander, IntPtr hWnd, POINT pOld)
        {
            POINT* lpPointer = &pOld;
            UInt32 retcode = ScreenToClient(hWnd, lpPointer);
            if (retcode == 0) return pOld;
            POINT pNew = *lpPointer;
            return pNew;
        }

        public static int DrawText(this WinExpander expander, IntPtr hDc, object value, RECT rect, uint format)
        {
            var text = value.As<string>();
            fixed (char* pString = text)
            {
                return DrawText(hDc, pString, text.Length, &rect, format);
            }
        }

        public static string[] GetSqlServers(this WinExpander expander)
        {
            var sqlServers = new LanCollection((uint)Je.win.SV_TYPE_SQLSERVER);
            var list = new List<string>();
            foreach (var sqlServer in sqlServers) list.Add(sqlServer.As<string>());
            return list.ToArray();
        }

        public static SHFILEINFO ShellInfo(this WinExpander expander, string fileName)
        {
            var flags = (
                Je.win.SHGFI_DISPLAYNAME |
                Je.win.SHGFI_ICON |
                Je.win.SHGFI_TYPENAME |
                Je.win.SHGFI_USEFILEATTRIBUTES);
            var fileAttributes = Je.win.FILE_ATTRIBUTE_NORMAL;
            var shellFileInfo = new SHFILEINFO(true);
            var size = (uint)Marshal.SizeOf(shellFileInfo);
            var fullName = fileName;//FileNameAsPathFileExt(fileName);
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

        public static MdiClient MdiClientDisableScrolling(this WinExpander expander, Form form)
        {
            var nonScrollableMdiClient = new NonScrollableMdiClient();
            foreach (Control control in form.Controls)
            {
                if (!(control is MdiClient)) continue;
                var oldWindowStyle = GetWindowLong(control.Handle, Je.win.GWL_EXSTYLE);
                var newWindowStyle = new IntPtr(oldWindowStyle ^ Je.win.WS_EX_CLIENTEDGE);
                SetWindowLongPtr(expander, control.Handle, Je.win.GWL_EXSTYLE, newWindowStyle);
                nonScrollableMdiClient.AssignHandle(control.Handle);
                control.HandleDestroyed += nonScrollableMdiClient.OnHandleDestroyed;
                return control as MdiClient;
            }
            return null;
        }

        public static string GetKeyboardLayout(this WinExpander expander)
        {
            switch (GetKeyboardLayout((int)GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero)))
            {
                case 1049: return "RU";
                case 1033: return "EN";
                default: return "";
            }
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
            var dwFlags = (uint)Je.win.CRYPT_FIRST;
            StringBuilder sb = null;
            try
            {
                CryptAcquireContext(ref hProv, null, null, (uint)Je.win.CRYPT_PROV_TYPE, Je.win.CRYPT_VERIFYCONTEXT);
                CryptGetProvParam(hProv, (uint)Je.win.PP_ENUMCONTAINERS, sb, ref pdwDataLen, dwFlags);
                var buffsize = (int)(2 * pdwDataLen);
                sb = new StringBuilder(buffsize);
                while (CryptGetProvParam(hProv, (uint)Je.win.PP_ENUMCONTAINERS, sb, ref pdwDataLen, dwFlags | (uint)Je.win.CRYPT_FQCN))
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
                CryptAcquireContext(ref hProv, container, null, (uint)Je.win.CRYPT_PROV_TYPE, 0);
                CryptExportPublicKeyInfo(hProv, (uint)Je.win.AT_KEYEXCHANGE, (uint)Je.win.X509_ASN_ENCODING | (uint)Je.win.PKCS_7_ASN_ENCODING, IntPtr.Zero, ref pcbInfo);
                pInfo = Marshal.AllocHGlobal((int)pcbInfo);
                Marshal.StructureToPtr(info, pInfo, false);
                CryptExportPublicKeyInfo(hProv, (uint)Je.win.AT_KEYEXCHANGE, (uint)Je.win.X509_ASN_ENCODING | (uint)Je.win.PKCS_7_ASN_ENCODING, pInfo, ref pcbInfo);
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