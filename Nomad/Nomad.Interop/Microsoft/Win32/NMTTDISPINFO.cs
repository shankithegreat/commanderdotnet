namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
    public struct NMTTDISPINFO
    {
        public NMHDR hdr;
        public IntPtr lpszText;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=80)]
        public string szText;
        public IntPtr hinst;
        public TTF uFlags;
        public IntPtr lParam;
        public IntPtr hbmp;
        public static string GetText(IntPtr ptr, CharSet charSet)
        {
            IntPtr ptr2 = Marshal.ReadIntPtr(ptr, Marshal.SizeOf(typeof(NMHDR)));
            if (ptr2 == IntPtr.Zero)
            {
                NMTTDISPINFO nmttdispinfo = (NMTTDISPINFO) Marshal.PtrToStructure(ptr, typeof(NMTTDISPINFO));
                return nmttdispinfo.szText;
            }
            switch (charSet)
            {
                case CharSet.None:
                case CharSet.Ansi:
                    return Marshal.PtrToStringAnsi(ptr2);

                case CharSet.Unicode:
                    return Marshal.PtrToStringUni(ptr2);
            }
            return Marshal.PtrToStringAuto(ptr2);
        }

        public static void ClearText(IntPtr ptr)
        {
            Marshal.WriteIntPtr(ptr, Marshal.SizeOf(typeof(NMHDR)), IntPtr.Zero);
            Marshal.WriteInt32(ptr, Marshal.SizeOf(typeof(NMHDR)) + IntPtr.Size, 0);
        }
    }
}

