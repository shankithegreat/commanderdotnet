namespace Nomad.Controls.Filter
{
    using System;

    [Flags]
    public enum ViewFilters
    {
        Advanced = 0x10,
        Attributes = 8,
        Content = 4,
        ExcludeMask = 2,
        Folder = 0x20,
        IncludeMask = 1
    }
}

