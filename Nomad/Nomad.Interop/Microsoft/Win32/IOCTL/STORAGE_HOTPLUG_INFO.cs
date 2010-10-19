namespace Microsoft.Win32.IOCTL
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct STORAGE_HOTPLUG_INFO
    {
        public uint Size;
        [MarshalAs(UnmanagedType.I1)]
        public bool MediaRemovable;
        [MarshalAs(UnmanagedType.I1)]
        public bool MediaHotplug;
        [MarshalAs(UnmanagedType.I1)]
        public bool DeviceHotplug;
        [MarshalAs(UnmanagedType.I1)]
        public bool WriteCacheEnableOverride;
    }
}

