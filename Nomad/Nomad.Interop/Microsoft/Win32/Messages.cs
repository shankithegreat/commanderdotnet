namespace Microsoft.Win32
{
    using System;

    public static class Messages
    {
        public const int EM_GETSEL = 0xb0;
        public const int EM_LIMITTEXT = 0xc5;
        public const int EM_SETLIMITTEXT = 0xc5;
        public const int EM_SETSEL = 0xb1;
        public const int WM_ACTIVATE = 6;
        public const int WM_ACTIVATEAPP = 0x1c;
        public const int WM_APPCOMMAND = 0x319;
        public const int WM_CLEAR = 0x303;
        public const int WM_CLIPBOARDUPDATE = 0x31d;
        public const int WM_COPY = 0x301;
        public const int WM_CREATE = 1;
        public const int WM_CUT = 0x300;
        public const int WM_DESTROY = 2;
        public const int WM_DEVICECHANGE = 0x219;
        public const int WM_DRAWITEM = 0x2b;
        public const int WM_ENDSESSION = 0x16;
        public const int WM_ERASEBKGND = 20;
        public const int WM_GETDLGCODE = 0x87;
        public const int WM_HOTKEY = 0x312;
        public const int WM_INITMENUPOPUP = 0x117;
        public const int WM_INPUTLANGCHANGE = 0x51;
        public const int WM_INPUTLANGCHANGEREQUEST = 80;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_MBUTTONDBLCLK = 0x209;
        public const int WM_MBUTTONDOWN = 0x207;
        public const int WM_MBUTTONUP = 520;
        public const int WM_MEASUREITEM = 0x2c;
        public const int WM_MOUSEACTIVATE = 0x21;
        public const int WM_MOUSEFIRST = 0x200;
        public const int WM_MOUSEHOVER = 0x2a1;
        public const int WM_MOUSEHWHEEL = 0x20e;
        public const int WM_MOUSELEAVE = 0x2a3;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_MOUSEWHEEL = 0x20a;
        public const int WM_MOVE = 3;
        public const int WM_NCHITTEST = 0x84;
        public const int WM_NCMOUSEHOVER = 0x2a0;
        public const int WM_NOTIFY = 0x4e;
        public const int WM_NULL = 0;
        public const int WM_PAINT = 15;
        public const int WM_PASTE = 770;
        public const int WM_PRINT = 0x317;
        public const int WM_PRINTCLIENT = 0x318;
        public const int WM_QUERYENDSESSION = 0x11;
        public const int WM_RBUTTONDBLCLK = 0x206;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_REFLECT = 0x2000;
        public const int WM_SETCURSOR = 0x20;
        public const int WM_SETREDRAW = 11;
        public const int WM_SETTEXT = 12;
        public const int WM_SHOWWINDOW = 0x18;
        public const int WM_SIZE = 5;
        public const int WM_SYSCOLORCHANGE = 0x15;
        public const int WM_SYSCOMMAND = 0x112;
        public const int WM_THEMECHANGED = 0x31a;
        public const int WM_TIMER = 0x113;
        public const int WM_UNDO = 0x304;
        public const int WM_USER = 0x400;
        public const int WM_WINDOWPOSCHANGED = 0x47;
        public const int WM_WINDOWPOSCHANGING = 70;
        public const int WM_XBUTTONDBLCLK = 0x20d;
        public const int WM_XBUTTONDOWN = 0x20b;
        public const int WM_XBUTTONUP = 0x20c;

        public static class Common
        {
            public const int CCM_FIRST = 0x2000;
            public const int CCM_GETCOLORSCHEME = 0x2003;
            public const int CCM_GETDROPTARGET = 0x2004;
            public const int CCM_GETUNICODEFORMAT = 0x2006;
            public const int CCM_LAST = 0x2200;
            public const int CCM_SETBKCOLOR = 0x2001;
            public const int CCM_SETCOLORSCHEME = 0x2002;
            public const int CCM_SETUNICODEFORMAT = 0x2005;
        }

        public static class Edit
        {
            public const int EM_GETMARGINS = 0xd4;
            public const int EM_SETMARGINS = 0xd3;
        }
    }
}

