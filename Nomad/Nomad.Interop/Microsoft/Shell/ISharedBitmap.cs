namespace Microsoft.Shell
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("091162a4-bc96-411f-aae8-c5122cd03363")]
    public interface ISharedBitmap
    {
        void GetSharedBitmap(out IntPtr phbm);
        void GetSize(out Size pSize);
        void GetFormat(out WTS_ALPHATYPE pat);
        void InitializeBitmap(IntPtr hbm, WTS_ALPHATYPE wtsAT);
        void Detach(out IntPtr phbm);
    }
}

