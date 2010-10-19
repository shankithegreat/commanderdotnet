namespace Microsoft.Win32.SafeHandles
{
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security;

    public sealed class SafeFindHandle : SafeHandleMinusOneIsInvalid
    {
        public SafeFindHandle() : base(true)
        {
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity, DllImport("Kernel32.dll")]
        private static extern bool FindClose(IntPtr hFindFile);
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()
        {
            return FindClose(base.handle);
        }
    }
}

