namespace Microsoft.Win32
{
    using System;

    public sealed class Macros
    {
        public static int GET_X_LPARAM(IntPtr lParam)
        {
            return (((int) lParam) & 0xffff);
        }

        public static int GET_Y_LPARAM(IntPtr lParam)
        {
            return (((int) lParam) >> 0x10);
        }

        public static bool IS_INTRESOURCE(IntPtr value)
        {
            return (value.ToInt64() < 0xffffL);
        }
    }
}

