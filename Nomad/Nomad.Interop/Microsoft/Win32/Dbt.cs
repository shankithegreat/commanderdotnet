namespace Microsoft.Win32
{
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Runtime.InteropServices;

    public static class Dbt
    {
        public const int BROADCAST_QUERY_DENY = 0x424d5144;
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;
        public const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const int DBT_DEVICEREMOVEPENDING = 0x8003;
        public const int DBT_DEVICETYPESPECIFIC = 0x8005;

        [DllImport("user32.dll", SetLastError=true)]
        public static extern SafeDeviceNotificationHandle RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, DEVICE_NOTIFY Flags);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern SafeDeviceNotificationHandle RegisterDeviceNotification(IntPtr hRecipient, [In] ref _DEV_BROADCAST_DEVICEINTERFACE NotificationFilter, DEVICE_NOTIFY Flags);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern SafeDeviceNotificationHandle RegisterDeviceNotification(IntPtr hRecipient, [In] ref _DEV_BROADCAST_HANDLE NotificationFilter, DEVICE_NOTIFY Flags);
    }
}

