namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
    public struct MENUITEMINFO
    {
        public uint cbSize;
        public MIIM fMask;
        public MFT fType;
        public MFS fState;
        public uint wID;
        public IntPtr hSubMenu;
        public IntPtr hbmpChecked;
        public IntPtr hbmpUnchecked;
        public IntPtr dwItemData;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string dwTypeData;
        public uint cch;
        public IntPtr hbmpItem;
        public static uint SizeOf
        {
            get
            {
                return (uint) Marshal.SizeOf(typeof(MENUITEMINFO));
            }
        }
    }
}

