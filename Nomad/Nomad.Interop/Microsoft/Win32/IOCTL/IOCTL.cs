namespace Microsoft.Win32.IOCTL
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    public static class IOCTL
    {
        public const int ERROR_INVALID_REPARSE_DATA = 0x1128;
        public const int ERROR_NOT_A_REPARSE_POINT = 0x1126;
        public const int ERROR_REPARSE_ATTRIBUTE_CONFLICT = 0x1127;
        public const int ERROR_REPARSE_TAG_INVALID = 0x1129;
        public const int ERROR_REPARSE_TAG_MISMATCH = 0x112a;
        public const uint IO_REPARSE_TAG_MOUNT_POINT = 0xa0000003;
        public const uint IO_REPARSE_TAG_SYMLINK = 0xa000000c;
        public const uint IOCTL_STORAGE_BASE = 0x2d;
        public const uint IOCTL_VOLUME_BASE = 0x56;

        public static uint CTL_CODE(DEVICE_TYPE deviceType, ushort function, METHOD method, FILE_ACCESS access)
        {
            return (uint) ((((((uint) deviceType) << 0x10) | (((byte) access) << 14)) | (function << 2)) | ((long) method));
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", SetLastError=true)]
        public static extern bool DeviceIoControl(SafeFileHandle hDevice, FSCTL dwIoControlCode, IntPtr lpInBuffer, int nInBufferSize, IntPtr lpOutBuffer, int nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);
    }
}

