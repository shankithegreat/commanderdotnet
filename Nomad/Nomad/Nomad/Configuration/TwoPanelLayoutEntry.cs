namespace Nomad.Configuration
{
    using System;

    [Flags]
    public enum TwoPanelLayoutEntry
    {
        ActivePanel = 0x10,
        All = 0x1f,
        LeftLayout = 4,
        OnePanel = 1,
        PanelsOrientation = 2,
        RightLayout = 8
    }
}

