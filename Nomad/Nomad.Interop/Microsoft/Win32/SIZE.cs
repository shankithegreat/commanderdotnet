namespace Microsoft.Win32
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public int Width;
        public int Height;
        public SIZE(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}

