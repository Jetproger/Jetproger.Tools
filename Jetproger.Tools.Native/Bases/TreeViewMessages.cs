namespace Tools
{
    public static unsafe partial class Native
    {
        public static class TreeViewMessages
        {
            public const int TV_FIRST = 0x1100;
            public const int TVM_INSERTITEMA = TV_FIRST + 0;
            public const int TVM_INSERTITEMW = TV_FIRST + 50;
            public const int TVM_INSERTITEM = TVM_INSERTITEMW;
            public const int TVM_DELETEITEM = TV_FIRST + 1;
            public const int TVM_EXPAND = TV_FIRST + 2;
            public const int TVM_GETITEMRECT = TV_FIRST + 4;
            public const int TVM_GETCOUNT = TV_FIRST + 5;
            public const int TVM_GETINDENT = TV_FIRST + 6;
            public const int TVM_SETINDENT = TV_FIRST + 7;
            public const int TVM_GETIMAGELIST = TV_FIRST + 8;
            public const int TVM_SETIMAGELIST = TV_FIRST + 9;
            public const int TVM_GETNEXTITEM = TV_FIRST + 10;
            public const int TVM_SELECTITEM = TV_FIRST + 11;
            public const int TVIF_TEXT = 0x0001;
            public const int TVIF_IMAGE = 0x0002;
            public const int TVIF_PARAM = 0x0004;
            public const int TVIF_STATE = 0x0008;
            public const int TVIF_HANDLE = 0x0010;
            public const int TVIF_SELECTEDIMAGE = 0x0020;
            public const int TVIF_CHILDREN = 0x0040;
            public const int TVIF_INTEGRAL = 0x0080;
            public const int TVIF_STATEEX = 0x0100;
            public const int TVIF_EXPANDEDIMAGE = 0x0200;
            public const int TVIS_SELECTED = 0x0002;
            public const int TVIS_CUT = 0x0004;
            public const int TVIS_DROPHILITED = 0x0008;
            public const int TVIS_BOLD = 0x0010;
            public const int TVIS_EXPANDED = 0x0020;
            public const int TVIS_EXPANDEDONCE = 0x0040;
            public const int TVIS_EXPANDPARTIAL = 0x0080;
            public const int TVIS_OVERLAYMASK = 0x0F00;
            public const int TVIS_STATEIMAGEMASK = 0xF000;
            public const int TVIS_USERMASK = 0xF000;
            public const int TVIS_EX_FLAT = 0x0001;
            public const int TVIS_EX_DISABLED = 0x0002;
            public const int TVIS_EX_ALL = 0x0002;
            public const int I_CHILDRENCALLBACK = -1;
            public const int I_CHILDRENAUTO = -2;
        }
    }
}