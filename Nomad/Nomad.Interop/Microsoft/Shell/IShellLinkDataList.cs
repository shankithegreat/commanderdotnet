namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("45E2B4AE-B1C3-11D0-B92F-00A0C90312E1"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellLinkDataList
    {
        void AddDataBlock(IntPtr pDataBlock);
        [PreserveSig]
        int CopyDataBlock(uint dwSig, out IntPtr ppDataBlock);
        void RemoveDataBlock(uint dwSig);
        SHELL_LINK_DATA_FLAGS GetFlags();
        void SetFlags(SHELL_LINK_DATA_FLAGS dwFlags);
    }
}

