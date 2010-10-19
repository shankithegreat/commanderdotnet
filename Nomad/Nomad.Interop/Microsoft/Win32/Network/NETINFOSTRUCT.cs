namespace Microsoft.Win32.Network
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct NETINFOSTRUCT
    {
        public uint cbStructure;
        public uint dwProviderVersion;
        public uint dwStatus;
        public uint dwCharacteristics;
        public IntPtr dwHandle;
        public ushort wNetType;
        public uint dwPrinters;
        public uint dwDrives;
    }
}

