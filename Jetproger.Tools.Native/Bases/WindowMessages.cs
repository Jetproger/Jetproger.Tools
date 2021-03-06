﻿namespace Jetproger.Tools.WinApi
{
    public static unsafe partial class Native
    {
        public static class WindowMessages
        {
            public const uint WM_NULL = 0X0000;
            public const uint WM_CREATE = 0X0001;
            public const uint WM_DESTROY = 0X0002;
            public const uint WM_MOVE = 0X0003;
            public const uint WM_SIZE = 0X0005;
            public const uint WM_ACTIVATE = 0X0006;
            public const uint WM_SETFOCUS = 0X0007;
            public const uint WM_KILLFOCUS = 0X0008;
            public const uint WM_ENABLE = 0X000A;
            public const uint WM_SETREDRAW = 0X000B;
            public const uint WM_SETTEXT = 0X000C;
            public const uint WM_GETTEXT = 0X000D;
            public const uint WM_GETTEXTLENGTH = 0X000E;
            public const uint WM_PAINT = 0X000F;
            public const uint WM_CLOSE = 0X0010;
            public const uint WM_QUERYENDSESSION = 0X0011;
            public const uint WM_QUERYOPEN = 0X0013;
            public const uint WM_ENDSESSION = 0X0016;
            public const uint WM_QUIT = 0X0012;
            public const uint WM_ERASEBKGND = 0X0014;
            public const uint WM_SYSCOLORCHANGE = 0X0015;
            public const uint WM_SHOWWINDOW = 0X0018;
            public const uint WM_WININICHANGE = 0X001A;
            public const uint WM_SETTINGCHANGE = WM_WININICHANGE;
            public const uint WM_DEVMODECHANGE = 0X001B;
            public const uint WM_ACTIVATEAPP = 0X001C;
            public const uint WM_FONTCHANGE = 0X001D;
            public const uint WM_TIMECHANGE = 0X001E;
            public const uint WM_CANCELMODE = 0X001F;
            public const uint WM_SETCURSOR = 0X0020;
            public const uint WM_MOUSEACTIVATE = 0X0021;
            public const uint WM_CHILDACTIVATE = 0X0022;
            public const uint WM_QUEUESYNC = 0X0023;
            public const uint WM_GETMINMAXINFO = 0X0024;
            public const uint WM_PAINTICON = 0X0026;
            public const uint WM_ICONERASEBKGND = 0X0027;
            public const uint WM_NEXTDLGCTL = 0X0028;
            public const uint WM_SPOOLERSTATUS = 0X002A;
            public const uint WM_DRAWITEM = 0X002B;
            public const uint WM_MEASUREITEM = 0X002C;
            public const uint WM_DELETEITEM = 0X002D;
            public const uint WM_VKEYTOITEM = 0X002E;
            public const uint WM_CHARTOITEM = 0X002F;
            public const uint WM_SETFONT = 0X0030;
            public const uint WM_GETFONT = 0X0031;
            public const uint WM_SETHOTKEY = 0X0032;
            public const uint WM_GETHOTKEY = 0X0033;
            public const uint WM_QUERYDRAGICON = 0X0037;
            public const uint WM_COMPAREITEM = 0X0039;
            public const uint WM_GETOBJECT = 0X003D;
            public const uint WM_COMPACTING = 0X0041;
            public const uint WM_COMMNOTIFY = 0X0044;
            public const uint WM_WINDOWPOSCHANGING = 0X0046;
            public const uint WM_WINDOWPOSCHANGED = 0X0047;
            public const uint WM_POWER = 0X0048;
            public const uint WM_COPYDATA = 0X004A;
            public const uint WM_CANCELJOURNAL = 0X004B;
            public const uint WM_NOTIFY = 0X004E;
            public const uint WM_INPUTLANGCHANGEREQUEST = 0X0050;
            public const uint WM_INPUTLANGCHANGE = 0X0051;
            public const uint WM_TCARD = 0X0052;
            public const uint WM_HELP = 0X0053;
            public const uint WM_USERCHANGED = 0X0054;
            public const uint WM_NOTIFYFORMAT = 0X0055;
            public const uint WM_CONTEXTMENU = 0X007B;
            public const uint WM_STYLECHANGING = 0X007C;
            public const uint WM_STYLECHANGED = 0X007D;
            public const uint WM_DISPLAYCHANGE = 0X007E;
            public const uint WM_GETICON = 0X007F;
            public const uint WM_SETICON = 0X0080;
            public const uint WM_NCCREATE = 0X0081;
            public const uint WM_NCDESTROY = 0X0082;
            public const uint WM_NCCALCSIZE = 0X008;
            public const uint WM_NCHITTEST = 0X0084;
            public const uint WM_NCPAINT = 0X0085;
            public const uint WM_NCACTIVATE = 0X0086;
            public const uint WM_GETDLGCODE = 0X0087;
            public const uint WM_SYNCPAINT = 0X0088;
            public const uint WM_NCMOUSEMOVE = 0X00A0;
            public const uint WM_NCLBUTTONDOWN = 0X00A1;
            public const uint WM_NCLBUTTONUP = 0X00A2;
            public const uint WM_NCLBUTTONDBLCLK = 0X00A3;
            public const uint WM_NCRBUTTONDOWN = 0X00A4;
            public const uint WM_NCRBUTTONUP = 0X00A5;
            public const uint WM_NCRBUTTONDBLCLK = 0X00A6;
            public const uint WM_NCMBUTTONDOWN = 0X00A7;
            public const uint WM_NCMBUTTONUP = 0X00A8;
            public const uint WM_NCMBUTTONDBLCLK = 0X00A9;
            public const uint WM_NCXBUTTONDOWN = 0X00AB;
            public const uint WM_NCXBUTTONUP = 0X00AC;
            public const uint WM_NCXBUTTONDBLCLK = 0X00AD;
            public const uint WM_INPUT = 0X00FF;
            public const uint WM_KEYDOWN = 0X0100;
            public const uint WM_KEYUP = 0X0101;
            public const uint WM_CHAR = 0X0102;
            public const uint WM_DEADCHAR = 0X0103;
            public const uint WM_SYSKEYDOWN = 0X0104;
            public const uint WM_SYSKEYUP = 0X0105;
            public const uint WM_SYSCHAR = 0X0106;
            public const uint WM_SYSDEADCHAR = 0X0107;
            public const uint WM_UNICHAR = 0X0109;
            public const uint WM_UNICODENOCHAR = 0XFFFF;
            public const uint WM_KEYLAST = 0X0108;
            public const uint WM_IME_STARTCOMPOSITION = 0X010D;
            public const uint WM_IME_ENDCOMPOSITION = 0X010E;
            public const uint WM_IME_COMPOSITION = 0X010F;
            public const uint WM_IME_KEYLAST = 0X010F;
            public const uint WM_INITDIALOG = 0X0110;
            public const uint WM_COMMAND = 0X0111;
            public const uint WM_SYSCOMMAND = 0X0112;
            public const uint WM_TIMER = 0X0113;
            public const uint WM_HSCROLL = 0X0114;
            public const uint WM_VSCROLL = 0X0115;
            public const uint WM_SCROLL = 0X3F;
            public const uint WM_INITMENU = 0X0116;
            public const uint WM_INITMENUPOPUP = 0X0117;
            public const uint WM_MENUSELECT = 0X011F;
            public const uint WM_MENUCHAR = 0X0120;
            public const uint WM_ENTERIDLE = 0X0121;
            public const uint WM_MENURBUTTONUP = 0X0122;
            public const uint WM_MENUDRAG = 0X0123;
            public const uint WM_MENUGETOBJECT = 0X0124;
            public const uint WM_UNINITMENUPOPUP = 0X0125;
            public const uint WM_MENUCOMMAND = 0X0126;
            public const uint WM_CHANGEUISTATE = 0X0127;
            public const uint WM_UPDATEUISTATE = 0X0128;
            public const uint WM_QUERYUISTATE = 0X0129;
            public const uint WM_CTLCOLORMSGBOX = 0X0132;
            public const uint WM_CTLCOLOREDIT = 0X0133;
            public const uint WM_CTLCOLORLISTBOX = 0X0134;
            public const uint WM_CTLCOLORBTN = 0X0135;
            public const uint WM_CTLCOLORDLG = 0X0136;
            public const uint WM_CTLCOLORSCROLLBAR = 0X0137;
            public const uint WM_CTLCOLORSTATIC = 0X0138;
            public const uint WM_GETHMENU = 0X01E1;
            public const uint WM_MOUSEFIRST = 0X0200;
            public const uint WM_MOUSEMOVE = 0X0200;
            public const uint WM_LBUTTONDOWN = 0X0201;
            public const uint WM_LBUTTONUP = 0X0202;
            public const uint WM_LBUTTONDBLCLK = 0X0203;
            public const uint WM_RBUTTONDOWN = 0X0204;
            public const uint WM_RBUTTONUP = 0X0205;
            public const uint WM_RBUTTONDBLCLK = 0X0206;
            public const uint WM_MBUTTONDOWN = 0X0207;
            public const uint WM_MBUTTONUP = 0X0208;
            public const uint WM_MBUTTONDBLCLK = 0X0209;
            public const uint WM_MOUSEWHEEL = 0X020A;
            public const uint WM_XBUTTONDOWN = 0X020B;
            public const uint WM_XBUTTONUP = 0X020C;
            public const uint WM_XBUTTONDBLCLK = 0X020D;
            public const uint WM_MOUSELAST = 0X0209;
            public const uint WM_PARENTNOTIFY = 0X0210;
            public const uint WM_ENTERMENULOOP = 0X0211;
            public const uint WM_EXITMENULOOP = 0X0212;
            public const uint WM_NEXTMENU = 0X0213;
            public const uint WM_SIZING = 0X0214;
            public const uint WM_CAPTURECHANGED = 0X0215;
            public const uint WM_MOVING = 0X0216;
            public const uint WM_POWERBROADCAST = 0X0218;
            public const uint WM_DEVICECHANGE = 0X0219;
            public const uint WM_MDICREATE = 0X0220;
            public const uint WM_MDIDESTROY = 0X0221;
            public const uint WM_MDIACTIVATE = 0X0222;
            public const uint WM_MDIRESTORE = 0X0223;
            public const uint WM_MDINEXT = 0X0224;
            public const uint WM_MDIMAXIMIZE = 0X0225;
            public const uint WM_MDITILE = 0X0226;
            public const uint WM_MDICASCADE = 0X0227;
            public const uint WM_MDIICONARRANGE = 0X0228;
            public const uint WM_MDIGETACTIVE = 0X0229;
            public const uint WM_MDISETMENU = 0X0230;
            public const uint WM_ENTERSIZEMOVE = 0X0231;
            public const uint WM_EXITSIZEMOVE = 0X0232;
            public const uint WM_DROPFILES = 0X0233;
            public const uint WM_MDIREFRESHMENU = 0X0234;
            public const uint WM_IME_SETCONTEXT = 0X0281;
            public const uint WM_IME_NOTIFY = 0X0282;
            public const uint WM_IME_CONTROL = 0X0283;
            public const uint WM_IME_COMPOSITIONFULL = 0X0284;
            public const uint WM_IME_SELECT = 0X0285;
            public const uint WM_IME_CHAR = 0X0286;
            public const uint WM_IME_REQUEST = 0X0288;
            public const uint WM_IME_KEYDOWN = 0X0290;
            public const uint WM_IME_KEYUP = 0X0291;
            public const uint WM_MOUSEHOVER = 0X02A1;
            public const uint WM_MOUSELEAVE = 0X02A3;
            public const uint WM_NCMOUSEHOVER = 0X02A0;
            public const uint WM_NCMOUSELEAVE = 0X02A2;
            public const uint WM_WTSSESSIONCHANGE = 0X02B1;
            public const uint WM_TABLETFIRST = 0X02C0;
            public const uint WM_TABLETLAST = 0X02DF;
            public const uint WM_CUT = 0X0300;
            public const uint WM_COPY = 0X0301;
            public const uint WM_PASTE = 0X0302;
            public const uint WM_CLEAR = 0X0303;
            public const uint WM_UNDO = 0X0304;
            public const uint WM_RENDERFORMAT = 0X0305;
            public const uint WM_RENDERALLFORMATS = 0X0306;
            public const uint WM_DESTROYCLIPBOARD = 0X0307;
            public const uint WM_DRAWCLIPBOARD = 0X0308;
            public const uint WM_PAINTCLIPBOARD = 0X0309;
            public const uint WM_VSCROLLCLIPBOARD = 0X030A;
            public const uint WM_SIZECLIPBOARD = 0X030B;
            public const uint WM_ASKCBFORMATNAME = 0X030C;
            public const uint WM_CHANGECBCHAIN = 0X030D;
            public const uint WM_HSCROLLCLIPBOARD = 0X030E;
            public const uint WM_QUERYNEWPALETTE = 0X030F;
            public const uint WM_PALETTEISCHANGING = 0X0310;
            public const uint WM_PALETTECHANGED = 0X0311;
            public const uint WM_HOTKEY = 0X0312;
            public const uint WM_PRINT = 0X0317;
            public const uint WM_PRINTCLIENT = 0X0318;
            public const uint WM_APPCOMMAND = 0X0319;
            public const uint WM_THEMECHANGED = 0X031A;
            public const uint WM_HANDHELDFIRST = 0X0358;
            public const uint WM_HANDHELDLAST = 0X035F;
            public const uint WM_AFXFIRST = 0X0360;
            public const uint WM_AFXLAST = 0X037F;
            public const uint WM_PENWINFIRST = 0X0380;
            public const uint WM_PENWINLAST = 0X038F;
            public const uint WM_APP = 0X8000;
        }
    }
}