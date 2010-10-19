namespace Microsoft.Win32
{
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;

    public class SafeEncryptedFileHandle : SafeHandle
    {
        public SafeEncryptedFileHandle() : base(IntPtr.Zero, true)
        {
        }

        [DllImport("AdvApi32.dll")]
        private static extern void CloseEncryptedFileRaw(IntPtr pvContext);
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()
        {
            CloseEncryptedFileRaw(base.handle);
            return true;
        }

        public override bool IsInvalid
        {
            get
            {
                return (base.handle == IntPtr.Zero);
            }
        }
    }
}

