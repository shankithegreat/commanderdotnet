namespace Nomad
{
    using System;

    [Flags]
    public enum QuickFindOptions
    {
        AlwaysShowFolders = 4,
        AutoHide = 0x10,
        ExecuteOnEnter = 8,
        PrefixSearch = 1,
        QuickFilter = 2
    }
}

