namespace Microsoft.Win32.SafeHandles
{
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security;

    public sealed class SafeFindVolumeHandle : SafeHandleMinusOneIsInvalid
    {
        public SafeFindVolumeHandle() : base(true)
        {
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity, DllImport("Kernel32.dll")]
        private static extern bool FindVolumeClose(IntPtr hFindVolume);
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()
        {
            return FindVolumeClose(base.handle);
        }
    }
}

