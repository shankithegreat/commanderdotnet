namespace Nomad.Configuration
{
    using System;

    [Flags]
    public enum FormPlacement
    {
        All = -1,
        Location = 1,
        None = 0,
        Size = 2,
        WindowState = 4
    }
}

