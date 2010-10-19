namespace Microsoft.IO
{
    using System;

    [Flags]
    public enum DeviceCapabilities
    {
        CommandQueueing = 2,
        DeviceHotPlug = 1,
        MediaHotPlug = 4,
        MediaRemovable = 8
    }
}

