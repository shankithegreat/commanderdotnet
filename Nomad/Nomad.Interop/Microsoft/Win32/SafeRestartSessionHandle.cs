namespace Microsoft.Win32
{
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security;

    public sealed class SafeRestartSessionHandle : SafeHandle
    {
        public SafeRestartSessionHandle() : base(IntPtr.Zero, true)
        {
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()
        {
            return (RmEndSession(base.handle) == 0);
        }

        [SuppressUnmanagedCodeSecurity, DllImport("rstrtmgr.dll")]
        public static extern int RmEndSession(IntPtr pSessionHandle);

        public override bool IsInvalid
        {
            get
            {
                return (base.handle == IntPtr.Zero);
            }
        }
    }
}

