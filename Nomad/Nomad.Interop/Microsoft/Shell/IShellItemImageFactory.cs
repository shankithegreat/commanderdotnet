namespace Microsoft.Shell
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [ComImport, Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItemImageFactory
    {
        void GetImage(Size size, SIIGBF flags, out IntPtr phbm);
    }
}

