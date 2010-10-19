namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct NMLVGETINFOTIP
    {
        public NMHDR hdr;
        public LVGIT dwFlags;
        public IntPtr pszText;
        public int cchTextMax;
        public int iItem;
        public int iSubItem;
        public IntPtr lParam;
        public static unsafe int GetItem(IntPtr ptr)
        {
            return ((void*) ptr).iItem;
        }

        public static unsafe string GetText(IntPtr ptr, CharSet charSet)
        {
            switch (charSet)
            {
                case CharSet.None:
                case CharSet.Ansi:
                    return Marshal.PtrToStringAnsi(((void*) ptr).pszText);

                case CharSet.Unicode:
                    return Marshal.PtrToStringUni(((void*) ptr).pszText);
            }
            return Marshal.PtrToStringAuto(((void*) ptr).pszText);
        }

        public static unsafe void ClearText(IntPtr ptr)
        {
            Marshal.WriteInt16(((void*) ptr).pszText, (short) 0);
        }
    }
}

