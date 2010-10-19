namespace Microsoft.Win32
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public static class CommCtrl
    {
        public const int BCM_FIRST = 0x1600;
        public const int BCM_SETSHIELD = 0x160c;
        public const int HDM_DELETEITEM = 0x1202;
        public const int HDM_FIRST = 0x1200;
        public const int HDM_GETITEMA = 0x1203;
        public const int HDM_GETITEMCOUNT = 0x1200;
        public const int HDM_GETITEMRECT = 0x1207;
        public const int HDM_GETITEMW = 0x120b;
        public const int HDM_HITTEST = 0x1206;
        public const int HDM_INSERTITEMA = 0x1201;
        public const int HDM_INSERTITEMW = 0x120a;
        public const int HDM_SETIMAGELIST = 0x1208;
        public const int HDM_SETITEMA = 0x1204;
        public const int HDM_SETITEMW = 0x120c;
        public const int HDN_BEGINTRACKA = -306;
        public const int HDN_BEGINTRACKW = -326;
        public const int HDN_DIVIDERDBLCLICKA = -305;
        public const int HDN_DIVIDERDBLCLICKW = -325;
        public const int HDN_FIRST = -300;
        public const int I_CHILDRENCALLBACK = -1;
        public const int I_IMAGECALLBACK = -1;
        public static readonly IntPtr LPSTR_TEXTCALLBACK = ((IntPtr) (-1));
        public static readonly IntPtr LPSTR_TEXTCALLBACKA = ((IntPtr) (-1));
        public static readonly IntPtr LPSTR_TEXTCALLBACKW = ((IntPtr) (-1));
        public const int LVM_CREATEDRAGIMAGE = 0x1021;
        public const int LVM_ENSUREVISIBLE = 0x1013;
        public const int LVM_FIRST = 0x1000;
        public const int LVM_GETCALLBACKMASK = 0x100a;
        public const int LVM_GETCOLUMNORDERARRAY = 0x103b;
        public const int LVM_GETCOLUMNWIDTH = 0x101d;
        public const int LVM_GETCOUNTPERPAGE = 0x1028;
        public const int LVM_GETEDITCONTROL = 0x1018;
        public const int LVM_GETGROUPINFO = 0x1095;
        public const int LVM_GETHEADER = 0x101f;
        public const int LVM_GETITEMRECT = 0x100e;
        public const int LVM_GETITEMSPACING = 0x1033;
        public const int LVM_GETITEMSTATE = 0x102c;
        public const int LVM_GETNEXTITEM = 0x100c;
        public const int LVM_GETTOOLTIPS = 0x104e;
        public const int LVM_GETTOPINDEX = 0x1027;
        public const int LVM_HITTEST = 0x1012;
        public const int LVM_INSERTCOLUMNA = 0x101b;
        public const int LVM_INSERTCOLUMNW = 0x1061;
        public const int LVM_INSERTGROUP = 0x1091;
        public const int LVM_SETCALLBACKMASK = 0x100b;
        public const int LVM_SETCOLUMNWIDTH = 0x101e;
        public const int LVM_SETEXTENDEDLISTVIEWSTYLE = 0x1036;
        public const int LVM_SETGROUPINFO = 0x1093;
        public const int LVM_SETICONSPACING = 0x1035;
        public const int LVM_SETITEMCOUNT = 0x102f;
        public const int LVM_SETITEMSTATE = 0x102b;
        public const int LVM_SETVIEW = 0x108e;
        public const int LVM_SUBITEMHITTEST = 0x1039;
        public const int LVN_FIRST = -100;
        public const int LVN_GETDISPINFOA = -150;
        public const int LVN_GETDISPINFOW = -177;
        public const int LVN_GETINFOTIPA = -157;
        public const int LVN_GETINFOTIPW = -158;
        public const int LVN_MARQUEEBEGIN = -156;
        public const int LVN_SETDISPINFOA = -151;
        public const int LVN_SETDISPINFOW = -178;
        public const int NM_CLICK = -2;
        public const int NM_CUSTOMDRAW = -12;
        public const int NM_DBLCLK = -3;
        public const int NM_FIRST = 0;
        public const int NM_HOVER = -13;
        public const int NM_RCLICK = -5;
        public const int NM_RDBLCLK = -6;
        public const int NM_RELEASEDCAPTURE = -16;
        public const int TTM_ACTIVATE = 0x401;
        public static readonly int TTM_ADDTOOL = (OS.IsWinNT ? 0x432 : 0x404);
        public const int TTM_ADDTOOLA = 0x404;
        public const int TTM_ADDTOOLW = 0x432;
        public static readonly int TTM_DELTOOL = (OS.IsWinNT ? 0x433 : 0x405);
        public const int TTM_DELTOOLA = 0x405;
        public const int TTM_DELTOOLW = 0x433;
        public const int TTM_GETDELAYTIME = 0x415;
        public static readonly int TTM_NEWTOOLRECT = (OS.IsWinNT ? 0x434 : 0x406);
        public const int TTM_NEWTOOLRECTA = 0x406;
        public const int TTM_NEWTOOLRECTW = 0x434;
        public const int TTM_POP = 0x41c;
        public const int TTM_POPUP = 0x422;
        public const int TTM_RELAYEVENT = 0x407;
        public const int TTM_SETDELAYTIME = 0x403;
        public const int TTM_TRACKACTIVATE = 0x411;
        public const int TTM_WINDOWFROMPOINT = 0x410;
        public const int TTN_FIRST = -520;
        public const int TTN_GETDISPINFOA = -520;
        public const int TTN_GETDISPINFOW = -530;
        public const int TTN_LINKCLICK = -523;
        public const int TTN_NEEDTEXTA = -520;
        public const int TTN_NEEDTEXTW = -530;
        public const int TTN_POP = -522;
        public const int TTN_SHOW = -521;
        public const int TV_FIRST = 0x1100;
        public const int TVM_CREATEDRAGIMAGE = 0x1112;
        public const int TVM_GETCOUNT = 0x1105;
        public const int TVM_GETEXTENDEDSTYLE = 0x112d;
        public const int TVM_GETINDENT = 0x1106;
        public const int TVM_GETITEMA = 0x110c;
        public const int TVM_GETITEMSTATE = 0x1127;
        public const int TVM_GETITEMW = 0x113e;
        public const int TVM_GETNEXTITEM = 0x110a;
        public const int TVM_GETUNICODEFORMAT = 0x2006;
        public const int TVM_INSERTITEMA = 0x1100;
        public const int TVM_INSERTITEMW = 0x1132;
        public const int TVM_SELECTITEM = 0x110b;
        public const int TVM_SETBKCOLOR = 0x111d;
        public const int TVM_SETEXTENDEDSTYLE = 0x112c;
        public const int TVM_SETINDENT = 0x1107;
        public const int TVM_SETITEMA = 0x110d;
        public const int TVM_SETITEMW = 0x113f;
        public const int TVN_FIRST = -400;
        public const int TVN_GETDISPINFOA = -403;
        public const int TVN_GETDISPINFOW = -452;

        public static int Button_SetElevationRequiredState(IntPtr hwnd, bool fRequired)
        {
            return (int) Windows.SendMessage(hwnd, 0x160c, IntPtr.Zero, fRequired ? ((IntPtr) (-1)) : IntPtr.Zero);
        }

        public static int Header_GetColumnAt(IntPtr hWndControl, Point pt)
        {
            int num;
            HDHITTESTINFO hdhittestinfo = new HDHITTESTINFO {
                pt = pt
            };
            GCHandle handle = GCHandle.Alloc(hdhittestinfo, GCHandleType.Pinned);
            try
            {
                num = (int) Windows.SendMessage(hWndControl, 0x1206, IntPtr.Zero, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
            return num;
        }

        public static int Header_GetItemCount(IntPtr hwndHD)
        {
            return (int) Windows.SendMessage(hwndHD, 0x1200, IntPtr.Zero, IntPtr.Zero);
        }

        private static int HitTest(IntPtr hwnd, int msg, ref LVHITTESTINFO pinfo)
        {
            return (int) SendMessage(hwnd, msg, IntPtr.Zero, ref pinfo);
        }

        public static IntPtr ListView_CreateDragImage(IntPtr hwnd, int iItem, out Point lpptUpLeft)
        {
            IntPtr ptr3;
            IntPtr lParam = Marshal.AllocHGlobal(8);
            try
            {
                IntPtr ptr2 = Windows.SendMessage(hwnd, 0x1021, (IntPtr) iItem, lParam);
                lpptUpLeft = (Point) Marshal.PtrToStructure(lParam, typeof(Point));
                ptr3 = ptr2;
            }
            finally
            {
                Marshal.FreeHGlobal(lParam);
            }
            return ptr3;
        }

        public static LVIS ListView_GetCallbackMask(IntPtr hwnd)
        {
            return (LVIS) ((int) Windows.SendMessage(hwnd, 0x100a, IntPtr.Zero, IntPtr.Zero));
        }

        public static int ListView_GetCountPerPage(IntPtr hwnd)
        {
            return (int) Windows.SendMessage(hwnd, 0x1028, IntPtr.Zero, IntPtr.Zero);
        }

        public static IntPtr ListView_GetEditControl(IntPtr hwnd)
        {
            return Windows.SendMessage(hwnd, 0x1018, IntPtr.Zero, IntPtr.Zero);
        }

        public static bool ListView_GetItemRect(IntPtr hwnd, int i, int code, out Microsoft.Win32.RECT prc)
        {
            prc = new Microsoft.Win32.RECT();
            prc.Left = code;
            return (((int) SendMessage(hwnd, 0x100e, (IntPtr) i, ref prc)) != 0);
        }

        public static Size ListView_GetItemSpacing(IntPtr hwnd, LV_VIEW view)
        {
            int num;
            switch (view)
            {
                case LV_VIEW.LV_VIEW_ICON:
                    num = (int) Windows.SendMessage(hwnd, 0x1033, IntPtr.Zero, IntPtr.Zero);
                    break;

                case LV_VIEW.LV_VIEW_SMALLICON:
                    num = (int) Windows.SendMessage(hwnd, 0x1033, (IntPtr) 1, IntPtr.Zero);
                    break;

                default:
                    throw new ArgumentException();
            }
            return new Size(num & 0xffff, num >> 0x10);
        }

        public static LVIS ListView_GetItemState(IntPtr hwnd, int i, LVIS mask)
        {
            return (LVIS) ((int) Windows.SendMessage(hwnd, 0x102c, (IntPtr) i, (IntPtr) ((long) mask)));
        }

        public static int ListView_GetNextItem(IntPtr hwnd, int i, LVNI flags)
        {
            return (int) Windows.SendMessage(hwnd, 0x100c, (IntPtr) i, (IntPtr) ((long) flags));
        }

        public static IntPtr ListView_GetToolTips(IntPtr hwnd)
        {
            return Windows.SendMessage(hwnd, 0x104e, IntPtr.Zero, IntPtr.Zero);
        }

        public static int ListView_HitTest(IntPtr hwnd, ref LVHITTESTINFO pinfo)
        {
            return HitTest(hwnd, 0x1012, ref pinfo);
        }

        public static void ListView_SetCallbackMask(IntPtr hwnd, LVIS mask)
        {
            Windows.SendMessage(hwnd, 0x100b, (IntPtr) ((long) mask), IntPtr.Zero);
        }

        public static void ListView_SetExtendedListViewStyleEx(IntPtr hwndLV, LVS_EX dwExMask, LVS_EX dwExStyle)
        {
            Windows.SendMessage(hwndLV, 0x1036, (IntPtr) ((ulong) dwExMask), (IntPtr) ((ulong) dwExStyle));
        }

        public static void ListView_SetIconSpacing(IntPtr hwndLV, int cx, int cy)
        {
            Windows.SendMessage(hwndLV, 0x1035, IntPtr.Zero, (IntPtr) (((cy & 0xffff) << 0x10) | (cx & 0xffff)));
        }

        public static void ListView_SetItemCountEx(IntPtr hwndLV, int cItems, lv_sti_flags dwFlags)
        {
            Windows.SendMessage(hwndLV, 0x102f, (IntPtr) cItems, (IntPtr) ((long) dwFlags));
        }

        public static void ListView_SetView(IntPtr hwndLV, LV_VIEW iView)
        {
            Windows.SendMessage(hwndLV, 0x108e, (IntPtr) ((ulong) iView), IntPtr.Zero);
        }

        public static int ListView_SubItemHitTest(IntPtr hwnd, Point pt)
        {
            LVHITTESTINFO pinfo = new LVHITTESTINFO {
                pt = pt
            };
            if (HitTest(hwnd, 0x1039, ref pinfo) < 0)
            {
                return -1;
            }
            return pinfo.iSubItem;
        }

        public static int ListView_SubItemHitTest(IntPtr hwnd, ref LVHITTESTINFO pinfo)
        {
            return HitTest(hwnd, 0x1039, ref pinfo);
        }

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref LVHITTESTINFO lParam);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref Microsoft.Win32.RECT lParam);
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref TVITEM lParam);
        public static IntPtr TreeView_CreateDragImage(IntPtr hwndTV, IntPtr hitem)
        {
            return Windows.SendMessage(hwndTV, 0x1112, IntPtr.Zero, hitem);
        }

        public static void TreeView_GetItem(IntPtr hwndTV, ref TVITEM pitem)
        {
            SendMessage(hwndTV, TreeView_GetUnicodeFormat(hwndTV) ? 0x113e : 0x110c, IntPtr.Zero, ref pitem);
        }

        public static TVIS TreeView_GetItemState(IntPtr hwndTV, IntPtr hItem, TVIS stateMask)
        {
            return (TVIS) ((int) Windows.SendMessage(hwndTV, 0x102c, hItem, (IntPtr) ((ulong) stateMask)));
        }

        public static bool TreeView_GetUnicodeFormat(IntPtr hwnd)
        {
            return Convert.ToBoolean((int) Windows.SendMessage(hwnd, 0x2006, IntPtr.Zero, IntPtr.Zero));
        }

        public static int TreeView_SetExtendedStyle(IntPtr hwnd, TVS_EX dw, TVS_EX mask)
        {
            return (int) Windows.SendMessage(hwnd, 0x112c, (IntPtr) ((long) mask), (IntPtr) ((long) dw));
        }

        public static void TreeView_SetItem(IntPtr hwndTV, ref TVITEM pitem)
        {
            SendMessage(hwndTV, TreeView_GetUnicodeFormat(hwndTV) ? 0x113f : 0x110d, IntPtr.Zero, ref pitem);
        }

        [Flags]
        public enum lv_sti_flags
        {
            LVSICF_NOINVALIDATEALL = 1,
            LVSICF_NOSCROLL = 2
        }
    }
}

