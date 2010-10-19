namespace Nomad
{
    using System;

    [Flags]
    internal enum SafeMode
    {
        All = 0xff,
        DefaultSkip = 0x1f,
        DisableIcons = 0x80,
        SkipConfig = 1,
        SkipFormPlacement = 2,
        SkipIconCache = 8,
        SkipImageProvider = 0x10,
        SkipTabs = 4,
        SkipUICulture = 0x20,
        UseShellImageProvider = 0x40
    }
}

