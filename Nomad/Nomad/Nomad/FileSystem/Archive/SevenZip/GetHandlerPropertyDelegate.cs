namespace Nomad.FileSystem.Archive.SevenZip
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int GetHandlerPropertyDelegate(ArchivePropId propID, ref PropVariant value);
}

