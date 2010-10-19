namespace Nomad.FileSystem.Property
{
    using System;

    [Flags]
    public enum VirtualPropertyOption
    {
        Hidden = 1,
        OnDemand = 4,
        Slow = 2
    }
}

