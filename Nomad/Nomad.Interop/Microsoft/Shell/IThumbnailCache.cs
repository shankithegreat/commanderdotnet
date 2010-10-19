namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("F676C15D-596A-4ce2-8234-33996F445DB1"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IThumbnailCache
    {
        void GetThumbnail(IShellItem pShellItem, uint cxyRequestedThumbSize, WTS_FLAGS flags, out ISharedBitmap ppvThumb, ref WTS_CACHEFLAGS pOutFlags, IntPtr pThumbnailID);
        void GetThumbnailByID(IntPtr thumbnailID, uint cxyRequestedThumbSize, ref ISharedBitmap ppvThumb, out WTS_CACHEFLAGS pOutFlags);
    }
}

