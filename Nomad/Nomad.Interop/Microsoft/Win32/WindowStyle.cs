namespace Microsoft.Win32
{
    using System;

    public static class WindowStyle
    {
        public const int WS_BORDER = 0x800000;
        public const int WS_CAPTION = 0xc00000;
        public const int WS_CHILD = 0x40000000;
        public const int WS_CLIPCHILDREN = 0x2000000;
        public const int WS_CLIPSIBLINGS = 0x4000000;
        public const int WS_DISABLED = 0x8000000;
        public const int WS_DLGFRAME = 0x400000;
        public const int WS_GROUP = 0x20000;
        public const int WS_HSCROLL = 0x100000;
        public const int WS_MAXIMIZE = 0x1000000;
        public const int WS_MAXIMIZEBOX = 0x10000;
        public const int WS_MINIMIZE = 0x20000000;
        public const int WS_MINIMIZEBOX = 0x20000;
        public const int WS_OVERLAPPED = 0;
        public const int WS_POPUP = -2147483648;
        public const int WS_SYSMENU = 0x80000;
        public const int WS_TABSTOP = 0x10000;
        public const int WS_THICKFRAME = 0x40000;
        public const int WS_VISIBLE = 0x10000000;
        public const int WS_VSCROLL = 0x200000;

        public static class SysLink
        {
            public const int LWS_IGNORERETURN = 2;
            public const int LWS_NOPREFIX = 4;
            public const int LWS_RIGHT = 0x20;
            public const int LWS_TRANSPARENT = 1;
            public const int LWS_USECUSTOMTEXT = 0x10;
            public const int LWS_USEVISUALSTYLE = 8;
        }
    }
}

