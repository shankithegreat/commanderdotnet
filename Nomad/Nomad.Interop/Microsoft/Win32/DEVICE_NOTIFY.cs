namespace Microsoft.Win32
{
    using System;

    [Flags]
    public enum DEVICE_NOTIFY
    {
        DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4,
        DEVICE_NOTIFY_SERVICE_HANDLE = 1,
        DEVICE_NOTIFY_WINDOW_HANDLE = 0
    }
}

