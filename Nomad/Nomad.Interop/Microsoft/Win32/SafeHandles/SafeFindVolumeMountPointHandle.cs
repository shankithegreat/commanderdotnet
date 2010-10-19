namespace Microsoft.Win32.SafeHandles
{
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security;

    public sealed class SafeFindVolumeMountPointHandle : SafeHandleMinusOneIsInvalid
    {
        public SafeFindVolumeMountPointHandle() : base(true)
        {
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity, DllImport("Kernel32.dll")]
        private static extern bool FindVolumeMountPointClose(IntPtr hFindVolumeMountPoint);
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()
        {
            return FindVolumeMountPointClose(base.handle);
        }
    }
}

