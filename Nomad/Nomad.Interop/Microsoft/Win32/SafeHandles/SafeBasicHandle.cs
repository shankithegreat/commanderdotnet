namespace Microsoft.Win32.SafeHandles
{
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security;

    public class SafeBasicHandle : SafeHandleMinusOneIsInvalid
    {
        protected SafeBasicHandle(bool ownsHandle) : base(ownsHandle)
        {
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity, DllImport("Kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()
        {
            return CloseHandle(base.handle);
        }
    }
}

