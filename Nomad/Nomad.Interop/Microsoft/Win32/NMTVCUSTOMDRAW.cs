namespace Microsoft.Win32
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct NMTVCUSTOMDRAW
    {
        public NMCUSTOMDRAW nmcd;
        public int clrText;
        public int clrTextBk;
        public int iLevel;
        public static unsafe System.Drawing.Color GetBackColor(IntPtr ptr)
        {
            return ColorTranslator.FromWin32(((void*) ptr).clrTextBk);
        }

        public static unsafe System.Drawing.Color GetForeColor(IntPtr ptr)
        {
            return ColorTranslator.FromWin32(((void*) ptr).clrText);
        }

        public static unsafe void SetBackColor(IntPtr ptr, System.Drawing.Color value)
        {
            ((void*) ptr).clrTextBk = ColorTranslator.ToWin32(value);
        }

        public static unsafe void SetForeColor(IntPtr ptr, System.Drawing.Color value)
        {
            ((void*) ptr).clrText = ColorTranslator.ToWin32(value);
        }
    }
}

