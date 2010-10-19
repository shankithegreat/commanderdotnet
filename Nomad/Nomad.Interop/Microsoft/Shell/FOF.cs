namespace Microsoft.Shell
{
    using System;

    public enum FOF : ushort
    {
        FOF_ALLOWUNDO = 0x40,
        FOF_CONFIRMMOUSE = 2,
        FOF_FILESONLY = 0x80,
        FOF_MULTIDESTFILES = 1,
        FOF_NO_CONNECTED_ELEMENTS = 0x2000,
        FOF_NOCONFIRMATION = 0x10,
        FOF_NOCONFIRMMKDIR = 0x200,
        FOF_NOCOPYSECURITYATTRIBS = 0x800,
        FOF_NOERRORUI = 0x400,
        FOF_NORECURSEREPARSE = 0x8000,
        FOF_NORECURSION = 0x1000,
        FOF_RENAMEONCOLLISION = 8,
        FOF_SILENT = 4,
        FOF_SIMPLEPROGRESS = 0x100,
        FOF_WANTMAPPINGHANDLE = 0x20,
        FOF_WANTNUKEWARNING = 0x4000
    }
}

