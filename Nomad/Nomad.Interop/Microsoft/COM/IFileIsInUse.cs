namespace Microsoft.COM
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport, Guid("64a1cbf0-3a1a-4461-9158-376969693950"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFileIsInUse
    {
        void GetAppName([MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
        void GetUsage(out FILE_USAGE_TYPE pfut);
        void GetCapabilities(out uint pdwCapFlags);
        void GetSwitchToHWND(out IntPtr phwnd);
        void CloseFile();
    }
}

