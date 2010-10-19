namespace Microsoft.Win32.SafeHandles
{
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security;

    public sealed class SafeNetEnumHandle : SafeHandleMinusOneIsInvalid
    {
        public SafeNetEnumHandle() : base(true)
        {
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()
        {
            return (WNetCloseEnum(base.handle) == 0);
        }

        [SuppressUnmanagedCodeSecurity, DllImport("mpr.dll")]
        private static extern uint WNetCloseEnum(IntPtr hEnum);
    }
}

