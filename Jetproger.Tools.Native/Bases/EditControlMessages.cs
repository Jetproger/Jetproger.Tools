namespace Tools
{
    public static unsafe partial class Native
    {
        public static class EditControlMessages
        {
            public const uint EM_GETSEL = 0x00B0;
            public const uint EM_SETSEL = 0x00B1;
            public const uint EM_GETRECT = 0x00B2;
            public const uint EM_SETRECT = 0x00B3;
            public const uint EM_SETRECTNP = 0x00B4;
            public const uint EM_SCROLL = 0x00B5;
            public const uint EM_LINESCROLL = 0x00B6;
            public const uint EM_SCROLLCARET = 0x00B7;
            public const uint EM_GETMODIFY = 0x00B8;
            public const uint EM_SETMODIFY = 0x00B9;
            public const uint EM_GETLINECOUNT = 0x00BA;
            public const uint EM_LINEINDEX = 0x00BB;
            public const uint EM_SETHANDLE = 0x00BC;
            public const uint EM_GETHANDLE = 0x00BD;
            public const uint EM_GETTHUMB = 0x00BE;
            public const uint EM_LINELENGTH = 0x00C1;
            public const uint EM_REPLACESEL = 0x00C2;
            public const uint EM_GETLINE = 0x00C4;
            public const uint EM_LIMITTEXT = 0x00C5;
            public const uint EM_CANUNDO = 0x00C6;
            public const uint EM_UNDO = 0x00C7;
            public const uint EM_FMTLINES = 0x00C8;
            public const uint EM_LINEFROMCHAR = 0x00C9;
            public const uint EM_SETTABSTOPS  = 0x00CB;
            public const uint EM_SETPASSWORDCHAR = 0x00CC;
            public const uint EM_EMPTYUNDOBUFFER = 0x00CD;
            public const uint EM_GETFIRSTVISIBLELINE = 0x00CE;
            public const uint EM_SETREADONLY = 0x00CF;
            public const uint EM_SETWORDBREAKPROC = 0x00D0;
            public const uint EM_GETWORDBREAKPROC = 0x00D1;
            public const uint EM_GETPASSWORDCHAR  = 0x00D2;
            public const uint EM_SETMARGINS = 0x00D3;
            public const uint EM_GETMARGINS = 0x00D4;
            public const uint EM_SETLIMITTEXT = EM_LIMITTEXT;
            public const uint EM_GETLIMITTEXT = 0x00D5;
            public const uint EM_POSFROMCHAR = 0x00D6;
            public const uint EM_CHARFROMPOS = 0x00D7;
            public const uint EM_SETIMESTATUS = 0x00D8;
            public const uint EM_GETIMESTATUS = 0x00D9;
        }
    }
}