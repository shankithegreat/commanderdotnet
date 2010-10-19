namespace Nomad.FileSystem.Property.Providers.Wdx
{
    using System;

    [Flags]
    public enum ContentFlag
    {
        contflags_edit = 1,
        contflags_fieldedit = 0x10,
        contflags_passthrough_size_float = 14,
        contflags_substattributes = 10,
        contflags_substattributestr = 12,
        contflags_substdate = 6,
        contflags_substdatetime = 4,
        contflags_substmask = 14,
        contflags_substsize = 2,
        contflags_substtime = 8
    }
}

