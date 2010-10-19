namespace Microsoft.Shell
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DLLVERSIONINFO
    {
        public uint cbSize;
        public uint dwMajorVersion;
        public uint dwMinorVersion;
        public uint dwBuildNumber;
        public DLLVER_PLATFORM dwPlatformID;
        public System.Version Version
        {
            get
            {
                return new System.Version((int) this.dwMajorVersion, (int) this.dwMinorVersion, (int) this.dwBuildNumber);
            }
        }
    }
}

