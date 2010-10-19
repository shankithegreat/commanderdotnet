namespace Microsoft.Win32.SafeHandles
{
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security;

    public sealed class SafeDeviceNotificationHandle : SafeHandle
    {
        public SafeDeviceNotificationHandle() : base(IntPtr.Zero, true)
        {
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()
        {
            return UnregisterDeviceNotification(base.handle);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity, DllImport("user32.dll")]
        private static extern bool UnregisterDeviceNotification(IntPtr Handle);

        public override bool IsInvalid
        {
            get
            {
                return (base.handle == IntPtr.Zero);
            }
        }
    }
}

